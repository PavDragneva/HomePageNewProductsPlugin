using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.Collections.Generic;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.HomePageNewProducts
{
    public class HomePageNewProductsPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;

        public HomePageNewProductsPlugin(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsHomePageNewProducts";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.HomePageNewProducts.Controllers" }, { "area", null } };
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsHomePageNewProducts";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Widgets.HomePageNewProducts.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> { this._settingService.GetSettingByKey<string>("HomePageNewProductsSettings.WidgetZone")};
        }

        public override void Install()
        {
            var settings = new HomePageNewProductsSettings
            {
                WidgetZone = WidgetZones.home_page_before_products,
                NumberOfProducts = 4
            };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.WidgetZone", "Widget zone");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.NumberOfProducts", "Number of products to display");

           base.Install();
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<HomePageNewProductsSettings>();

            this.DeletePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.WidgetZone");
            this.DeletePluginLocaleResource("Plugins.Widgets.HomePageNewProducts.NumberOfProducts");

            base.Uninstall();
        }
    }
}
