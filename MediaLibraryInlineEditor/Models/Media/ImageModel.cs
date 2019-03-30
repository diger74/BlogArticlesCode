using System;

namespace diger74.Models.Media
{
    public class ImageModel
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string FullFileName { get; set; }

        public string FolderPath { get; set; }

        public string FileExtension { get; set; }

        public DateTime? UploadDate { get; set; }
    }
}