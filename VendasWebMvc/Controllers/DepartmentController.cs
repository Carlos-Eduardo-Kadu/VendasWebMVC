using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VendasWebMvc.Models;

namespace VendasWebMvc.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            List < Department > list = new List<Department> ();
            list.Add(new Department { Id = 1, Name = "Eletronic" });
            list.Add(new Department { Id = 2, Name = "Moda" });
            return View(list);
        }
    }
}
