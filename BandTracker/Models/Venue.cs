using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
    public class Venue
    {
        private int _id;
        private string _name;

        public Venue(string name, int id=0)
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

        public void AddBand(Band newBand)
        {
          MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

                MySqlParameter band_id = new MySqlParameter();
                band_id.ParameterName = "@BandId";
                band_id.Value = newBand.GetId();
                cmd.Parameters.Add(band_id);

                MySqlParameter venue_id = new MySqlParameter();
                venue_id.ParameterName = "@VenueId";
                venue_id.Value = _id;
                cmd.Parameters.Add(venue_id);

                cmd.ExecuteNonQuery();
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }

        }

        public static List<Venue> GetAll()
        {
          List<Venue> allVenues = new List<Venue> {};
          MySqlConnection conn = DB.Connection();

          conn.Open();

          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

          cmd.CommandText = @"SELECT * FROM venues;";
          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

          while(rdr.Read())
          {
            int venueId = rdr.GetInt32(0);
            string venueName = rdr.GetString(1);

            Venue newVenue = new Venue(venueName, venueId);

            allVenues.Add(newVenue);
          }

          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return allVenues;
        }

        public List<Band> GetBands()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT bands.* FROM venues
            JOIN venues_bands ON (venues.id = venues_bands.venue_id)
            JOIN bands ON (venues_bands.band_id = bands.id)
            WHERE venues.id = @VenueId;";
            // @"SELECT band_id FROM venues_bands WHERE venue_id = @VenueId;";

            MySqlParameter venueIdParameter = new MySqlParameter();
            venueIdParameter.ParameterName = "@VenueId";
            venueIdParameter.Value = _id;
            cmd.Parameters.Add(venueIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Band> bands = new List<Band>{};

            while(rdr.Read())
            {
              int bandId = rdr.GetInt32(0);
              string bandName = rdr.GetString(1);
              Band newBand = new Band(bandName, bandId);
              bands.Add(newBand);
            }

            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return bands;
        }

        public void Delete()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM venues WHERE id = @VenueId; DELETE FROM venues_bands WHERE venue_id = @VenueId;";

          MySqlParameter venueIdParameter = new MySqlParameter();
          venueIdParameter.ParameterName = "@VenueId";
          venueIdParameter.Value = this.GetId();
          cmd.Parameters.Add(venueIdParameter);

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
            cmd.CommandText = @"UPDATE venues SET name = @newDescription WHERE id = @searchId;";

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
          cmd.CommandText = @"DELETE FROM venues;";

          cmd.ExecuteNonQuery();

          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }

        public override bool Equals(System.Object otherVenue)
        {
          if (!(otherVenue is Venue))
          {
            return false;
          }
          else
          {
            Venue newVenue = (Venue) otherVenue;
            bool idEquality = (this.GetId() == newVenue.GetId());
            //when we change an object from one type to another, its called "TYPE CASTING"
            bool nameEquality = (this.GetName() == newVenue.GetName());
            return (idEquality && nameEquality);
          }
        }

        public void Save()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO venues (name) VALUES (@name);";


          cmd.Parameters.Add(new MySqlParameter("@name", _name));


          cmd.ExecuteNonQuery();
          _id = (int) cmd.LastInsertedId;

          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }

        public static Venue Find(int id)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = id;
          cmd.Parameters.Add(searchId);

          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          int venueId = 0;
          string venueName = "";

          while(rdr.Read())
          {
            venueId = rdr.GetInt32(0);
            venueName = rdr.GetString(1);
          }

          // Constructor below no longer includes a itemCategoryId parameter:
          Venue newVenue = new Venue(venueName, venueId);
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }

          return newVenue;
        }
      }
    }
