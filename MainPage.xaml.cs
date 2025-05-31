using System.Net.Http.Json;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace RestoranApp
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private string _selectedCity = null;

        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<ReservationPopup, (int, DateTime)>(this, "ReservationChanged", (sender, data) =>
            {
                int people = data.Item1;
                DateTime date = data.Item2;
                dateGuestButton.Text = $"{people} 👤 - {date:dd/MM/yyyy}";
            });

            LoadRestaurantsAsync();
        }
        private void OnResetLocationClicked(object sender, EventArgs e)
        {
            _selectedCity = null;
            locationButton.Text = "Оберіть місто";
            LoadRestaurantsAsync();
        }
        private async void LoadRestaurantsAsync()
        {
            try
            {
                string url = "https://backend-restoran.onrender.com/api/Restaurant";
                var response = await _httpClient.GetStringAsync(url);
                var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(response);

                // Фільтрація за містом, якщо місто вибране
                if (!string.IsNullOrEmpty(_selectedCity))
                {
                    restaurants = restaurants
                        .Where(r => r.City?.Equals(_selectedCity, StringComparison.OrdinalIgnoreCase) == true)
                        .ToList();
                }

                RestaurantList.Children.Clear(); // Очистка перед оновленням

                foreach (var restaurant in restaurants)
                {
                    var layout = CreateRestaurantView(restaurant);
                    RestaurantList.Children.Add(layout);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", "Не вдалося завантажити ресторани: " + ex.Message, "OK");
            }
        }


        private View CreateRestaurantView(Restaurant restaurant)
        {
            var image = new Image
            {
                Source = restaurant.ImageUrl,
                Aspect = Aspect.AspectFill
            };

            var frame = new Frame
            {
                WidthRequest = 150,
                HeightRequest = 200,
                BackgroundColor = Colors.White,
                CornerRadius = 15,
                Padding = 0,
                HasShadow = true,
                Content = image
            };

            var nameLabel = new Label
            {
                Text = restaurant.Name,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colors.Black
            };

            var heartButton = new Button
            {
                Text = "♡",
                FontSize = 18,
                BackgroundColor = Colors.Wheat,
                TextColor = Colors.Red
            };

            heartButton.Clicked += (s, e) =>
            {
                FavoriteService.ToggleFavorite(restaurant);
                heartButton.Text = FavoriteService.IsFavorite(restaurant) ? "❤️" : "♡";
            };
            heartButton.Text = FavoriteService.IsFavorite(restaurant) ? "❤️" : "♡";


            var calendarButton = new Button
            {
                Text = "📅",
                FontSize = 18,
                BackgroundColor = Colors.Wheat
            };

            var buttons = new HorizontalStackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                Children = { calendarButton, heartButton }
            };

            return new VerticalStackLayout
            {
                Spacing = 10,
                Children = { frame, nameLabel, buttons }
            };
        }


        private async void OnDateGuestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ReservationPopup());
        }

        private async void OnLocationButtonClicked(object sender, EventArgs e)
        {
            var locationPopup = new LocationPopup();
            MessagingCenter.Subscribe<LocationPopup, string>(this, "LocationChanged", (popup, location) =>
            {
                _selectedCity = location;
                ((Button)sender).Text = location;
                LoadRestaurantsAsync(); // Перезавантаження з фільтрацією
            });
            await Navigation.PushModalAsync(locationPopup);
        }


        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void OnSavedRestaurantButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedRestaurant());
        }
    }

    public class Restaurant
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("photoUrl")]
        public string ImageUrl { get; set; }

        public string City { get; set; } // Додай це
    }
}
