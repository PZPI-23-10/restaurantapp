using System;
using Microsoft.Maui.Controls;

namespace RestoranApp
{
    public partial class ReservationPopup : ContentPage
    {
        public int SelectedPeople { get; set; } = 2;
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public ReservationPopup()
        {
            InitializeComponent();

            for (int i = 1; i <= 10; i++)
            {
                PeoplePicker.Items.Add(i.ToString());
            }

            PeoplePicker.SelectedIndex = SelectedPeople - 1;
            DateSelector.MinimumDate = DateTime.Today;
            DateSelector.MaximumDate = DateTime.Today.AddDays(7);
            DateSelector.Date = SelectedDate;
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            SelectedPeople = PeoplePicker.SelectedIndex + 1;
            SelectedDate = DateSelector.Date;

            MessagingCenter.Send(this, "ReservationChanged", (SelectedPeople, SelectedDate));
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
