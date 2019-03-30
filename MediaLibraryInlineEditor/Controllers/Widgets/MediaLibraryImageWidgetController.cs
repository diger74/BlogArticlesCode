using System.Web.Mvc;
using diger74.Controllers.Widgets;
using diger74.Models.Widgets;
using Kentico.PageBuilder.Web.Mvc;

[assembly:
    RegisterWidget("MediaLibraryImageWidget", typeof(MediaLibraryImageWidgetController), "Media Library Image",
        Description = "Image from media library", IconClass = "icon-picture")]

namespace diger74.Controllers.Widgets
{
    public class MediaLibraryImageWidgetController : WidgetController<MediaLibraryImageWidgetProperties>
    {
        public MediaLibraryImageWidgetController()
        {
        }

        public MediaLibraryImageWidgetController(
            IWidgetPropertiesRetriever<MediaLibraryImageWidgetProperties> propertiesRetriever,
            ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }

        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_MediaLibraryImageWidget",
                new MediaLibraryImageWidgetViewModel(properties.ImageGuid));
        }
    }
}