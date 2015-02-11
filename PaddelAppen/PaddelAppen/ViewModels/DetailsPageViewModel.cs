using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PaddelAppen.Controls;
using PaddelAppen.Views;

namespace PaddelAppen.ViewModels
{
    class DetailsPageViewModel
    {
        private PointOfInterest point;
        INavigation nav;

        public DetailsPageViewModel(PointOfInterest point, DetailsPage page, StackLayout MapStack)
        {
            this.point = point;
            page.BackgroundColor = Color.White;
            nav = page.Navigation;
            MapStack.Children.Add(AddMap());
        }

        private CustomMap AddMap()
        {
            var mapSpan = MapSpan.FromCenterAndRadius(new Position(point.Lat, point.Long), Distance.FromKilometers(5));
            CustomMap TrailMap = new CustomMap(mapSpan);
            TrailMap.HeightRequest = 200;
            TrailMap.WidthRequest = 320;
            TrailMap.AddPoint(point);
            var position = new Position(point.Lat, point.Long);
            return TrailMap;
        }

        public string PointName
        {
            get { return point.Name; }
        }

        public string PointScore
        {
            get { return "Poäng: " + point.Score.ToString(); }
        }

        public string PointType
        {
            get { return point.Type.ToString(); }
        }

        public string PointDescription
        {
            get { return point.Notes; }
        }

        public string PointID
        {
            get { return point.ID.ToString(); }
        }
        
        public string PointDistance
        {
            get { if (point.Type == Extensions.MapExtensions.LocationType.Trail) 
            {
                point.SetDistance();
                return "Längd: " + point.RoundedDistanceKilometers.ToString() + "km";
            }
            else return "";
            }
        }
        
        public string PointComments
        {
            get { return "Not implemented"; }
        }

        private Command liveCommand;
        public Command LiveCommand
        {
            get
            {
                return liveCommand ??
                    (liveCommand = new Command(async () => await OpenLivePage()));
            }
        }

        protected async Task OpenLivePage()
        {
            var liveP = new LivePage(point);
            await nav.PushAsync(liveP);
        }

        /*var tbi = new ToolbarItem("x", null, () =>
        {
            var listItem = point;
            var listItemPage = new ListItemXaml();
            listItemPage.BindingContext = listItem;
            Navigation.PushAsync(listItemPage);
        }, 0, 0);
        if (Device.OS == TargetPlatform.Android)
        { // BUG: Android doesn't support the icon being null
            tbi = new ToolbarItem("x", "edit", () =>
            {
                var listItem = point;
                var listItemPage = new ListItemXaml();
                listItemPage.BindingContext = listItem;
                Navigation.PushAsync(listItemPage);
            }, 0, 0);
        }

        ToolbarItems.Add(tbi);
        */
    }
}
