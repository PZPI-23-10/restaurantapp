using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace RestoranApp
{
    public partial class LocationPopup : ContentPage
    {
        private HttpClient _httpClient = new HttpClient();

        public LocationPopup()
        {
            InitializeComponent();
            LocationEntry.Keyboard = Keyboard.Default;

            // Завантажити динамічні міста з API
            LoadCitiesFromRestaurantsAsync();
        }

        private async void LoadCitiesFromRestaurantsAsync()
        {
            try
            {
                string url = "https://backend-restoran.onrender.com/api/Restaurant";
                var response = await _httpClient.GetStringAsync(url);
                var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(response);

                var uniqueCities = restaurants
                    .Select(r => r.City?.Trim())
                    .Where(city => !string.IsNullOrEmpty(city))
                    .Distinct()
                    .OrderBy(city => city)
                    .ToList();

                LocationSuggestions.ItemsSource = uniqueCities;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", "Не вдалося завантажити міста: " + ex.Message, "OK");
            }
        }

        private void OnLocationTextChanged(object sender, TextChangedEventArgs e)
        {
            if (LocationSuggestions.ItemsSource is not IEnumerable<string> allCities)
                return;

            string query = e.NewTextValue.Trim().ToLower();

            var filtered = allCities
                .Where(city => city.ToLower().Contains(query))
                .ToList();

            LocationSuggestions.ItemsSource = filtered;
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
