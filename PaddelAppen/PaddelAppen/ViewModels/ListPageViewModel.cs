using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PaddelAppen.Extensions;

namespace PaddelAppen.ViewModels
{
    class ListPageViewModel
    {
        private Command itemSelectedCommand;
        private INavigation Navigation;
        private ListView _ListView;

        public ListPageViewModel(ListPage page, INavigation nav, ListView lv)
        {
            Navigation = nav;
            _ListView = lv;
            _ListView.ItemTemplate = new DataTemplate(typeof(PointCell));
            _ListView.ItemTemplate.SetBinding(TextCell.TextProperty, "Name");
            _ListView.BackgroundColor = Color.White;
            _ListView.ItemsSource = App.Database.GetPoIs();

            var FilterAllTBI = new ToolbarItem("+", "sleepingbag24", () =>
            {
                _ListView.ItemsSource = App.Database.GetPoIs();
            }, 0, 0);

            var FilterRentalsTBI = new ToolbarItem("+", "cardinuse24", () =>
            {
                _ListView.ItemsSource = App.Database.GetPoIsByType(MapExtensions.LocationType.Rental);
            }, 0, 0);

            var FilterTrailsTBI = new ToolbarItem("+", "polyline24", () =>
            {
                _ListView.ItemsSource = App.Database.GetPoIsByType(MapExtensions.LocationType.Trail);
            }, 0, 0);
            var FilterCampTBI = new ToolbarItem("+", "matches24", () =>
            {
                _ListView.ItemsSource = App.Database.GetPoIsByType(MapExtensions.LocationType.Camp);
            }, 0, 0);

            page.ToolbarItems.Add(FilterAllTBI);
            page.ToolbarItems.Add(FilterRentalsTBI);
            page.ToolbarItems.Add(FilterTrailsTBI);
            page.ToolbarItems.Add(FilterCampTBI);
        }

        /*
         * Skapa property, tex current selected item, låt denna uppdateras när SelectionChanged triggas.
         * I gettern returnerar den bara selected item och i settern kör den kommandot som öppnar 
         * sidan, dvs metoden under
         */

        public Command ItemSelectedCommand
        {
            get
            {
                return itemSelectedCommand ??
                    (itemSelectedCommand = new Command(async () => await OpenSelectedItem()));
            }
        }

        protected async Task OpenSelectedItem()
        {
            var listItem = _ListView.SelectedItem as PointOfInterest;
            //var listItemPage = new ListItemXaml();
            var detailsPage = new DetailsPage(listItem);
            //listItemPage.BindingContext = listItem;
            await Navigation.PushAsync(detailsPage);
        }
    }
}