using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Widget;
using DIPS.Xamarin.UI.Android.Pdf;
using DIPS.Xamarin.UI.Controls.Pdf;
using Java.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Console = System.Console;

[assembly: ExportRenderer(typeof(PdfViewer), typeof(PdfViewerRenderer))]
namespace DIPS.Xamarin.UI.Android.Pdf
{
    public class PdfViewerRenderer : ViewRenderer
    {
        public PdfViewerRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                try
                {
                    var view = (this.Context as Activity).LayoutInflater.Inflate(Resource.Layout.PdfViewerLayout, this, false);
                    SetNativeControl(view);

                    var cacheDir = (Context as Activity).CacheDir.AbsolutePath;
                    File fileCopy = new File(cacheDir, "sample.pdf");

                    // Copy content to file
                    CopyToLocalCache(fileCopy , Resource.Raw.sample);


                    // We will get a page from the PDF file by calling openPage
                    var fileDescriptor =
                        ParcelFileDescriptor.Open(fileCopy,
                            ParcelFileMode.ReadOnly);
                    var mPdfRenderer = new PdfRenderer(fileDescriptor);
                    var mPdfPage = mPdfRenderer.OpenPage(0);

                    // Create a new bitmap and render the page contents on to it
                    Bitmap bitmap = Bitmap.CreateBitmap(mPdfPage.Width,
                        mPdfPage.Height,
                        Bitmap.Config.Argb8888);
                    mPdfPage.Render(bitmap, null, null, PdfRenderMode.ForDisplay);

                    // Set the bitmap in the ImageView so we can view it
                    var imageView = new ImageView(Context);
                    imageView.SetImageBitmap(bitmap);
                    ((LinearLayout)view).AddView(imageView);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
            else
            {
                //Register events
            }
        }

        void CopyToLocalCache(File outputFile, int resourceId)
        {
            if (!outputFile.Exists())
            {
                var input = (this.Context as Activity).Resources.OpenRawResource(resourceId);
                var output = new FileOutputStream(outputFile);
                byte[] buffer = new byte[1024];
                int size;
                // Just copy the entire contents of the file
                while ((size = input.Read(buffer)) != -1)
                {
                    output.Write(buffer, 0, size);
                }

                input.Close();
                output.Close();
            }
        }
    }
}