using System;

namespace DIPS.Xamarin.UI.Samples.Controls.Pdf
{
    public class DocumentMetadata
    {
        public DocumentMetadata() { }

        public string Author { get; set; }
        public string Department { get; set; }
        public string DocumentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; }
    }

    public class GetDocumentMetadataResponse
    {
        public DocumentMetadata DocumentMetadataInfo { get; set; }
    }
}