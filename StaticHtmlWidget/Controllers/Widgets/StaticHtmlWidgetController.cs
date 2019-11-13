using System.Linq;
using System.Web.Mvc;
using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets.StaticHtmlWidget;
using DancingGoat.Repositories;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("DancingGoat.StaticHtmlWidget", typeof(StaticHtmlWidgetController), "Static HTML", Description = "Embed static HTML into the markup", IconClass = "icon-xml-tag")]

namespace DancingGoat.Controllers.Widgets
{
    public class StaticHtmlWidgetController : WidgetController<StaticHtmlWidgetProperties>
    {
        protected readonly IStaticHtmlChunkRepository mStaticHtmlChunkRepository;

        public StaticHtmlWidgetController(IStaticHtmlChunkRepository staticHtmlChunkRepository)
        {
            mStaticHtmlChunkRepository = staticHtmlChunkRepository;
        }

        protected StaticHtmlWidgetViewModel GetModel(StaticHtmlWidgetProperties properties)
        {
            var model = new StaticHtmlWidgetViewModel {Html = properties.Html};
            if (!string.IsNullOrWhiteSpace(model.Html)) return model;

            var selectedItem = properties.StaticHtmlChunks?.FirstOrDefault();
            if (selectedItem != null)
                model.Html = this.mStaticHtmlChunkRepository.GetByNodeGuid(selectedItem.NodeGuid).Html;

            return model;
        }

        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_StaticHtml", GetModel(properties));
        }
    }
}