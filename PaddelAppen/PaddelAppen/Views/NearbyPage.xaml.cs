using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PaddelAppen.ViewModels;

namespace PaddelAppen
{
    public partial class NearbyPage
    {
        public NearbyPage()
        {
            InitializeComponent();
            this.BindingContext = new NearbyPageViewModel(this, MapStack);
            this.BackgroundColor = Color.White;

            listView.ItemTemplate = new DataTemplate(typeof(PointCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Name");
            listView.BackgroundColor = Color.White;
            //listView.ItemsSource = App.Database.GetPoIs();
            listView.ItemsSource = App.Database.GetPoIByLocation(App.CurrentLocation.Latitude, App.CurrentLocation.Longitude);
        }

        protected async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listItem = e.SelectedItem as PointOfInterest;
            //var listItemPage = new ListItemXaml();
            var detailsPage = new DetailsPage(listItem);
            //listItemPage.BindingContext = listItem;
            await Navigation.PushAsync(detailsPage);
        }
    }
}
