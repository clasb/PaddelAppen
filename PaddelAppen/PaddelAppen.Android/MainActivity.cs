using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.IO;

using Xamarin.Forms.Platform.Android;

using PaddelAppen.Droid.Services;
using Android.Locations;

namespace PaddelAppen.Droid
{
    [Activity(Label = "PaddelAppen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            //We need to initalize the xamarin forms and xamarin forms maps
            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);

            //And we need to initialize the database and put it in the personal folder
            var sqliteFilename = "PaddelAppenDatabase.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            // This is where we copy in the prepopulated database if there is one
            /*Console.WriteLine(path);
            if (!File.Exists(path))
            {
                var s = Resources.OpenRawResource(PaddelAppen.Resource.Raw.TodoSQLite);  // RESOURCE NAME ###

                // create a write stream
                FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                // write to the stream
                ReadWriteStream(s, writeStream);
            }*/

            //Specify it is an android database and then set the connection
            var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var conn = new SQLite.Net.SQLiteConnection(plat, path);

            // Set the database connection string
            App.SetDatabaseConnection(conn);

            /*
             * BELOW IS LOCATION STUFF
             */
            
            LocationProvider.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
            {
                // notifies us of location changes from the system
                LocationProvider.Current.LocationService.LocationChanged += HandleLocationChanged;
                //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                LocationProvider.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                LocationProvider.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                // notifies us of the changing status of a provider (ie GPS no longer available)
                LocationProvider.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            //This is for xamarin.forms page
            SetPage(App.GetMainPage());
        }

        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Android.Locations.Location location = e.Location;

            // these events are on a background thread, need to update on the UI thread
            RunOnUiThread(() =>
            {
                //Can also get Altitude, Speed, Accuracy, Bearing!
                App.SetLocation(location.Latitude, location.Longitude, location.Speed, location.Bearing);
            });
        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
        }

    }
}

