using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLiteNetExtensions;
using PaddelAppen.Models;
using System.Collections.ObjectModel;
using PaddelAppen.Extensions;

namespace PaddelAppen
{
    public class PointDatabase
    {
        static object locker = new object();
        SQLiteConnection database;
        
        /// <summary>
        /// Creates the database from the connection in the App class and then creates a table
        /// that can contain PointOfInterest objects.
        /// </summary>
        public PointDatabase(SQLiteConnection conn)
        {
            database = conn;
            database.CreateTable<PointOfInterest>();
        }

        #region PoI database methods
        /// <summary>
        /// Gets all the PointsOfInterest objects in the table of such objects from the databse.
        /// Returns them as a Collection of such objects.
        /// </summary>
        /// <returns>IEnumerable Collection of PointOfInterest objects</returns>
        public IEnumerable<PointOfInterest> GetPoIs()
        {
            lock (locker)
            {
                return (from i in database.Table<PointOfInterest>() select i).ToList();
            }
        }

        public IEnumerable<PointOfInterest> GetPoIsByType(MapExtensions.LocationType type)
        {
            lock (locker)
            {
                var typeList = database.Query<PointOfInterest>("SELECT * FROM PointOfInterest WHERE Type = ?", type);
                return typeList;
            }
        }

        public IEnumerable<PointOfInterest> GetPoIByLocation(double latitude, double longitude)
        {
            double minLat = latitude - 0.05,
                maxLat = latitude + 0.05,
                minLong = longitude - 0.05,
                maxLong = longitude + 0.05;
            
            //"SELECT * FROM PointOfInterest WHERE (Lat BETWEEN ? AND ?) AND (Long BETWEEN ? AND ?)";
            
            lock (locker)
            {
                var locList = database.Query<PointOfInterest>("SELECT * FROM PointOfInterest WHERE (Lat BETWEEN ? AND ?) AND (Long BETWEEN ? AND ?)", new String[] {minLat.ToString(), maxLat.ToString(), minLong.ToString(), maxLong.ToString()});
                return locList;
            }
        }

        /// <summary>
        /// Gets a specific PointOfInterest object from the database by it's ID (primary key).
        /// </summary>
        /// <param name="id">int representing ID of wanted object</param>
        /// <returns>PointOfInterest object</returns>
        public PointOfInterest GetPoI(int id)
        {
            lock (locker)
            {
                return database.Table<PointOfInterest>().FirstOrDefault(point => point.ID == id);
            }
        }

        /// <summary>
        /// Saves an object of type PointOfInterest to the databse by either updating existing
        /// item or inserting new.
        /// </summary>
        /// <param name="item">PointOfInterest object to save</param>
        /// <returns></returns>
        public int SavePoI(PointOfInterest item)
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    database.Update(item);
                    return item.ID;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        /// <summary>
        /// Removes a PointOfInterest object from the database table by it's ID (primary key).
        /// </summary>
        /// <param name="id">int ID of object to be removed</param>
        public int DeletePoI(int id)
        {
            lock (locker)
            {
                return database.Delete<PointOfInterest>(id);
            }
        }

        /// <summary>
        /// Removes all items in PointOfInterest table (???).
        /// </summary>
        public void DeleteAll()
        {
            lock(locker)
            {
                database.DeleteAll<PointOfInterest>();
            }
        }
        #endregion
    }
}
