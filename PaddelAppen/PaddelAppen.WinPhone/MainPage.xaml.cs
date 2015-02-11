using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;

using Xamarin.Forms;
using System.IO;
using SQLite;


namespace PaddelAppen.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();
            Xamarin.FormsMaps.Init();
            string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            var plat = new SQLite.Net.Platform.WindowsPhone8.SQLitePlatformWP8();
            var conn = new SQLite.Net.SQLiteConnection(plat, dbPath);

            // Set the database connection string
            PaddelAppen.App.SetDatabaseConnection(conn);
            //string sqliteFilename = "TodoSQLite.db3";

            //var path = Path.Combine(documentsPath, sqliteFilename);

            // This is where we copy in the prepopulated database
            

            Content = PaddelAppen.App.GetMainPage().ConvertPageToUIElement(this);
        }
    }
}
