using System.Collections.Generic;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Models.Widgets.StaticHtmlWidget
{
    public class StaticHtmlWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextAreaComponent.IDENTIFIER, Label = "Html", Order = 0)]
        public string Html { get; set; }

        [EditingComponent(PageSelector.IDENTIFIER, Label = "Html chunk", Order = 1)]
        public IList<PageSelectorItem> StaticHtmlChunks { get; set; }
    }
}