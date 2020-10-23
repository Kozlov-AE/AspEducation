using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspEducation.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AspEducation.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public IActionResult Index(string text)
        {
            return View(dataManager.TextFields.GetTextFieldByCodeWord("PageIndex"));
        }
        
        public  IActionResult Contacts(string text)
        {
            return View(dataManager.TextFields.GetTextFieldByCodeWord("PageContacts"));
        }

    }
}
