using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoranApp
{
    public partial class LocationPopup : ContentPage
    {
        private List<string> PopularCities = new List<string>
        {
            "���",
            "�����",
            "�����",
            "����",
            "�����"
        };

        public LocationPopup()
        {
            InitializeComponent();
            // ������������ ������� ��������� ��� Entry
            LocationEntry.Keyboard = Keyboard.Default;
            // �������� �������� �� �������� ����
            LocationSuggestions.ItemsSource = PopularCities;
        }

        private void OnLocationTextChanged(object sender, TextChangedEventArgs e)
        {
            string query = e.NewTextValue.Trim().ToLower();

            // Գ������� ����, �� ��������� �������� �����
            var filteredCities = PopularCities.Where(city => city.ToLower().Contains(query)).ToList();
            LocationSuggestions.ItemsSource = filteredCities;
        }

        private async void OnLocationSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                string selectedLocation = e.CurrentSelection[0].ToString();
                MessagingCenter.Send(this, "LocationChanged", selectedLocation);
                await Navigation.PopModalAsync();
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
