using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MoneyMind.Controllers
{
    public class SobreController : Controller
    {
        // Ação para a página inicial
        public ActionResult Index()
        {
            return View();
        }

        // Ação para a página de contato
        public ActionResult Contact()
        {
            return View();
        }
    }
}

