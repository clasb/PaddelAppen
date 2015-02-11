using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

using PaddelAppen.Extensions;
using PaddelAppen.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PaddelAppen.ViewModels;

namespace PaddelAppen
{
    public class App : INotifyPropertyChanged
    {
        static NavigationPage rootPage;

        public static NavigationPage RootPage
        { get { return rootPage; } }
            
        public static Page GetMainPage()
        {
            rootPage = new NavigationPage(new BrowsePage());
            rootPage.BarBackgroundColor = Color.White;
            rootPage.BarTextColor = Color.Black;
            rootPage.BackgroundImage = "background.png";
            return rootPage;
            //return new NavigationPage(new BrowsePage());
        }

        static SQLite.Net.SQLiteConnection conn;
        static PointDatabase database;
        public static void SetDatabaseConnection(SQLite.Net.SQLiteConnection connection)
        {
            conn = connection;
            database = new PointDatabase(conn);
            
            PopulateDatabaseWithTests();
        }

        public static PointDatabase Database
        {
            get { return database; }
        }



        private static Location defaultLocation = new Location() { Latitude = 62.3875, Longitude = 16.325556 };
        public static Location CurrentLocation
        { get { if (_location == null) return defaultLocation; return _location; } }
        protected static Location _location;

        public static float CurrentSpeed
        { get { return _speed; } }
        protected static float _speed;

        public static float CurrentBearing
        { get { return _bearing; } }
        protected static float _bearing;


        public static void SetLocation(double latitude, double longitude, float speed, float bearing)
        {
            _location = new Location() { Latitude = latitude, Longitude = longitude };
            _speed = speed;
            _bearing = bearing;
            OnGlobalPropertyChanged("CurrentLocation");
        }

        /*protected static void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(null, e);
        }

        protected static void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }*/

        public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };
        public static void OnGlobalPropertyChanged(string propertyName)
        {
            GlobalPropertyChanged(
                typeof(LivePageViewModel),
                new PropertyChangedEventArgs(propertyName));
        }
        /*----------------------------------------------*/

        public event PropertyChangedEventHandler PropertyChanged;

        private static void PopulateDatabaseWithTests()
        {
            database.DeleteAll();
            ObservableCollection<Location> tempList = new ObservableCollection<Location>();
            tempList.Add(new Location() { Latitude = 59.522180, Longitude = 13.450431 });
            tempList.Add(new Location() { Latitude = 59.509399, Longitude = 13.440353 });
            tempList.Add(new Location() { Latitude = 59.492323, Longitude = 13.429881 });
            tempList.Add(new Location() { Latitude = 59.478377, Longitude = 13.413573 });
            tempList.Add(new Location() { Latitude = 59.471498, Longitude = 13.417761 });
            tempList.Add(new Location() { Latitude = 59.466441, Longitude = 13.428576 });
            tempList.Add(new Location() { Latitude = 59.452571, Longitude = 13.440249 });
            tempList.Add(new Location() { Latitude = 59.448669, Longitude = 13.447214 });
            tempList.Add(new Location() { Latitude = 59.438022, Longitude = 13.447729 });
            tempList.Add(new Location() { Latitude = 59.429467, Longitude = 13.450991 });
            tempList.Add(new Location() { Latitude = 59.422394, Longitude = 13.480173 });
            tempList.Add(new Location() { Latitude = 59.404663, Longitude = 13.499228 });
            tempList.Add(new Location() { Latitude = 59.395656, Longitude = 13.505950 });
            tempList.Add(new Location() { Latitude = 59.388401, Longitude = 13.498741 });
            string trailString = MapExtensions.MakeTrailString(tempList);
            database.SavePoI(new PointOfInterest()
            {
                Name = "Forshaga - Karlstad",
                Notes = "En led i Klarälven från Forshaga i norr till Karlstad i söder",
                Score = 3.4,
                Type = MapExtensions.LocationType.Trail,
                Trail = trailString,
                Lat = 59.522180,
                Long = 13.450431

            });

            tempList.Clear();
            tempList.Add(new Location() { Latitude = 59.384535, Longitude = 13.499044 });
            tempList.Add(new Location() { Latitude = 59.381956, Longitude = 13.499817 });
            tempList.Add(new Location() { Latitude = 59.380688, Longitude = 13.496641 });
            tempList.Add(new Location() { Latitude = 59.380950, Longitude = 13.490547 });
            tempList.Add(new Location() { Latitude = 59.379814, Longitude = 13.483337 });
            tempList.Add(new Location() { Latitude = 59.378896, Longitude = 13.478187 });
            tempList.Add(new Location() { Latitude = 59.376447, Longitude = 13.473724 });
            tempList.Add(new Location() { Latitude = 59.372512, Longitude = 13.470205 });
            tempList.Add(new Location() { Latitude = 59.367833, Longitude = 13.461622 });
            tempList.Add(new Location() { Latitude = 59.366521, Longitude = 13.460335 });
            tempList.Add(new Location() { Latitude = 59.362191, Longitude = 13.460506 });
            tempList.Add(new Location() { Latitude = 59.357948, Longitude = 13.461450 });
            tempList.Add(new Location() { Latitude = 59.355236, Longitude = 13.460077 });
            tempList.Add(new Location() { Latitude = 59.350467, Longitude = 13.468231 });
            tempList.Add(new Location() { Latitude = 59.350773, Longitude = 13.474239 });
            tempList.Add(new Location() { Latitude = 59.352523, Longitude = 13.478359 });
            tempList.Add(new Location() { Latitude = 59.355804, Longitude = 13.481706 });
            tempList.Add(new Location() { Latitude = 59.358123, Longitude = 13.485140 });
            tempList.Add(new Location() { Latitude = 59.360179, Longitude = 13.489088 });
            tempList.Add(new Location() { Latitude = 59.364466, Longitude = 13.494152 });
            tempList.Add(new Location() { Latitude = 59.365515, Longitude = 13.503679 });
            tempList.Add(new Location() { Latitude = 59.367833, Longitude = 13.508056 });
            tempList.Add(new Location() { Latitude = 59.364378, Longitude = 13.519729 });
            tempList.Add(new Location() { Latitude = 59.365865, Longitude = 13.522304 });
            tempList.Add(new Location() { Latitude = 59.369189, Longitude = 13.516296 });
            tempList.Add(new Location() { Latitude = 59.370807, Longitude = 13.513120 });
            tempList.Add(new Location() { Latitude = 59.373868, Longitude = 13.511833 });
            tempList.Add(new Location() { Latitude = 59.375223, Longitude = 13.511318 });
            tempList.Add(new Location() { Latitude = 59.377934, Longitude = 13.511919 });
            trailString = MapExtensions.MakeTrailString(tempList);
            database.SavePoI(new PointOfInterest()
            {
                Name = "Sandgrund - Vänerkajak",
                Notes = "En led i Klarälven från Sandgrundsudden till Vänerkajak i inre hamn.",
                Score = 3.9,
                Type = MapExtensions.LocationType.Trail,
                Trail = trailString,
                Lat = 59.384535,
                Long = 13.499044

            });

            tempList.Clear();
            tempList.Add(new Location() { Latitude = 59.433521, Longitude = 13.595228 });
            tempList.Add(new Location() { Latitude = 59.432970, Longitude = 13.597753 });
            tempList.Add(new Location() { Latitude = 59.432696, Longitude = 13.598744 });
            tempList.Add(new Location() { Latitude = 59.433070, Longitude = 13.599932 });
            tempList.Add(new Location() { Latitude = 59.431850, Longitude = 13.599583 });
            tempList.Add(new Location() { Latitude = 59.431599, Longitude = 13.600943 });
            tempList.Add(new Location() { Latitude = 59.429615, Longitude = 13.600748 });
            tempList.Add(new Location() { Latitude = 59.427093, Longitude = 13.599392 });
            tempList.Add(new Location() { Latitude = 59.424362, Longitude = 13.597385 });
            tempList.Add(new Location() { Latitude = 59.423660, Longitude = 13.597772 });
            tempList.Add(new Location() { Latitude = 59.421895, Longitude = 13.596458 });
            tempList.Add(new Location() { Latitude = 59.420865, Longitude = 13.596695 });
            tempList.Add(new Location() { Latitude = 59.420253, Longitude = 13.595793 });
            tempList.Add(new Location() { Latitude = 59.418646, Longitude = 13.595366 });
            tempList.Add(new Location() { Latitude = 59.416732, Longitude = 13.598013 });
            tempList.Add(new Location() { Latitude = 59.416666, Longitude = 13.600165 });
            tempList.Add(new Location() { Latitude = 59.416459, Longitude = 13.601477 });
            tempList.Add(new Location() { Latitude = 59.417060, Longitude = 13.605759 });
            tempList.Add(new Location() { Latitude = 59.412281, Longitude = 13.607734 });
            tempList.Add(new Location() { Latitude = 59.410935, Longitude = 13.607605 });
            tempList.Add(new Location() { Latitude = 59.409116, Longitude = 13.608598 });
            tempList.Add(new Location() { Latitude = 59.408552, Longitude = 13.606792 });
            tempList.Add(new Location() { Latitude = 59.401031, Longitude = 13.606920 });
            tempList.Add(new Location() { Latitude = 59.399176, Longitude = 13.605928 });
            tempList.Add(new Location() { Latitude = 59.398771, Longitude = 13.605371 });
            tempList.Add(new Location() { Latitude = 59.398804, Longitude = 13.604203 });
            tempList.Add(new Location() { Latitude = 59.398048, Longitude = 13.603944 });
            tempList.Add(new Location() { Latitude = 59.397730, Longitude = 13.604616 });
            tempList.Add(new Location() { Latitude = 59.396294, Longitude = 13.604086 });
            tempList.Add(new Location() { Latitude = 59.394845, Longitude = 13.603765 });
            tempList.Add(new Location() { Latitude = 59.394362, Longitude = 13.602987 });
            tempList.Add(new Location() { Latitude = 59.393494, Longitude = 13.604391 });
            tempList.Add(new Location() { Latitude = 59.392412, Longitude = 13.607050 });
            tempList.Add(new Location() { Latitude = 59.392213, Longitude = 13.611765 });
            tempList.Add(new Location() { Latitude = 59.393159, Longitude = 13.611718 });
            tempList.Add(new Location() { Latitude = 59.394237, Longitude = 13.612127 });
            trailString = MapExtensions.MakeTrailString(tempList);
            database.SavePoI(new PointOfInterest()
            {
                Name = "Alstern - Vänern",
                Notes = "En led i Alstersån, från Alstern till Vänerg genom tät skog och grunda vatten.",
                Score = 4.1,
                Type = MapExtensions.LocationType.Trail,
                Trail = trailString,
                Lat = 59.433521,
                Long = 13.595228

            });

            database.SavePoI(new PointOfInterest()
            {
                Name = "Uthyrning: Vänerkajak",
                Notes = "Kayak- och kanouthyrning i inre hamn med guidade turer, kurser och butik.",
                Score = 4,
                Type = MapExtensions.LocationType.Rental,
                Lat = 59.377960,
                Long = 13.511419

            });
        }
    }
}
