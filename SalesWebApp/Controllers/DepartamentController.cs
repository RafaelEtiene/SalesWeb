using Microsoft.AspNetCore.Mvc;
using SalesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebApp.Controllers
{
    public class DepartamentController : Controller
    {
        public IActionResult Index()
        {
            List<Departament> list = new List<Departament>();
            list.Add(new Departament { Id = 1, Name = "Eletronics" });
            list.Add(new Departament { Id = 2, Name = "Fashion" });

            return View(list);
        }
    }
}
