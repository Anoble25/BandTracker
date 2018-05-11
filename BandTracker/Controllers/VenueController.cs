using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System.Collections.Generic;

namespace BandTracker.Controllers
{
  public class VenueController : Controller
  {
    [HttpGet("/venues")]
    public ActionResult Index()
    {
      List<Venue> allVenues = Venue.GetAll();
      return View(allVenues);
    }

    [HttpGet("/venues/new")]
    public ActionResult CreateForm()
    {
        return View();
    }
    [HttpPost("/venues")]
    public ActionResult Create()
    {
        Venue newVenue = new Venue(Request.Form["venue-name"]);
        newVenue.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/venues/{id}")]
    public ActionResult Details(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue selectedVenue = Venue.Find(id);
        List<Band> venueBands = selectedVenue.GetBands();
        List<Band> allBands = Band.GetAll();
        model.Add("selectedVenue", selectedVenue);
        model.Add("venueBands", venueBands);
        model.Add("allBands", allBands);
        return View(model);
    }

    [HttpGet("/venues/delete")]
    public ActionResult DeleteVenue()
    {
        List<Venue> allVenues = Venue.GetAll();
        return View(allVenues);
    }

    [HttpPost("/venues/delete")]
    public ActionResult DeletePost()
    {
        int id= int.Parse(Request.Form["venue-delete-dropdown"]);
        Venue selectedVenue=Venue.Find(id);
        selectedVenue.Delete();
        return RedirectToAction("Index");
    }

    [HttpGet("/venues/update")]
    public ActionResult UpdateVenue()
    {
        List<Venue> allVenues = Venue.GetAll();
        return View(allVenues);
    }

    [HttpPost("/venues/update")]
    public ActionResult UpdatePost()
    {
        int id= int.Parse(Request.Form["venue-update-dropdown"]);
        Venue selectedVenue=Venue.Find(id);

        string newName=Request.Form["new-venue-name"];

        selectedVenue.Update(newName);
        return RedirectToAction("Index");
    }



    [HttpPost("/venues/{venueId}/bands/new")]
    public ActionResult AddBand(int venueId)
    {
        Venue venue = Venue.Find(venueId);
        Band band = Band.Find(int.Parse(Request.Form["band-id"]));
        venue.AddBand(band);
        return RedirectToAction("Details",  new { id = venueId });
    }
  }
}
