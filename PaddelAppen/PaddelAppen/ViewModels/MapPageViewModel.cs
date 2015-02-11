using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PaddelAppen.Controls;
using PaddelAppen.Extensions;

namespace PaddelAppen.ViewModels
{
    class MapPageViewModel
    {
        private CustomMap TrailMap;

        public MapPageViewModel(MapPage page, StackLayout mapStack)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(App.CurrentLocation.Latitude, App.CurrentLocation.Longitude), Distance.FromKilometers(5));
            TrailMap = new CustomMap(mapSpan);

            //TrailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(currentPosition.Latitude, currentPosition.Longitude), Distance.FromKilometers(5)));
            //TrailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(59.378974, 13.505321), Distance.FromKilometers(5)));

            var FilterAllTBI = new ToolbarItem("+", "sleepingbag24", () =>
            {
                TrailMap.AddPoints();
            }, 0, 0);

            var FilterRentalsTBI = new ToolbarItem("+", "cardinuse24", () =>
            {
                TrailMap.AddPoints(MapExtensions.LocationType.Rental);
            }, 0, 0);

            var FilterTrailsTBI = new ToolbarItem("+", "polyline24", () =>
            {
                TrailMap.AddPoints(MapExtensions.LocationType.Trail);
            }, 0, 0);

            var FilterCampTBI = new ToolbarItem("+", "matches24", () =>
            {
                TrailMap.AddPoints(MapExtensions.LocationType.Trail);
            }, 0, 0);

            page.ToolbarItems.Add(FilterAllTBI);
            page.ToolbarItems.Add(FilterRentalsTBI);
            page.ToolbarItems.Add(FilterTrailsTBI);
            page.ToolbarItems.Add(FilterCampTBI);

            TrailMap.AddPoints();

            mapStack.Children.Add(TrailMap);
        }
    }
}
