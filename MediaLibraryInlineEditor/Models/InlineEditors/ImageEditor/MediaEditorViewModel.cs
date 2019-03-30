using System;

namespace diger74.Models.InlineEditors.ImageEditor
{
    public class MediaEditorViewModel : BaseInlineEditorViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string ImageFolderPath { get; set; }

        public Guid? ImageId { get; set; }

        public string FileName { get; set; }

        public int Order { get; set; }

        #region "in many" mode
        public int? ImageNumber { get; set; }

        public string ImagesJSON { get; set; }

        #endregion
    }
}