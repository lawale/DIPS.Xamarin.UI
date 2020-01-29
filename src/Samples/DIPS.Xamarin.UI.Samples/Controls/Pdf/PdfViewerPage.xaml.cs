using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DIPS.Xamarin.UI.Extensions;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Samples.Controls.Pdf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfViewerPage : ContentPage
    {
        public PdfViewerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((PdfViewerPageViewModel)BindingContext).Initialize();
        }
    }

    public class PdfViewerPageViewModel : INotifyPropertyChanged
    {
        private byte[] m_pdfContent;
        private string m_pdfFilePath;

        public byte[] PdfContent
        {
            get => m_pdfContent;
            set => PropertyChanged.RaiseWhenSet(ref m_pdfContent, value);
        }

        public string PdfFilePath
        {
            get => m_pdfFilePath;
            set => PropertyChanged.RaiseWhenSet(ref m_pdfFilePath, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize()
        {
            var assembly = typeof(PdfViewerPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.testdata.json");

            using (var reader = new System.IO.StreamReader(stream))
            {

                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<GetDocumentMetadataResponse>(json);
                PdfContent = data.DocumentMetadataInfo.Content;
            }

            
        }
    }
}