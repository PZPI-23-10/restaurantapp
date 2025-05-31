namespace RestoranApp;

public partial class SignUp2 : ContentPage
{
	public SignUp2(string email)
	{
		InitializeComponent();
        emailEntry.Text = email;

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
    private void GoogleTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://google.com");
    }
    private void FacebookTapped(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://facebook.com");
    }
    private async void OnSignUpButtonClick(object sender, EventArgs e)
    {
        if (passwordEntry.Text != passwordConfirm.Text)
        {
            DisplayAlert("Помилка", "Паролі не зходиться", "ОК");

        }
        else
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}