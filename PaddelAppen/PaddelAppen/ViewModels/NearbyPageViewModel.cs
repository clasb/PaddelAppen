using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PaddelAppen.Controls;

namespace PaddelAppen.ViewModels
{
    class NearbyPageViewModel
    {
        CustomMap TrailMap;

        public NearbyPageViewModel(NearbyPage nearbyPage, StackLayout mapStack)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(App.CurrentLocation.Latitude, App.CurrentLocation.Longitude), Distance.FromKilometers(3));
            TrailMap = new CustomMap(mapSpan);
            TrailMap.WidthRequest = 320;
            TrailMap.HeightRequest = 200;
            
            TrailMap.AddPoints(App.CurrentLocation.Latitude, App.CurrentLocation.Longitude);
            mapStack.WidthRequest = 320;
            mapStack.HeightRequest = 200;
            mapStack.Children.Add(TrailMap);
        }
    }
}
