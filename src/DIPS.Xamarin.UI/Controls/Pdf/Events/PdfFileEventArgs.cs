using System;

namespace DIPS.Xamarin.UI.Controls.Pdf.Events {
    internal class PdfFileEventArgs : EventArgs
    {
        public string FilePath { get; }

        public PdfFileEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}