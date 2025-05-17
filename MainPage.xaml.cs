
namespace RestoranApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Підписуємось на зміну резервації
            MessagingCenter.Subscribe<ReservationPopup, (int, DateTime)>(this, "ReservationChanged", (sender, data) =>
            {
                int people = data.Item1;
                DateTime date = data.Item2;

                // Оновлюємо кнопки на головній сторінці
                dateGuestButton.Text = $"{people} 👤 - {date:dd/MM/yyyy}";
            });
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
                ((Button)sender).Text = location;
            });
            await Navigation.PushModalAsync(locationPopup);
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

    }
}

