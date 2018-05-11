using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System.Collections.Generic;
using System;

namespace BandTracker.Tests
{
  [TestClass]
  public class VenueTests : IDisposable
  {

    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {

      int result = Venue.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Venue()
    {
      Venue firstVenue = new Venue("Algebra");
      Venue secondVenue = new Venue("Algebra");


      Assert.AreEqual(firstVenue, secondVenue);
    }

    [TestMethod]
    public void Save_SavesToDatabase_FlightList()
    {
      Venue testVenue = new Venue("Algebra");

      testVenue.Save();

      List<Venue> result = Venue.GetAll();
      List<Venue> testList = new List<Venue>{testVenue};
      //Console.WriteLine("result " + result.GetFlightName());
      Console.WriteLine("testList " + testList.Count);


      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsVenueIdToObject_Id()
    {
      //Arrange
      Venue testVenue = new Venue("Algebra");
      Venue testVenue2 = new Venue("English");

      //Act
      testVenue.Save();
      testVenue2.Save();
      Venue savedVenue = Venue.GetAll()[1];

      int result = savedVenue.GetId();
      int testId = testVenue2.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void AddVenue_AddsVenuetoBands_VenueList()
    {
      //Arrange
      Band testBand = new Band("James");
      testBand.Save();

      Venue testVenue = new Venue("Algebra");
      testVenue.Save();

      //Act
      testBand.AddVenue(testVenue);

      List<Venue> result = testBand.GetVenues();
      //CALL GetVenues()
      List<Venue> testList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesVenueAssociationsFromDatabase_VenueList()
    {
      //Arrange
      Band testBand = new Band("James");
      testBand.Save();

      string testName = "Algebra";
      Venue testVenue = new Venue(testName);
      testVenue.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.Delete();

      List<Venue> resultBandVenues = testBand.GetVenues();
      List<Venue> testBandVenues = new List<Venue> {};

      //Assert
      CollectionAssert.AreEqual(testBandVenues, resultBandVenues);
    }

    [TestMethod]
    public void Find_FindsVenueInDatabase_Venue()
    {
      //Arrange
      Venue testVenue = new Venue("English");
      testVenue.Save();

      //Act
      Venue result = Venue.Find(testVenue.GetId());

      //Assert
      Assert.AreEqual(testVenue, result);
    }
  }
}
