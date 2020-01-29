using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Controls.Pdf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfViewer : ContentView
    {
        public PdfViewer()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PdfFilePathProperty = BindableProperty.Create(nameof(PdfFilePath), typeof(string), typeof(PdfViewer), propertyChanged:OnPdfFilePathPropertyChanged);

        private static void OnPdfFilePathPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (!(bindable is PdfViewer pdfViewer)) return;
            if (Device.RuntimePlatform == Device.Android)
            {
                pdfViewer.PdfRenderer.ShowPdf((string)newvalue);
            }
            else
            {
                pdfViewer.PdfWebView.ShowPdf((string)newvalue);
            }
        }

        public string PdfFilePath
        {
            get => (string)GetValue(PdfFilePathProperty);
            set => SetValue(PdfFilePathProperty, value);
        }

        public static readonly BindableProperty PdfContentProperty = BindableProperty.Create(nameof(PdfContent), typeof(byte[]), typeof(PdfViewer), propertyChanged:OnPdfContentPropertyChanged);
        

        private static void OnPdfContentPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (!(bindable is PdfViewer pdfViewer)) return;
            pdfViewer.PdfRenderer.ShowPdf((byte[])newvalue);
        }

        public byte[] PdfContent
        {
            get => (byte[])GetValue(PdfContentProperty);
            set => SetValue(PdfContentProperty, value);
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            PdfRenderer.AnchorX = 0;
            PdfRenderer.AnchorY = 0;
            PdfRenderer.Scale = e.NewValue;
        }

        private void Reset(object sender, EventArgs e)
        {
            PdfRenderer.TranslationX = 0;
            PdfRenderer.TranslationY = 0;
            PdfRenderer.Scale = 1;
            slider.Value = 1;
            ZoomContainer.Reset();
        }
    }
}