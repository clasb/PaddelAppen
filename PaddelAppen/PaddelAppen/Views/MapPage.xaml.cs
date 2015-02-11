using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PaddelAppen.ViewModels;

namespace PaddelAppen
{
    public partial class MapPage
    {
        public MapPage()
        {
            InitializeComponent();
            this.BindingContext = new MapPageViewModel(this, MapStack);
            this.BackgroundColor = Color.White;
        }
    }
}
