using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.ViewComponents
{
    public class LatestProductsGridViewComponent : ViewComponent
    {
        private readonly ApiService _apiService;

        public LatestProductsGridViewComponent(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _apiService.GetLatestProductsAsync();
            return View(products);
        }
    }
}
