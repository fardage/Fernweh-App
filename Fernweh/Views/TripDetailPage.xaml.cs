﻿using System;
using System.Threading.Tasks;
using Fernweh.ViewModels;
using Xamarin.Forms;

namespace Fernweh.Views
{
    public partial class TripDetailPage : ContentPage
    {
        private readonly TripDetailViewModel viewModel;

        public TripDetailPage(TripDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private async void Menu_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Edit Trip", DialogActions.Cancel,
                DialogActions.Delete, DialogActions.Rename, DialogActions.AddTemplateCategory,
                DialogActions.AddEmptyCategory);

            switch (action)
            {
                case DialogActions.Delete:
                    await DeleteItem_Clicked();
                    break;
                case DialogActions.Rename:
                    await RenameItem_Clicked();
                    break;
                case DialogActions.AddTemplateCategory:
                    await AddTemplateCategory_Clicked();
                    break;
                case DialogActions.AddEmptyCategory:
                    await AddEmptyCategory_Clicked();
                    break;
            }
        }

        private async Task AddEmptyCategory_Clicked()
        {
            var categoryName = await DisplayPromptAsync("Add Empty Category", "Enter New Category Name:");

            if (!string.IsNullOrEmpty(categoryName)) viewModel.AddEmptyCategoryAsync(categoryName);
        }

        private async Task DeleteItem_Clicked()
        {
            await Navigation.PopAsync().ContinueWith(_ => MessagingCenter.Send(this, "DeleteTrip", viewModel.Trip));
        }

        private async Task RenameItem_Clicked()
        {
            var tripName = await DisplayPromptAsync("Rename Trip", "Enter New Trip Name:");

            if (!string.IsNullOrEmpty(tripName))
            {
                Title = tripName;
                viewModel.TripName = tripName;
                viewModel.Trip.Destination = tripName;
                MessagingCenter.Send(this, "RenameTrip", viewModel.Trip);
            }
        }

        private async Task AddTemplateCategory_Clicked()
        {
            var setupPage = new SetupTripPage(new SetupTripViewModel(Navigation, viewModel.Trip));
            await Navigation.PushModalAsync(new NavigationPage(setupPage));
        }
    }

    internal static class DialogActions
    {
        public const string Cancel = "Cancel";
        public const string Delete = "Delete";
        public const string Rename = "Rename";
        public const string AddTemplateCategory = "Add Category from Template";
        public const string AddEmptyCategory = "Add Category Empty";
    }
}