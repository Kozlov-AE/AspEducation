using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspEducation.Domain;
using AspEducation.Domain.Entities;
using AspEducation.Sevice;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspEducation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceItemsController : Controller
    {
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public ServiceItemsController(DataManager dataManager, IWebHostEnvironment hostEnvironment)
        {
            this.dataManager = dataManager;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Edit(Guid id)
        {
            var entity = id == default ? new ServiceItem() : dataManager.ServiceItems.GetServiceItemById(id);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(ServiceItem model, IFormFile titleImageFile)
        {
            if(ModelState.IsValid)
            {
                model.TitleImagePath = titleImageFile.FileName;
                using (var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "images/", titleImageFile.FileName),FileMode.Create))
                {
                    titleImageFile.CopyTo(stream);
                }
                dataManager.ServiceItems.SaveServiceItem(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            dataManager.ServiceItems.DeleteServiceItem(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}
