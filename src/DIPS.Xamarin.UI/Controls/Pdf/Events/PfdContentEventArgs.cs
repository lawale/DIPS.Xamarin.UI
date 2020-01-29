using System;

namespace DIPS.Xamarin.UI.Controls.Pdf.Events {
    internal class PfdContentEventArgs : EventArgs
    {
        public byte[] Content { get; }

        public PfdContentEventArgs(byte[] content)
        {
            Content = content;
        }
    }
}