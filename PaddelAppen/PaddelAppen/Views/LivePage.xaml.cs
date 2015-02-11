using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaddelAppen.ViewModels;
using Xamarin.Forms;

namespace PaddelAppen.Views
{
	public partial class LivePage : ContentPage
	{
		public LivePage (PointOfInterest point)
		{
			InitializeComponent ();
            this.BindingContext = new LivePageViewModel(point, this, MapStack);
		}
	}
}
