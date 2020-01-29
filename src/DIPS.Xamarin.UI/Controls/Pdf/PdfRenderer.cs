using System;
using DIPS.Xamarin.UI.Controls.Pdf.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Controls.Pdf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    internal class PdfRenderer : ContentView
    {
        public static readonly BindableProperty PageCountProperty = BindableProperty.Create(nameof(PageCount), typeof(int), typeof(PdfRenderer));

        public static readonly BindableProperty CurrentPageIndexProperty = BindableProperty.Create(
            nameof(CurrentPageIndex),
            typeof(int),
            typeof(PdfRenderer));

        public int CurrentPageIndex
        {
            get => (int)GetValue(CurrentPageIndexProperty);
            set => SetValue(CurrentPageIndexProperty, value);
        }

        public int PageCount
        {
            get => (int)GetValue(PageCountProperty);
            set => SetValue(PageCountProperty, value);
        }

        internal event EventHandler<PfdContentEventArgs>? OnShowPdfFromContent;
        internal event EventHandler<PdfFileEventArgs>? OnShowPdfFromFile;
        internal event EventHandler<PdfZoomEventArgs>? OnZoomPdf;

        internal void ShowPdf(byte[] content)
        {
            OnShowPdfFromContent?.Invoke(this, new PfdContentEventArgs(content));
        }

        internal void ShowPdf(string filePath)
        {
            OnShowPdfFromFile?.Invoke(this, new PdfFileEventArgs(filePath));
        }

        internal void ZoomPdf(double zoomFactor)
        {
            OnZoomPdf?.Invoke(this, new PdfZoomEventArgs(zoomFactor));
        }
    }
}