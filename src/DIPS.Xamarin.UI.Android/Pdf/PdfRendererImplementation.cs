using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Widget;
using DIPS.Xamarin.UI.Android.Pdf;
using DIPS.Xamarin.UI.Controls.Pdf.Events;
using Java.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using File = Java.IO.File;

[assembly: ExportRenderer(typeof(DIPS.Xamarin.UI.Controls.Pdf.PdfRenderer), typeof(PdfRendererImplementation))]

namespace DIPS.Xamarin.UI.Android.Pdf
{
    internal class PdfRendererImplementation : ViewRenderer
    {
        private readonly Activity m_mainActivity;
        private PdfRenderer.Page m_currentPage;
        private Controls.Pdf.PdfRenderer m_formsPdfRenderer;
        private ImageView m_imageView;
        private PdfRenderer m_pdfRenderer;

        public PdfRendererImplementation(Context context) : base(context)
        {
            m_mainActivity = (Activity)Context;
        }

        public static void Initialize() { }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            m_formsPdfRenderer = (Controls.Pdf.PdfRenderer)e.NewElement;
            base.OnElementChanged(e);
            if (Control == null)
            {
                m_formsPdfRenderer.OnShowPdfFromContent += ShowFormsPdfFromContent;
                m_formsPdfRenderer.OnShowPdfFromFile += ShowFormsPdfFromFile;
                m_formsPdfRenderer.OnZoomPdf += ZoomPdf;
                m_imageView = new ImageView(Context);
                SetNativeControl(m_imageView);
            }
        }

        private void ZoomPdf(object sender, PdfZoomEventArgs e)
        {
            AddCurrentPageToImage((int)e.ZoomFactor);   
        }

        private void ShowFormsPdfFromFile(object sender, PdfFileEventArgs e)
        {
            AddPdfToView(new File(e.FilePath));
        }

        private async void ShowFormsPdfFromContent(object sender, PfdContentEventArgs e)
        {
            //Create cached file
            var cacheDir = m_mainActivity.CacheDir.AbsolutePath;
            var cachedFile = new File(cacheDir, "cachedPdf.pdf");

            // Copy content to cached file
            await CopyContentToFile(cachedFile, e.Content);

            //Read cached file and add it to the view
            AddPdfToView(cachedFile);

            //Remove the cached file
            cachedFile.Delete();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            m_formsPdfRenderer.OnShowPdfFromContent -= ShowFormsPdfFromContent;
            m_formsPdfRenderer.OnShowPdfFromFile -= ShowFormsPdfFromFile;
            m_formsPdfRenderer.OnZoomPdf -= ZoomPdf;

            m_currentPage?.Close();
            m_pdfRenderer?.Close();

            m_imageView.Dispose();
        }

        private void AddPdfToView(File cachedFile)
        {
            // We will get a page from the PDF file by calling openPage
            var fileDescriptor = ParcelFileDescriptor.Open(cachedFile, ParcelFileMode.ReadOnly);
            m_pdfRenderer = new PdfRenderer(fileDescriptor);

            m_currentPage?.Close();
            m_currentPage = m_pdfRenderer.OpenPage(0);

            AddCurrentPageToImage();

            m_formsPdfRenderer.PageCount = m_pdfRenderer.PageCount;
            m_formsPdfRenderer.CurrentPageIndex = m_currentPage.Index+1;
        }

        private void AddCurrentPageToImage(int zoomFactor = 1)
        {
            // Create a new bitmap and render the page contents on to it
            var bitmap = Bitmap.CreateBitmap(m_currentPage.Width*zoomFactor, m_currentPage.Height*zoomFactor, Bitmap.Config.Argb8888);
            m_currentPage.Render(bitmap, null, null, PdfRenderMode.ForDisplay);

            // Set the bitmap in the ImageView so we can view it
            m_imageView.SetImageBitmap(bitmap);
        }

        /// <summary>
        ///     From file
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="resourceId"></param>
        private void CopyStreamToFile(File outputFile, Stream input)
        {
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
        private static async Task CopyContentToFile(File outputFile, byte[] content)
        {
            var output = new FileOutputStream(outputFile);
            await output.WriteAsync(content);
            output.Close();
        }
    }
}