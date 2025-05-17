using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls;

namespace RestoranApp;

public partial class SearchPage : ContentPage
{
    public SearchPage()
    {
        InitializeComponent();

    }

    private async void OnHomeButtonClicked(object sender, EventArgs e)
    {
        // Повертаємось на головну сторінку
        await Navigation.PopToRootAsync();
    }


}