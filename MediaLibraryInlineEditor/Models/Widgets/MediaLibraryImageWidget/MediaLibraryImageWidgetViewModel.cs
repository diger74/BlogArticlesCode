using System;
using diger74.Models.Media;
using diger74.Services;

namespace diger74.Models.Widgets
{
    public class MediaLibraryImageWidgetViewModel
    {
        public MediaLibraryImageWidgetViewModel(Guid imageGuid)
        {
            Image = MediaService.GetImageModel(imageGuid);
        }

        public ImageModel Image { get; set; }
    }
}