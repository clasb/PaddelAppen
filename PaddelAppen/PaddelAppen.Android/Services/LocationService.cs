using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Locations;

namespace PaddelAppen.Droid.Services
{
    /*http://developer.xamarin.com/guides/android/application_fundamentals/backgrounding/part_3_android_backgrounding_walkthrough/
    */
        /// <summary>
        /// Dekorera med [Service] s� att Servicen registreras i AndroidManifest.
        /// </summary>
        [Service]
        public class LocationService : Service, ILocationListener
        {
            //Ny platsdata finns
            public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
            //GPS sl�s av
            public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
            //GPS sl�s p�
            public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };

            public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

            public LocationService()
            {
            }

            /// <summary>
            /// Location manager beh�vs f�r att f� kontakt med systemets location service.
            /// Nuvarande Context anv�nds f�r att initialisera Managern med systemets location service.
            /// </summary>
            protected LocationManager locationManager = Application.Context.GetSystemService("location") as LocationManager;
            IBinder binder;

            public override void OnCreate()
            {
                base.OnCreate();
            }

            public override IBinder OnBind(Intent intent)
            {
                binder = new LocationServiceBinder(this);
                return binder;
            }

            /// <summary>
            /// Override StartCommandResult f�r att g�ra den "sticky", vilket g�r att servicen startas om OSet st�nger den pga minne.
            /// </summary>
            public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
            {
                return StartCommandResult.Sticky;
            }

            public void StartLocationUpdates()
            {
                var locationCriteria = new Criteria();
                locationCriteria.Accuracy = Accuracy.NoRequirement;
                locationCriteria.PowerRequirement = Power.NoRequirement;
                var locationProvider = locationManager.GetBestProvider(locationCriteria, true);
                locationManager.RequestLocationUpdates(locationProvider, 2000, 0, this);
            }


            /// <summary>
            /// Servicen implementerar ILocationListener och prenumererar d�rf�r redan p� dessa metoder
            /// men f�r att appen som anv�nder servicen ska veta n�r dessa anropas beh�vs de g�ras tillg�ngliga.
            /// </summary>
            public void OnLocationChanged(Android.Locations.Location location)
            {
                this.LocationChanged(this, new LocationChangedEventArgs(location));
            }

            public void OnProviderDisabled(string provider)
            {
                this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
            }

            public void OnProviderEnabled(string provider)
            {
                this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
            }

            public void OnStatusChanged(string provider, Availability status, Bundle extras)
            {
                this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
            }
        }
    
}