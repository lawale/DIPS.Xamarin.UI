using System.Drawing;
using System.IO;
using System.Net;
using DIPS.Xamarin.UI.Controls.Pdf;
using DIPS.Xamarin.UI.Controls.Pdf.Events;
using DIPS.Xamarin.UI.iOS.Pdf;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfWebViewRenderer))]
namespace DIPS.Xamarin.UI.iOS.Pdf
{
    internal class PdfWebViewRenderer : ViewRenderer<PdfWebView, UIWebView>
    {
        public static void Initialize(){}
        protected override void OnElementChanged(ElementChangedEventArgs<PdfWebView>
            e)
        {
            base.OnElementChanged(e);
            var pdfWebView = Element as PdfWebView;
            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                pdfWebView.OnShowPdfFromFile -= ShowPdfFromFile;
            }
            if (e.NewElement != null)
            {
                pdfWebView.OnShowPdfFromFile += ShowPdfFromFile;
                Control.ScalesPageToFit = true;
            }

            this.Opaque = false;
        }

        private void ShowPdfFromFile(object sender, PdfFileEventArgs e)
        {
            var fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", WebUtility.UrlEncode(e.FilePath)));
            Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
        }
    }
}