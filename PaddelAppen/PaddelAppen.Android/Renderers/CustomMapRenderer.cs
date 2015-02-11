using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using PaddelAppen.Droid.Renderers;
using PaddelAppen.Controls;
using Xamarin.Forms.Maps.Android;
using System.Collections.ObjectModel;
using PaddelAppen.Droid.Services;
using Android.Locations;



[assembly: ExportRenderer (typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PaddelAppen.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        bool _isDrawnDone;
        private Dictionary<string, int> IDMap = new Dictionary<string, int>();
        private List<Polyline> polylines = new List<Polyline>();

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            var formsMap = (CustomMap)Element;
            var androidMapView = (MapView)Control;

            if (androidMapView != null && androidMapView.Map != null)
            {
                androidMapView.Map.InfoWindowClick += MapOnInfoWindowClick;
            }

            if (formsMap != null)
            {
                ((ObservableCollection<Pin>)formsMap.Pins).CollectionChanged += OnCollectionChanged;
            }
            
            
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion") && !_isDrawnDone)
            {
                UpdatePins();

                _isDrawnDone = true;

            }
        }
        
        void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdatePins();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void UpdatePins()
        {
            var androidMapView = (MapView)Control;
            var formsMap = (CustomMap)Element;

            androidMapView.Map.Clear();

            androidMapView.Map.MarkerClick += HandleMarkerClick;
            androidMapView.Map.MyLocationEnabled = formsMap.IsShowingUser;
            androidMapView.Map.MyLocationChange += HandleLocationChanged2;


            //locationstufftests
            //androidMapView.Map.MyLocationChange += HandleLocationChanged;
            LocationProvider.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
            {
                // notifies us of location changes from the system
                LocationProvider.Current.LocationService.LocationChanged += HandleLocationChangedd;
            };


            ///

            var items = formsMap.Items;

            foreach (var item in items)
            {
                var mapMarker = new MarkerOptions();
                mapMarker.SetPosition(new LatLng(item.Location.Latitude, item.Location.Longitude));
                mapMarker.SetTitle(string.IsNullOrWhiteSpace(item.Name) ? "-" : item.Name);
                mapMarker.SetSnippet(item.Details);

                try
                {
                    mapMarker.InvokeIcon(BitmapDescriptorFactory.FromResource(GetPinIcon(item.Type)));
                }
                catch (Exception)
                {
                    mapMarker.InvokeIcon(BitmapDescriptorFactory.DefaultMarker());
                }

                //Map android marker ID with Forms marker ID while adding marker to map
                try
                {
                    IDMap.Add(androidMapView.Map.AddMarker(mapMarker).Id, item.ID);
                }
                catch (ArgumentException)
                {
                    //wat do here
                }
            }

            if (items.Count == 1)
                AddPolyLines(items.ElementAtOrDefault(0).ID);
        }

        private void HandleLocationChanged2(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            var androidMapView = (MapView)Control;
            var formsMap = (CustomMap)Element;

            //if(formsMap.FollowUser)
            //{
            formsMap.MoveToCurrentPositionFromPage();
        }

        private void HandleLocationChangedd(object sender, LocationChangedEventArgs e)
        {
            var androidMapView = (MapView)Control;
            var formsMap = (CustomMap)Element;
            
            //if(formsMap.FollowUser)
            //{
            formsMap.MoveToCurrentPositionFromPage();
            //}
        }

        /// <summary>
        /// Returns a different icon png for a pin by it's location's type
        /// </summary>
        /// <param name="type">LocationType from the MapExtensions</param>
        /// <returns>icon.png</returns>
        private static int GetPinIcon(PaddelAppen.Extensions.MapExtensions.LocationType type)
        {
            if (type == Extensions.MapExtensions.LocationType.Trail)
                return Resource.Drawable.polyline24;
            else if (type == Extensions.MapExtensions.LocationType.Rental)
                return Resource.Drawable.cardinuse24;
            else if (type == Extensions.MapExtensions.LocationType.Camp)
                return Resource.Drawable.sleepingbag24;
            else if (type == Extensions.MapExtensions.LocationType.Launch)
                return Resource.Drawable.matches24;
            else if (type == Extensions.MapExtensions.LocationType.CurrentLocation)
                return Resource.Drawable.kayak24;
            else throw new Exception();
        }

        /// <summary>
        /// Handles the click event triggered when clicking a map pin and sets an ID of the shared codes
        /// CustomPin and maps it to the google marker's ID. Also calls the polyline add method.
        /// </summary>
        private void HandleMarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {  
            var marker = e.Marker;

            marker.ShowInfoWindow();
            var map = this.Element as CustomMap;
            
            var formsPin = new CustomPin(marker.Title, marker.Snippet, marker.Position.Latitude, marker.Position.Longitude);
            
            map.SelectedPin = formsPin;
            map.SelectedPin.ID = IDMap[marker.Id];

            AddPolyLines(IDMap[marker.Id]);
        }

        /// <summary>
        /// Handles the event triggered when click the info window of a pin/marker by calling the
        /// corresponding method in the CustomMap forms-map class (which opens the details page of the
        /// specified location).
        /// </summary>
        private void MapOnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            Marker clickedMarker = e.Marker;
            // Find the matchin item
            var formsMap = (CustomMap)Element;
            //formsMap.ShowDetailCommand.Execute(formsMap.SelectedPin);
            formsMap.InfoWindowClick(IDMap[clickedMarker.Id]);
        }

        //private bool IsItem(IMapModel item, Marker marker)
        //{
        //    return item.Name == marker.Title &&
        //           item.Details == marker.Snippet &&
        //           item.Location.Latitude == marker.Position.Latitude &&
        //           item.Location.Longitude == marker.Position.Longitude;
        //}

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            //NOTIFY CHANGE

            if (changed)
            {
                _isDrawnDone = false;
            }
        }

        
        /// <summary>
        /// Adds polylines to the map by getting the coordinates from the CustomMap objects point
        /// specified as a param and then calling that points GetTrailCollection (contains collection
        /// of coordinates) then adds them to a PolylineOptions which is then added to a Polyline object
        /// which in turn is added to a list of polylines. (The list is needed to remove the polylines
        /// from the map so that when you click a new location the old locations polylines will not remain
        /// on the map. Notice the list is cleared in the first segment of this method.)
        /// </summary>
        /// <param name="ID"></param>
        private void AddPolyLines(int ID)
        {
            if (polylines.Count != 0)
            {
                polylines.Last().Remove();
                polylines.Clear();
            }
            var formsMap = (CustomMap)Element;

            PolylineOptions polyLineOptions = new PolylineOptions();

            try
            {
                foreach (var p in formsMap.GetPointByID(ID).GetTrailCollection())
                    polyLineOptions.Add(new LatLng(p.Latitude, p.Longitude));
                var androidMapView = (MapView)Control;

                polylines.Add(androidMapView.Map.AddPolyline(polyLineOptions));

                polylines.Last().Color = Resources.GetColor(Android.Resource.Color.HoloBlueDark);
                polylines.Last().Width = 5;
            }
            catch (Exception)
            {

            }
        }

        
    }
}