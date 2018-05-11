using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System.Collections.Generic;

namespace BandTracker.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
        return View();
    }
  }
}
