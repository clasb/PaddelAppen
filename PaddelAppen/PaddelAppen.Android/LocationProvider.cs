using System;
using System.Threading;
using System.Threading.Tasks;

using Android.Content;
using Android.Util;

using PaddelAppen.Droid.Services;

namespace PaddelAppen.Droid
{
    public class LocationProvider
    {
        public event EventHandler<ServiceConnectedEventArgs> LocationServiceConnected = delegate {};

        protected LocationServiceConnection locationServiceConnection;

        public static LocationProvider Current
        {
            get { return current; }
        } private static LocationProvider current;

        public LocationService LocationService
        {
            get
            {
                if (this.locationServiceConnection.Binder == null)
                    throw new Exception("Service not bound");
                return this.locationServiceConnection.Binder.Service;
            }
        }

        static LocationProvider()
        {
            current = new LocationProvider();
        }

        protected LocationProvider()
        {
            new Task(() =>
            {
                Android.App.Application.Context.StartService(new Intent(Android.App.Application.Context, typeof(LocationService)));
                this.locationServiceConnection = new LocationServiceConnection(null);
                this.locationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
                {
                    this.LocationServiceConnected(this, e);
                };
                Intent locationServiceIntent = new Intent(Android.App.Application.Context, typeof(LocationService));
                Android.App.Application.Context.BindService(locationServiceIntent, locationServiceConnection, Bind.AutoCreate);
            }).Start();
        }
    }
}