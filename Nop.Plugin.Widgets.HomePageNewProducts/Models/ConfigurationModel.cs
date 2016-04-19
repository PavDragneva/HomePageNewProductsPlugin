using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Models
{
    public class ConfigurationModel:BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.HomePageNewProducts.WidgetZone")]
        public WidgetZones WidgetZone { get; set; }
        public bool WidgetZone_OverrideForStore { get; set; }

        [Range(1, 20)]
        [NopResourceDisplayName("Plugins.Widgets.HomePageNewProducts.NumberOfProducts")]
        public int NumberOfProducts { get; set; }
        public bool NumberOfProducts_OverrideForStore { get; set; }
    }
}
