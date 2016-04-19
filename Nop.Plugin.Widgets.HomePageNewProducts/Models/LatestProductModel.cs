using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.HomePageNewProducts.Models
{
    public class LatestProductModel: BaseNopModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public ProductPicture Picture { get; set; }
        public string SeName { get; set; }
    }
}
