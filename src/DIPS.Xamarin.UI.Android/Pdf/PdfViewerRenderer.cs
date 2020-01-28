using System.IO;
using System.Threading.Tasks;
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
using File = Java.IO.File;
using PdfRenderer = DIPS.Xamarin.UI.Controls.Pdf.PdfRenderer;
using RelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(PdfRenderer), typeof(PdfViewerRenderer))]

namespace DIPS.Xamarin.UI.Android.Pdf
{
    public class PdfViewerRenderer : ViewRenderer
    {
        private readonly Activity m_mainActivity;

        public PdfViewerRenderer(Context context) : base(context)
        {
            m_mainActivity = Context as Activity;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            var pdfRenderer = (PdfRenderer)e.NewElement;
            base.OnElementChanged(e);
            if (Control == null)
                pdfRenderer.OnShowPdf += PdfRenderer_OnOnShowPdf;
            else
                pdfRenderer.OnShowPdf -= PdfRenderer_OnOnShowPdf;
        }

        private async void PdfRenderer_OnOnShowPdf(object sender, ShowPdfEventArgs e)
        {
            //Clean up old cached file

            var cacheDir = m_mainActivity.CacheDir.AbsolutePath;
            var fileCopy = new File(cacheDir, "sample.pdf");

            // Copy content to file
            await CopyContentToFile(fileCopy, e.Content);

            AddPdfToView(fileCopy);
        }

        private void AddPdfToView(File cachedFile)
        {
            var view = m_mainActivity.LayoutInflater.Inflate(Resource.Layout.PdfViewerLayout, this, false);
            SetNativeControl(view);

            // We will get a page from the PDF file by calling openPage
            var fileDescriptor = ParcelFileDescriptor.Open(cachedFile, ParcelFileMode.ReadOnly);
            var mPdfRenderer = new global::Android.Graphics.Pdf.PdfRenderer(fileDescriptor);
            var mPdfPage = mPdfRenderer.OpenPage(0);

            // Create a new bitmap and render the page contents on to it
            var bitmap = Bitmap.CreateBitmap(mPdfPage.Width, mPdfPage.Height, Bitmap.Config.Argb8888);
            mPdfPage.Render(bitmap, null, null, PdfRenderMode.ForDisplay);

            // Set the bitmap in the ImageView so we can view it
            var imageView = new ImageView(Context);
            imageView.SetImageBitmap(bitmap);
            ((RelativeLayout)view).AddView(imageView);
        }

        /// <summary>
        ///     From file
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="resourceId"></param>
        private void CopyStreamToFile(File outputFile, Stream input)
        {
            //var input = (Context as Activity).Resources.OpenRawResource(resourceId);
            var output = new FileOutputStream(outputFile);
            var buffer = new byte[1024];
            int size;
            // Just copy the entire contents of the file
            while ((size = input.Read(buffer)) != -1) output.Write(buffer, 0, size);

            input.Close();
            output.Close();
        }

        /// <summary>
        ///     From content
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task CopyContentToFile(File outputFile, byte[] content)
        {
            var output = new FileOutputStream(outputFile);
            await output.WriteAsync(content);
            output.Close();
        }
    }
}