using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System.Collections.Generic;
using System;

namespace BandTracker.Tests
{
      [TestClass]
      public class BandTests : IDisposable
      {

        public void Dispose()
        {
          Band.DeleteAll();
          Venue.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_0()
        {

          int result = Band.GetAll().Count;

          Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfNamesAreTheSame_Band()
        {
          Band firstBand = new Band("James");
          Band secondBand = new Band("James");


          Assert.AreEqual(firstBand, secondBand);
        }

        [TestMethod]
        public void Save_SavesToDatabase_BandList()
        {
          Band testBand = new Band("James");

          testBand.Save();
          List<Band> result = Band.GetAll();
          List<Band> testList = new List<Band>{testBand};
          Console.WriteLine("result " + result.Count);
          Console.WriteLine("testList " + testList.Count);


          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsBandIdToObject_Id()
        {
          //Arrange
          Band testBand = new Band("James");
          Band testBand2 = new Band("Alex");

          //Act
          testBand.Save();
          testBand2.Save();
          Band savedBand = Band.GetAll()[1];

          int result = savedBand.GetId();
          int testId = testBand2.GetId();

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
        public void GetVenues_ReturnsAllBandsVenues_VenuesList()
        {
          //Arrange
          Band testBand = new Band("James");
          testBand.Save();

          Venue testVenue1 = new Venue("Algebra");
          testVenue1.Save();

          Venue testVenue2 = new Venue("English");
          testVenue2.Save();

          //Act
          testBand.AddVenue(testVenue1);
          //CALL AddVenue()
          List<Venue> result = testBand.GetVenues();
          List<Venue> testList = new List<Venue> {testVenue1};

          //Assert
          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Delete_DeletesBandAssociationsFromDatabase_BandList()
        {
          //Arrange
          Venue testVenue = new Venue("Algebra");
          testVenue.Save();

          string testName = "James";
          Band testBand = new Band(testName);
          testBand.Save();

          //Act
          testBand.AddVenue(testVenue);
          testBand.Delete();
          //CALL Delete()

          List<Band> resultVenueBands = testVenue.GetBands();
          List<Band> testVenueBands = new List<Band> {};

          //Assert
          CollectionAssert.AreEqual(testVenueBands, resultVenueBands);
        }

        [TestMethod]
        public void Find_FindsBandInDatabase_Band()
        {
          //Arrange
          Band testBand = new Band("Oprah");
          testBand.Save();

          //Act
          Band result = Band.Find(testBand.GetId());

          //Assert
          Assert.AreEqual(testBand, result);
        }
    }
}
