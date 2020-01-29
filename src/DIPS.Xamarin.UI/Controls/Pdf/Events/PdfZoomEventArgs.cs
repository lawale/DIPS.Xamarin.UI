using System;

namespace DIPS.Xamarin.UI.Controls.Pdf.Events {
    internal class PdfZoomEventArgs : EventArgs
    {
        public double ZoomFactor { get; }

        public PdfZoomEventArgs(double zoomFactor)
        {
            ZoomFactor = zoomFactor;
        }
    }
}