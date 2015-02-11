using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PaddelAppen.Controls;
using PaddelAppen.Views;
using System.ComponentModel;
using System.Collections.Generic;
using PaddelAppen.Models;
using System.Collections.ObjectModel;

namespace PaddelAppen.ViewModels
{
    public class LivePageViewModel : INotifyPropertyChanged
    {
        private PointOfInterest point;
        CustomMap TrailMap;

        public LivePageViewModel(PointOfInterest point, LivePage page, StackLayout MapStack)
        {
            this.point = point;
            page.BackgroundColor = Color.White;
            MapStack.Children.Add(AddMap());
            //App.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(location_PropertyChanged);
            points = new ObservableCollection<Location>();
            DistanceTotal = point.RoundedDistanceKilometers;
        }

        private CustomMap AddMap()
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(point.Lat, point.Long), Distance.FromKilometers(0.5));
            TrailMap = new CustomMap(mapSpan, this);
            TrailMap.FollowUser = true;
            TrailMap.AddPoint(point);
            var position = new Position(point.Lat, point.Long);
            return TrailMap;
        }

        private ObservableCollection<Location> points;
        double tempLat, tempLong;
        void HandleGlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            point.Type = Extensions.MapExtensions.LocationType.CurrentLocation;
            point.Lat = App.CurrentLocation.Latitude;
            point.Long = App.CurrentLocation.Longitude;
            TrailMap.AddPoint(point);
            Speed = App.CurrentSpeed;
            Bearing = App.CurrentBearing;
            MoveMapToCurrentPosition();
            if(tempLat != 0 && tempLong != 0)
            {
                double minLat = tempLat - 0.0005;
                double maxLat = tempLat + 0.0005;
                double minLong = tempLong - 0.0005;
                double maxLong = tempLong + 0.0005;
                if(!IsBetween(App.CurrentLocation.Latitude, minLat, maxLat) ||
                    !IsBetween(App.CurrentLocation.Longitude, minLong, maxLong))
                {
                    points.Add(new Location() { Latitude = App.CurrentLocation.Latitude, Longitude = App.CurrentLocation.Longitude });
                    if(points.Count>1)
                    {
                        DistanceTravelled = Extensions.MapExtensions.Distance(points);
                        DistanceRemaining = distanceTotal - distanceTravelled;
                    }
                }

            }
            tempLat = App.CurrentLocation.Latitude;
            tempLong = App.CurrentLocation.Longitude;
            
        }

        void location_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MoveMapToCurrentPosition();
        }

        public void MoveMapToCurrentPosition()
        {
            Speed = App.CurrentSpeed;
            Bearing = App.CurrentBearing;
            TrailMap.MoveToCurrentPosition(0.5);
        }

        protected Command startMapTrackingCommand;
        public Command StartMapTrackingCommand
        {
            get
            {
                return startMapTrackingCommand ??
                    (startMapTrackingCommand = new Command(async () => await StartMapTracking()));
            }
        }

        private bool started;
        protected async Task StartMapTracking()
        {
            double minLat = App.CurrentLocation.Latitude - 0.0005;
            double maxLat = App.CurrentLocation.Latitude + 0.0005;
            double minLong = App.CurrentLocation.Longitude - 0.0005;
            double maxLong = App.CurrentLocation.Longitude + 0.0005;
                if (!started)
                {
                    if (IsBetween(point.Lat, minLat, maxLat) && IsBetween(point.Long, minLong, maxLong))
                    {
                    App.GlobalPropertyChanged -= this.HandleGlobalPropertyChanged;
                    App.GlobalPropertyChanged += this.HandleGlobalPropertyChanged;
                    started = true;
                    ButtonText = "Started - Pause";
                    }
                }
                else
                {
                    App.GlobalPropertyChanged -= this.HandleGlobalPropertyChanged;
                    started = false;
                    ButtonText = "Paused - Start";
                }
        }

        private bool IsBetween(double num, double lower, double upper)
        {
            bool inclusive = false;
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private float speed;
        public float Speed
        {
            get { return speed; }
            set {
                if (speed != value)
                {
                    speed = value;
                    OnPropertyChanged("Speed");
                }
            }
        }

        private string buttonText = "Start";
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                if (buttonText != value)
                {
                    buttonText = value;
                    OnPropertyChanged("ButtonText");
                }
            }
        }

        private float bearing;
        public float Bearing
        {
            get { return bearing; }
            set
            {
                if (bearing != value)
                {
                    bearing = value;
                    OnPropertyChanged("Speed");
                }
            }
        }

        private double distanceTotal;
        public double DistanceTotal
        {
            get { return distanceTotal; }
            set
            {
                if (distanceTotal != value)
                {
                    distanceTotal = value;
                    OnPropertyChanged("DistanceTotal");
                }
            }
        }

        private double distanceTravelled;
        public double DistanceTravelled
        {
            get { return System.Math.Round(distanceTravelled * 1000); }
            set
            {
                if (distanceTravelled != value)
                {
                    distanceTravelled = value;
                    OnPropertyChanged("DistanceTravelled");
                }
            }
        }

        private double distanceRemaining;
        public double DistanceRemaining
        {
            get { return System.Math.Round(distanceRemaining * 1000); }
            set
            {
                if (distanceRemaining != value)
                {
                    distanceRemaining = value;
                    OnPropertyChanged("DistanceRemaining");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}