using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PaddelAppen.ViewModels;

namespace PaddelAppen
{
    public partial class BrowsePage
    {
        public BrowsePage()
        {
            InitializeComponent();
            BackgroundImage = "background.png";
            this.BindingContext = new BrowsePageViewModel(Navigation);
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            //LocationText.Text = App.CurrentLocation.Latitude.ToString() + " " + App.CurrentLocation.Longitude.ToString();
        }
    }
}
