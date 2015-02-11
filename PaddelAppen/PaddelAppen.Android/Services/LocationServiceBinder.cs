using System;

using Android.OS;

namespace PaddelAppen.Droid.Services
{
    /*http://developer.xamarin.com/guides/android/application_fundamentals/backgrounding/part_3_android_backgrounding_walkthrough/
    */

    //This is our Binder subclass, the LocationServiceBinder
    public class LocationServiceBinder : Binder
    {
        public LocationService Service
        {
            get { return this.service; }
        } protected LocationService service;

        public bool IsBound { get; set; }

        // constructor
        public LocationServiceBinder(LocationService service)
        {
            this.service = service;
        }
    }
}