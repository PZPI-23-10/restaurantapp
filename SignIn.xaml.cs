namespace RestoranApp;

public partial class SignIn : ContentPage
{
	public SignIn()
	{
		InitializeComponent();
	}


    private void OnTermsTapped(object sender, EventArgs e)
    {
        
        Launcher.OpenAsync("https://example.com/terms");
    }

    private void OnPrivacyTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://example.com/privacy");
    }
    private void GoogleTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://google.com");
    }
    private void FacebookTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://facebook.com");
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}