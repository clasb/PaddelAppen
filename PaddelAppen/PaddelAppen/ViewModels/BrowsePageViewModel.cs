using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PaddelAppen.ViewModels
{
    class BrowsePageViewModel
    {
        INavigation Navigation;
        Command listButtonCommand;
        Command mapButtonCommand;
        Command nearbyButtonCommand;
        Command favouriteButtonCommand;

        public BrowsePageViewModel(INavigation nav)
        {
            Navigation = nav;
        }

        public Command ListButtonCommand
        {
            get
            {
                return listButtonCommand ??
                    (listButtonCommand = new Command(async () => await OpenListPage()));
            }
        }

        public Command MapButtonCommand
        {
            get
            {
                return mapButtonCommand ??
                    (mapButtonCommand = new Command(async () => await OpenMapPage()));
            }
        }

        public Command NearbyButtonCommand
        {
            get
            {
                return nearbyButtonCommand ??
                    (nearbyButtonCommand = new Command(async () => await OpenNearbyPage()));
            }
        }

        public Command FavouritesButtonCommand
        {
            get
            {
                return favouriteButtonCommand ??
                    (favouriteButtonCommand = new Command(async () => await UpdateCurrentPosition()));
            }
        }

        protected async Task OpenListPage()
        {
            var listP = new ListPage();
            listP.BindingContext = App.Database.GetPoIs();
            await Navigation.PushAsync(listP);
        }

        protected async Task OpenMapPage()
        {
            await Navigation.PushAsync(new MapPage());
        }

        protected async Task OpenNearbyPage()
        {
            await Navigation.PushAsync(new NearbyPage());
        }

        protected async Task UpdateCurrentPosition()
        {
           

        }
    }
}
