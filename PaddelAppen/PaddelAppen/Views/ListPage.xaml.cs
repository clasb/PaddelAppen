using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PaddelAppen.ViewModels;
using PaddelAppen.Extensions;

namespace PaddelAppen
{
    public partial class ListPage
    {
        public ListPage()
        {
            InitializeComponent();
            //this.BindingContext = new ListPageViewModel(this, Navigation, listView);
            
            listView.ItemTemplate = new DataTemplate(typeof(PointCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Name");
            listView.BackgroundColor = Color.White;
            listView.ItemsSource = App.Database.GetPoIs();

            var FilterAllTBI = new ToolbarItem("+", "sleepingbag24", () =>
            {
                listView.ItemsSource = App.Database.GetPoIs();
            }, 0, 0);

            var FilterRentalsTBI = new ToolbarItem("+", "cardinuse24", () =>
            {
                listView.ItemsSource = App.Database.GetPoIsByType(MapExtensions.LocationType.Rental);
            }, 0, 0);

            var FilterTrailsTBI = new ToolbarItem("+", "polyline24", () =>
            {
                listView.ItemsSource = App.Database.GetPoIsByType(MapExtensions.LocationType.Trail);
            }, 0, 0);
            var FilterCampTBI = new ToolbarItem("+", "matches24", () =>
            {
                listView.ItemsSource = App.Database.GetPoIsByType(MapExtensions.LocationType.Camp);
            }, 0, 0);

            ToolbarItems.Add(FilterAllTBI);
            ToolbarItems.Add(FilterRentalsTBI);
            ToolbarItems.Add(FilterTrailsTBI);
            ToolbarItems.Add(FilterCampTBI);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        
        protected async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listItem = e.SelectedItem as PointOfInterest;
            //var listItemPage = new ListItemXaml();
            var detailsPage = new DetailsPage(listItem);
            //listItemPage.BindingContext = listItem;
            await Navigation.PushAsync(detailsPage);
        }

        /*
                //För att lägga till punkter manuellt, for testing and stuffs
                var tbi = new ToolbarItem("+", null, () =>
                {
                    var listItem = new PointOfInterest();
                    var listItemPage = new ListItemXaml();
                    listItemPage.BindingContext = listItem;
                    Navigation.PushAsync(listItemPage);
                }, 0, 0);
                if (Device.OS == TargetPlatform.Android)
                { // BUG: Android doesn't support the icon being null
                    tbi = new ToolbarItem("+", "plus", () =>
                    {
                        var listItem = new PointOfInterest();
                        var listItemPage = new ListItemXaml();
                        listItemPage.BindingContext = listItem;
                        Navigation.PushAsync(listItemPage);
                    }, 0, 0);
                }
            
                ToolbarItems.Add(tbi);
             */
    }
}
