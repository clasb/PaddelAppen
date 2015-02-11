using Xamarin.Forms.Maps;
using PaddelAppen.Models;
using System.Collections.Generic;
using System.Linq;
using PaddelAppen.Controls;
using System.Collections.ObjectModel;
using System;

namespace PaddelAppen.Extensions
{
    public static class MapExtensions
    {
        public enum LocationType
        {
            Trail = 1,
            Camp,
            Rental,
            Launch,
            CurrentLocation
        }

        /// <summary>
        /// Make actual map pins out of the CustomPin objects that holds a bit of extra information
        /// with the help of the AsPin method which returns a regular map pin.
        /// </summary>
        /// <returns>List of Pin objects</returns>
        public static IList<Pin> ToPins<T>(this IEnumerable<T> items) where T : CustomPin
        {
            return items.Select(i => i.AsPin()).ToList();
        }

        /// <summary>
        /// Makes a Pin object out of a CustomPing (only actualy Pin objects can be displayed on the map).
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Pin object</returns>
        public static Pin AsPin(this CustomPin item)
        {
            var location = item.Location;
            var position = location != null ? new Position(location.Latitude, location.Longitude) : Location.DefaultPosition;
            return new Pin { Label = item.Name, Address = item.Details, Position = position };
        }

        /// <summary>
        /// Makes a string of trail sub-coordinates from a list to make it savable in the database.
        /// String is separated by ';' between latitude and longitude and '-' between coordinates.
        /// </summary>
        /// <param name="TrailPoints">List of Location objects</param>
        /// <returns>String representation of coordinates</returns>
        public static string MakeTrailString(ObservableCollection<Location> TrailPoints)
        {
            string result = string.Empty;
            if (TrailPoints != null)
            {
                foreach (var p in TrailPoints)
                {
                    result += p.Latitude.ToString();
                    result += ";";
                    result += p.Longitude.ToString();
                    if (!p.Equals(TrailPoints.Last()))
                        result += "|";
                }
            }
            return result;
        }

        /// <summary>
        /// Makes a Collection of Locations from a string param by splitting it into coordinates and
        /// making Location objects to add to the Collection.
        /// </summary>
        /// <param name="TrailString">String representation of coordinates</param>
        /// <returns></returns>
        public static ObservableCollection<Location> MakeTrailPoints(string TrailString)
        {
            ObservableCollection<Location> result = new ObservableCollection<Location>();
            if (TrailString != string.Empty && TrailString != null)
            {
                string[] Locations = TrailString.Split('|');
                double tempLat;
                double tempLong;
                foreach (string s in Locations)
                {
                    string[] temp = s.Split(';');
                    if (temp[0] != null && temp[1] != null)
                    {
                        tempLat = double.Parse(temp[0]);
                        tempLong = double.Parse(temp[1]);
                        result.Add(new Location() { Latitude = tempLat, Longitude = tempLong });
                    }
                }
            }
            return result;
        }

        //Measure distance between two points with lat and long
        public static double Distance(ObservableCollection<Location> points)
        {
            const int r = 6371; // radius of earth in km
            double result = 0, prevLat = 0, prevLong = 0;

            foreach (Location p in points)
            {
                if (prevLat == 0 && prevLong == 0)
                {
                    prevLat = p.Latitude; prevLong = p.Longitude;
                }
                else
                {
                    double lat1 = ToRadians(prevLat),
                    lon1 = ToRadians(prevLong),
                    lat2 = ToRadians(p.Latitude),
                    lon2 = ToRadians(p.Longitude);

                    result += Math.Acos(
                        Math.Sin(lat1) * Math.Sin(lat2) +
                        Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1)
                        ) * r;
                    prevLat = p.Latitude; prevLong = p.Longitude;
                }
            }

            return result;
        }

        private static double ToRadians(double value)
        {
            double result;
            result = Math.PI * value / 180;
            //return (Math.PI / 180) * value;
            return result;
        }

    }
}