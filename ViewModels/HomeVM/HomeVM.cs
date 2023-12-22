using SitePustok.Areas.Admin.ViewModels;
using SitePustok.ViewModels.CommonVM;
using SitePustok.ViewModels.SliderVM;

namespace SitePustok.ViewModels.HomeVM
{
    public class HomeVM
    {
        //public IEnumerable<SliderListItemVM> Sliders { get; set; }  //Componentile yazdim deye gerek yoxdur burada yazmaga
        public IEnumerable<AdminProductListItemVM> Products { get; set; }
        public PaginationVM<IEnumerable<AdminProductListItemVM>> PaginatedProducts { get; set; }
    }
}
