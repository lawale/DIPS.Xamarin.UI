using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Controls.Pdf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class PdfRenderer : ContentView
    {
        internal event EventHandler<ShowPdfEventArgs>? OnShowPdf;
        public void ShowPdf(byte[] content)
        {
            OnShowPdf?.Invoke(this, new ShowPdfEventArgs(content));
        }

        public void ZoomPdf()
        {
            
        }
    }

    internal class ShowPdfEventArgs : EventArgs
    {
        public byte[] Content { get; }

        public ShowPdfEventArgs(byte[] content)
        {
            Content = content;
        }
    }
}