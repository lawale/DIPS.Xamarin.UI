using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using DIPS.Xamarin.UI.Android.Pdf;
using DIPS.Xamarin.UI.Controls.Pdf;
using Java.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

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
                var view = (this.Context as Activity).LayoutInflater.Inflate(Resource.Layout.PdfViewerLayout, this, false);
                SetNativeControl(view);
                File fileCopy = new File(GetCa(), FILE_NAME);
                copyToLocalCache();
            }
            else
            {
                //Register events
            }
        }

        void copyToLocalCache(File outputFile)
        {
            if (!outputFile.Exists())
            {
                var input = (this.Context as Activity).Resources.OpenRawResource(Resource.Raw.sample);
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