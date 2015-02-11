using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Windows.Input;
using PaddelAppen.Models;
using PaddelAppen.Extensions;
using PaddelAppen.ViewModels;



namespace PaddelAppen.Controls
{
    public class CustomMap : Map
    {
        /// <summary>
        /// This class along with CustomPin and the custom renderer are modified versions of the classes
        /// found here: https://github.com/raechten/BindableMapTest/blob/master/BindableMapTest/
        /// They are made to extend the functionality of the Xamarin.Froms.Maps control to introduce new
        /// features  (that can be found in googles Maps but not Forms.Maps) needed for this project.
        /// Those features includ the ability to draw lines on the map, change the looks of map pins, change
        /// the looks of info windows, the ability to click on the map and the info windows and the pins.
        /// </summary>

        private readonly ObservableCollection<CustomPin> _items = new ObservableCollection<CustomPin>();
        public ObservableCollection<Location> TrailPoints = new ObservableCollection<Location>();
        public bool FollowUser { get { return _followUser; } set { _followUser = value; } }
        private bool _followUser = false;
        private LivePageViewModel _page;
        
        public CustomMap(MapSpan region) : base(region)
        {
            MoveToRegion(region);
        }

        public CustomMap(MapSpan region, LivePageViewModel page) : base(region)
        {
            MoveToRegion(region);
            FollowUser = true;
            _page = page;
        }

        public void MoveToCurrentPositionFromPage()
        {
            //if (FollowUser && _page != null)
                _page.MoveMapToCurrentPosition();
        }

        public void MoveToCurrentPosition()
        {
            try
            {
                MapSpan currentPosition = MapSpan.FromCenterAndRadius(
                    new Xamarin.Forms.Maps.Position(App.CurrentLocation.Latitude, App.CurrentLocation.Longitude), Distance.FromKilometers(5));
                MoveToRegion(currentPosition);
            }
            catch (NullReferenceException e)
            {
                return;
            }
            //coords = Status + " " + Latitude.ToString() + " " + Longitude.ToString();
            //double doubleLat = double.Parse(Latitude);
            //double doubleLong = double.Parse(Longitude);
            //if (doubleLat < 0)
            //    doubleLat = doubleLat * -1;
            //if (doubleLong < 0)
            //    doubleLong = doubleLong * -1;
            //MapSpan newPosition = MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(doubleLat, doubleLong), Distance.FromKilometers(5));
        }

        public void MoveToCurrentPosition(double radius)
        {
            try
            {
                MapSpan currentPosition = MapSpan.FromCenterAndRadius(
                    new Xamarin.Forms.Maps.Position(App.CurrentLocation.Latitude, App.CurrentLocation.Longitude), Distance.FromKilometers(radius));
                MoveToRegion(currentPosition);
            }
            catch (NullReferenceException e)
            {
                return;
            }
        }

        public static readonly BindableProperty SelectedPinProperty = BindableProperty.Create<CustomMap, CustomPin>(x => x.SelectedPin, null);

        public CustomPin SelectedPin
        {
            get { return (CustomPin)base.GetValue(SelectedPinProperty); }
            set { base.SetValue(SelectedPinProperty, value); }
        }

        public ICommand ShowDetailCommand { get; set; }

        public async void InfoWindowClick(int ID)
        {
            PointOfInterest item = App.Database.GetPoI(ID);
            //var listItemPage = new ListItemXaml();
            var detailsPage = new DetailsPage(item);
            //listItemPage.BindingContext = listItem;
            await Navigation.PushAsync(detailsPage);
        }

        public ObservableCollection<CustomPin> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Adds PointOfInterest objects from the database to the Items list and then adds that list
        /// as Pin objevts (through use of the AsPin-method) to the Pins collection belonging to the map.
        /// The reason for having a collection of CustomPins (Items) and a collection of Pins is that information
        /// is lost when making Pins out of CustomPins and we want to keep that information in the Items list.
        /// </summary>
        public void AddPoints()
        {
            Pins.Clear();
            Items.Clear();
            foreach (PointOfInterest p in App.Database.GetPoIs())
            {
                //var position = new Location { Latitude = p.Lat, Longitude = p.Long };
                Items.Add(new CustomPin(p));
            }
            Items.Add(new CustomPin("Här är du!", "", App.CurrentLocation.Latitude, App.CurrentLocation.Longitude) { Type = MapExtensions.LocationType.CurrentLocation });
            foreach (var item in Items)
                Pins.Add(item.AsPin());
        }

        public void AddPoints(MapExtensions.LocationType type)
        {
            Pins.Clear();
            Items.Clear();
            foreach (PointOfInterest p in App.Database.GetPoIsByType(type))
            {
                //var position = new Location { Latitude = p.Lat, Longitude = p.Long };
                //if (p.Type == type)
                //{
                    Items.Add(new CustomPin(p));
                //}
            }
            Items.Add(new CustomPin("Här är du!", "", App.CurrentLocation.Latitude, App.CurrentLocation.Longitude) { Type = MapExtensions.LocationType.CurrentLocation });
            foreach (var item in Items)
                Pins.Add(item.AsPin());
        }

        public void AddPoints(double latitude, double longitude)
        {
            Pins.Clear();
            Items.Clear();
            foreach (PointOfInterest p in App.Database.GetPoIByLocation(latitude, longitude))
            {
                Items.Add(new CustomPin(p));
            }
            Items.Add(new CustomPin("Här är du!", "", App.CurrentLocation.Latitude, App.CurrentLocation.Longitude) { Type = MapExtensions.LocationType.CurrentLocation });
            foreach (var item in Items)
            {
                Pins.Add(item.AsPin());
            }
        }

        /// <summary>
        /// Method to add just one PointOfInterest to the Items and Pins Collections. Useful for when
        /// you want to show only one pin on the map instead of all of them.
        /// </summary>
        /// <param name="point">PointOfInterest object to add</param>
        public void AddPoint(PointOfInterest point)
        {
            Pins.Clear();
            Items.Clear();
            Items.Add(new CustomPin(point));
            Pins.Add(Items.First().AsPin());
        }

        public void AddPoint(CustomPin pin)
        {
            Pins.Clear();
            Items.Clear();
            Items.Add(pin);
            Pins.Add(Items.First().AsPin());
        }

        public PointOfInterest GetPointByID(int ID)
        {
            return App.Database.GetPoI(ID);
        }
    }
}
