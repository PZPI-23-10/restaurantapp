using Microsoft.Maui.Controls;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestoranApp;

public partial class SearchPage : ContentPage
{
    private const string NOMINATIM_URL = "https://nominatim.openstreetmap.org/search?format=json&q=";

    public SearchPage()
    {
        InitializeComponent();
        MapWebView.Source = "file:///android_asset/map.html";
    }

    private async void OnHomeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }


    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        var city = SearchEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(city))
        {
            await DisplayAlert("Попередження", "Введіть назву міста для пошуку", "OK");
            return;
        }

        try
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync(NOMINATIM_URL + city);
            var results = JsonSerializer.Deserialize<List<NominatimResult>>(response);

            if (results != null && results.Count > 0)
            {
                var location = results[0];
                var message = JsonSerializer.Serialize(new { type = "search", lat = location.lat, lng = location.lon });

                // Відправка повідомлення до WebView
                MapWebView.Dispatcher.Dispatch(() =>
                {
                    MapWebView.Eval($"window.postMessage('{message}')");
                });
            }
            else
            {
                await DisplayAlert("Помилка", "Місто не знайдено", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка пошуку: {ex.Message}");
            await DisplayAlert("Помилка", "Не вдалося знайти місто", "OK");
        }
    }
}

public class NominatimResult
{
    public string lat { get; set; }
    public string lon { get; set; }
    public string display_name { get; set; }
}
