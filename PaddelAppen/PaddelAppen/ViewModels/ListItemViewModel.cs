﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PaddelAppen.Models;

namespace PaddelAppen.ViewModels
{
    class ListItemViewModel
    {
        INavigation Navigation;
        ListItemXaml Page;

        public ListItemViewModel(INavigation nav, ListItemXaml page)
        {
            Navigation = nav;
            Page = page;
        }

        /// <summary>
        /// Binder entries till PointOfInterest properties och sparar objektet i databasen.
        /// PopAsync poppar (tar bort) den översta (nuvarande) sidan i navigationsstacken.
        /// </summary>
        protected async void OnSaveActivated(object sender, EventArgs e)
        {
            var pointItem = (PointOfInterest)Page.BindingContext;
            App.Database.SavePoI(pointItem);
            await this.Navigation.PopAsync();
        }

        /// <summary>
        /// Binder entries till PointOfInterest properties och tar bort objektet via ID.
        /// PopAsync poppar (tar bort) den översta sidan i navigationsstacken.
        /// </summary>
        protected async void OnDeleteActivated(object sender, EventArgs e)
        {
            var pointItem = (PointOfInterest)Page.BindingContext;
            App.Database.DeletePoI(pointItem.ID);
            await this.Navigation.PopAsync();
        }

        /// <summary>
        /// PopAsync poppar (tar bort) den översta sidan i navigationsstacken.
        /// Inget annars görs (cancel).
        /// </summary>
        protected async void OnCancelActivated(object sender, EventArgs e)
        {
            await this.Navigation.PopAsync();
        }
    }
}
