using System.Collections.ObjectModel;

namespace RestoranApp
{
    public static class FavoriteService
    {
        public static ObservableCollection<Restaurant> Favorites { get; } = new();

        public static void ToggleFavorite(Restaurant restaurant)
        {
            if (Favorites.Any(r => r.Name == restaurant.Name))
                Favorites.Remove(Favorites.First(r => r.Name == restaurant.Name));
            else
                Favorites.Add(restaurant);
        }

        public static bool IsFavorite(Restaurant restaurant)
        {
            return Favorites.Any(r => r.Name == restaurant.Name);
        }
    }
}
