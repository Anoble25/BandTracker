using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System.Collections.Generic;

namespace BandTracker.Controllers
{
  public class BandController : Controller
  {
    [HttpGet("/bands")]
    public ActionResult Index()
    {
      List<Band> allBands = Band.GetAll();
      return View(allBands);
    }

    [HttpGet("/bands/new")]
    public ActionResult CreateForm()
    {
        return View();
    }
    [HttpPost("/bands")]
    public ActionResult Create()
    {
        Band newBand = new Band(Request.Form["band-name"]);
        newBand.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/bands/delete")]
    public ActionResult DeleteBand()
    {
        List<Band> allBands = Band.GetAll();
        return View(allBands);
    }

    [HttpPost("/bands/delete")]
    public ActionResult DeletePost()
    {
        int id= int.Parse(Request.Form["band-delete-dropdown"]);
        Band selectedBand=Band.Find(id);
        selectedBand.Delete();
        return RedirectToAction("Index");
    }

    [HttpGet("/bands/update")]
    public ActionResult UpdateBand()
    {
        List<Band> allBands = Band.GetAll();
        return View(allBands);
    }

    [HttpPost("/bands/update")]
    public ActionResult UpdatePost()
    {
        int id= int.Parse(Request.Form["band-update-dropdown"]);
        Band selectedBand=Band.Find(id);

        string newName=Request.Form["new-band-name"];

        selectedBand.Update(newName);
        return RedirectToAction("Index");
    }


    [HttpGet("/bands/{id}")]
    public ActionResult Details(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Band selectedBand = Band.Find(id);
        List<Venue> bandVenues = selectedBand.GetVenues();
        List<Venue> allVenues = Venue.GetAll();
        model.Add("selectedBand", selectedBand);
        model.Add("bandVenues", bandVenues);
        model.Add("allVenues", allVenues);
        return View(model);

    }

    [HttpPost("/bands/{bandId}/venues/new")]
    public ActionResult AddVenue(int bandId)
    {
        Band band = Band.Find(bandId);
        Venue venue = Venue.Find(int.Parse(Request.Form["venue-id"]));
        band.AddVenue(venue);
        return RedirectToAction("Details",  new { id = bandId });
    }
  }
}
