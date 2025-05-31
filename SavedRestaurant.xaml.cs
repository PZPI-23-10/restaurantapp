using System.Collections.ObjectModel;

namespace RestoranApp;

public partial class SavedRestaurant : ContentPage
{
    private ObservableCollection<Restaurant> savedRestaurants = new();

    public SavedRestaurant()
    {
        InitializeComponent();
        savedRestaurants = FavoriteService.Favorites;
        SavedRestaurantsView.ItemsSource = savedRestaurants;
    }


    private void OnUnfavoriteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Restaurant restaurant)
        {
            savedRestaurants.Remove(restaurant);
        }
    }

    private async void OnHomeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }
}