using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaddelAppen.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PaddelAppen.Controls;

namespace PaddelAppen
{
    public partial class DetailsPage
    {
        public DetailsPage(PointOfInterest point)
        {
            InitializeComponent();
            this.BindingContext = new DetailsPageViewModel(point, this, MapStack);
            //TableVie.Intent = TableIntent.Data;
            if (point.Type != Extensions.MapExtensions.LocationType.Trail)
            {
                Live.IsEnabled = false;
                Live.IsVisible = false;
            }
        }
    }
}
