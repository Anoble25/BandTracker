using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace BandTracker.Models
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band (string name, int id=0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band> {};
      MySqlConnection conn = DB.Connection();

      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM bands;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);

        Band newBand = new Band(bandName, bandId);

        allBands.Add(newBand);

      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands WHERE id = @BandId; DELETE FROM venues_bands WHERE band_id = @BandId;";

      MySqlParameter bandIdParameter = new MySqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bandIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Update(string newDescription)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE bands SET name = @newDescription WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@newDescription";
        description.Value = newDescription;
        cmd.Parameters.Add(description);

        cmd.ExecuteNonQuery();
        _name = newDescription;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = (this.GetId() == newBand.GetId());
        bool nameEquality = (this.GetName() == newBand.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = _name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddVenue(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = newVenue.GetId();
      cmd.Parameters.Add(venue_id);

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = _id;
      cmd.Parameters.Add(band_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }

    }

    public List<Venue> GetVenues()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venues.* FROM bands
      JOIN venues_bands ON (bands.id = venues_bands.band_id)
      JOIN venues ON (venues_bands.venue_id = venues.id)
      WHERE bands.id = @BandId;";
      // @"SELECT band_id FROM venues_bands WHERE venue_id = @VenueId;";

      MySqlParameter bandIdParameter = new MySqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = _id;
      cmd.Parameters.Add(bandIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Venue> venues = new List<Venue>{};

      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        Venue newVenue = new Venue(venueName, venueId);
        venues.Add(newVenue);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return venues;
    }

    public static Band Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int bandId = 0;
      string bandName = "";

      while(rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
      }

      // Constructor below no longer includes a itemCategoryId parameter:
      Band newBand = new Band(bandName, bandId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }

      return newBand;
    }
  }
}
