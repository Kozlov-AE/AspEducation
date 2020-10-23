using System;
using AspEducation.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspEducation.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataManager dataManager;

        public ServicesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        // GET
        public IActionResult Index(Guid id)
        {
            if (id != default)
            return View("Show",dataManager.ServiceItems.GetServiceItemById(id));

            ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("PageServices");
            return View(dataManager.ServiceItems.GetServiceItems());
        }
    }
}