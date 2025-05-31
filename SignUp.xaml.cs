namespace RestoranApp;

using Microsoft.Maui.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
        InitializeComponent();

	}
    private void OnTermsTapped(object sender, EventArgs e)
    {
        // Наприклад, відкриваєш сторінку з умовами
        Launcher.OpenAsync("https://example.com/terms");
    }

    private void OnPrivacyTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://example.com/privacy");
    }
    private async void GoogleTapped(object sender, EventArgs e)
    {
        try
        {
            var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new Uri("https://accounts.google.com/o/oauth2/v2/auth" +
                        "?client_id=336693737634-ri7bdmguvvj6kbb6po3r2dg43v6aa0vt.apps.googleusercontent.com" +
                        "&redirect_uri=com.companyname.restoranapp:/auth" +
                        "&response_type=code" +
                        "&scope=openid%20email%20profile"),
                new Uri("com.companyname.restoranapp:/auth"));

            string code = authResult?.Properties["code"];

            var httpClient = new HttpClient();
            var values = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", "336693737634-ri7bdmguvvj6kbb6po3r2dg43v6aa0vt.apps.googleusercontent.com" },
            { "redirect_uri", "com.companyname.restoranapp:/auth" },
            { "grant_type", "authorization_code" }
        };

            var content = new FormUrlEncodedContent(values);
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseString);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("id_token", out var idTokenElement))
            {
                string idToken = idTokenElement.GetString();

                // ?? Розпарсимо ID Token (JWT)
                string[] parts = idToken.Split('.');
                if (parts.Length == 3)
                {
                    string payload = parts[1];
                    payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                    byte[] bytes = Convert.FromBase64String(payload);
                    string jsonPayload = Encoding.UTF8.GetString(bytes);

                    var userInfo = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonPayload);

                    string email = userInfo.ContainsKey("email") ? userInfo["email"].ToString() : "немає email";
                    string name = userInfo.ContainsKey("name") ? userInfo["name"].ToString() : "немає імені";
                    string picture = userInfo.ContainsKey("picture") ? userInfo["picture"].ToString() : "";

                    await DisplayAlert("Вхід успішний", $"Ім'я: {name}\nEmail: {email}", "ОК");
                }
                else
                {
                    await DisplayAlert("Помилка", "Неправильний формат ID токена", "ОК");
                }
            }
            else
            {
                await DisplayAlert("Помилка", "Не знайдено ID токен", "ОК");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "ОК");
        }
    }

    private void FacebookTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://facebook.com");
    }
    private  async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text?.Trim();

        if (string.IsNullOrEmpty(email))
        {
            DisplayAlert("Помилка", "Поле Email не може бути порожнім", "ОК");
        }
        else
        {

            await Navigation.PushAsync(new SignUp2(email));
            
        }
    }
    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignIn());
    }

}

/*
https://accounts.google.com/o/oauth2/v2/auth?client_id=1016114515774-h2hspsl0cik8llkkgr6sjpsjropn3hfn.apps.googleusercontent.com&redirect_uri=https://www.twitch.tv/&response_type=code&scope=openid%20email%20profile*/