using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    class HomePageNewProductsSettings: ISettings
    {
        public WidgetZones WidgetZone { get; set; }

        public int NumberOfProducts { get; set; }
    }
}
