using Xamarin.Forms.Maps;

namespace PaddelAppen.Models
{
    public class Location
    {
        /// <summary>
        /// A position on the map. 2 coordinates. Pretty much same as the Forms.Maps Position
        /// object only here we can set a default value in case shit fucks up.
        /// </summary>
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static Position DefaultPosition
        {
            get { return new Position(34.033897, -118.291869); }
        }
    }
}