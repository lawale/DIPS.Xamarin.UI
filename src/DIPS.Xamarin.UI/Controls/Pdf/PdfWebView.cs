using System;
using System.Collections.Generic;
using System.Text;
using DIPS.Xamarin.UI.Controls.Pdf.Events;
using Xamarin.Forms;

namespace DIPS.Xamarin.UI.Controls.Pdf
{
    internal class PdfWebView : WebView
    {
        internal event EventHandler<PfdContentEventArgs>? OnShowPdfFromContent;
        internal event EventHandler<PdfFileEventArgs>? OnShowPdfFromFile;

        internal void ShowPdf(byte[] content)
        {
            OnShowPdfFromContent?.Invoke(this, new PfdContentEventArgs(content));
        }

        internal void ShowPdf(string filePath)
        {
            OnShowPdfFromFile?.Invoke(this, new PdfFileEventArgs(filePath));
        }
    }
}
