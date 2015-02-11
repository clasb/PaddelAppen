using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using Xamarin.Forms.Maps;
using PaddelAppen.Extensions;
using System.Collections.ObjectModel;
using PaddelAppen.Models;

namespace PaddelAppen
{
    public class PointOfInterest
    {
     /// <summary>
     /// Representation of a place on the map that can contain information about said place
     /// and also has SQL-attributes so that it can be saved in a database.
     /// </summary>
   
        public PointOfInterest()
        {
        }

        public ObservableCollection<Location> GetTrailCollection()
        {
            return MapExtensions.MakeTrailPoints(Trail);
        }

        public void SetDistance()
        {
            this.Distance = MapExtensions.Distance(GetTrailCollection());
        }

        public double RoundedDistanceKilometers
        { get { return System.Math.Round(Distance); } }

        public double RoundedDistanceMeters
        { get { return System.Math.Round(Distance * 1000); } }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public String Name { get; set; }
        public String Notes { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Distance { set; get; }
        public double Score { get; set; }
        public MapExtensions.LocationType Type { get; set; }
        public String Trail { get; set; }
        public String Features { get; set; }
    }
}
