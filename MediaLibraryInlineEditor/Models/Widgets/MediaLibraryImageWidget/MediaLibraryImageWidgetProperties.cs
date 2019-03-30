using System;
using Kentico.PageBuilder.Web.Mvc;

namespace diger74.Models.Widgets
{
    public sealed class MediaLibraryImageWidgetProperties : IWidgetProperties
    {
        public Guid ImageGuid { get; set; }
    }
}