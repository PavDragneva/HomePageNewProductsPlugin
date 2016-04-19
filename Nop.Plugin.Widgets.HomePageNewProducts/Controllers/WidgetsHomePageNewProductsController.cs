using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.HomePageNewProducts.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System.Linq;
using System.Web.Mvc;
using Nop.Services.Seo;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Controllers
{
    public class WidgetsHomePageNewProductsController : BasePluginController
    {
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;

        public WidgetsHomePageNewProductsController(
            IWorkContext workContext,
            IStoreService storeService,
            ISettingService settingService,
            ILocalizationService localizationService,
            IProductService productService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._productService = productService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);
            var model = new ConfigurationModel();
            model.WidgetZone = homePageNewProductsSettings.WidgetZone;
            model.NumberOfProducts = homePageNewProductsSettings.NumberOfProducts;

            model.ActiveStoreScopeConfiguration = storeScope;

            if (storeScope > 0)
            {
                model.WidgetZone_OverrideForStore = _settingService.SettingExists(homePageNewProductsSettings, x => x.WidgetZone, storeScope);
                model.NumberOfProducts_OverrideForStore = _settingService.SettingExists(homePageNewProductsSettings, x => x.NumberOfProducts, storeScope);
            }

            return View("~/Plugins/Widgets.HomePageNewProducts/Views/WidgetsHomePageNewProducts/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (ModelState.IsValid)
            {
                var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
                var homePageNewProductsSettings = _settingService.LoadSetting<HomePageNewProductsSettings>(storeScope);
                homePageNewProductsSettings.WidgetZone = model.WidgetZone;
                homePageNewProductsSettings.NumberOfProducts = model.NumberOfProducts;

                if (model.WidgetZone_OverrideForStore || storeScope == 0)
                    _settingService.SaveSetting(homePageNewProductsSettings, x => x.WidgetZone, storeScope, false);
                else if (storeScope > 0)
                    _settingService.DeleteSetting(homePageNewProductsSettings, x => x.WidgetZone, storeScope);

                if (model.NumberOfProducts_OverrideForStore || storeScope == 0)
                    _settingService.SaveSetting(homePageNewProductsSettings, x => x.NumberOfProducts, storeScope, false);
                else if (storeScope > 0)
                    _settingService.DeleteSetting(homePageNewProductsSettings, x => x.NumberOfProducts, storeScope);

                _settingService.ClearCache();

                SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            }
            else
            {
                ErrorNotification("Pleace fill the number of products field with valid value.");
            }

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            var numberOfProducts = this._settingService.GetSettingByKey<int>("HomePageNewProductsSettings.NumberOfProducts");
            var products = _productService.SearchProducts(orderBy: ProductSortingEnum.CreatedOn).Take(numberOfProducts);
            var productsModel = products
              .Select(x =>
              {
                  var latestModel = new LatestProductModel
                  {
                      ProductId = x.Id,
                      Name = x.Name,
                      ShortDescription = x.ShortDescription,
                      Picture = x.ProductPictures.FirstOrDefault(),
                      SeName = x.GetSeName()
                  };
                  return latestModel;
              })
              .ToList();
            return View("~/Plugins/Widgets.HomePageNewProducts/Views/WidgetsHomePageNewProducts/PublicInfo.cshtml", productsModel);
        }
    }
}
