using System.Threading.Tasks;
using AspEducation.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspEducation.Models.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly DataManager datamanager;

        SidebarViewComponent(DataManager datamanager)
        {
            this.datamanager = datamanager;
        }

        public Task<ViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult) View("Default", datamanager.ServiceItems.GetServiceItems()));
        }
    }
}