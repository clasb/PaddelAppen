using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaddelAppen.Models;
using PaddelAppen.Extensions;
using System.Collections.ObjectModel;

namespace PaddelAppen.Controls
{
    public class CustomPin
    {
        /// <summary>
        /// This CustomPin class is needed to implement the necessary additions to pins 
        /// to use together with the custom renderer to the Pin class.
        /// </summary>
        PointOfInterest Point;
        
        public CustomPin(string name, string details, double latitude, double longitude)
        {
            Name = name;
            Details = details;
            Location = new Location { Latitude = latitude, Longitude = longitude };
        }

        public CustomPin(PointOfInterest poi)
        {
            Point = poi;
            Name = poi.Name;
            Details = poi.Notes;
            Location = new Location() { Latitude = poi.Lat, Longitude = poi.Long };
            Type = poi.Type;
            Trail = poi.Trail;
            Score = poi.Score;
            ID = poi.ID;
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public MapExtensions.LocationType Type { get; set; }
        public string Trail { get; set; }
        public double Score { get; set; }
        public Location Location { get; set; }
        public int ID { get; set; }

        public ObservableCollection<Location> GetTrailCollection()
        {
            return MapExtensions.MakeTrailPoints(Trail);
        }
    }
}
