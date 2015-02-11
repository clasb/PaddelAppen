using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PaddelAppen
{
    public partial class PointCellXaml : ContentView
    {
        public PointCellXaml()
        {
            InitializeComponent();
            
        }
    }

    public class PointCell : TextCell
    {
        public PointCell()
        {
            this.TextColor = Color.Black;
            this.DetailColor = Color.Gray;
            //this.Command = new Command(async () => await OpenListPage(this.CommandParameter));
        }

        //För att få vymodellen helt oberoende av vyn behövs ListPage-listans onclicked
        //flyttas hit, eftersom varje post i listan är en textcell och textcell har command property
        /*protected async Task OpenListPage(object item)
        {
            var listItem = item as PointOfInterest;
            var detailsPage = new DetailsPage(listItem);
            await Navigation.PushAsync(detailsPage);
        }*/
    }
}
