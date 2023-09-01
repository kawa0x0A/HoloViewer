namespace HoloViewer;

public partial class MainToolbar : ContentView
{
    public MainToolbar()
    {
        InitializeComponent();
    }

    private async void ScreenSingle_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenSingle());
    }

    private async void ScreenHorizontal2_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenHorizontal2());
    }

    private async void ScreenVertical2_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenVertical2());
    }

    private async void ScreenHorizontal3_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenHorizontal3());
    }

    private async void ScreenVertical3_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenVertical3());
    }

    private async void ScreenCustom3_1_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenCustom3_1());
    }

    private async void ScreenCustom3_2_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenCustom3_2());
    }

    private async void ScreenHorizontal4_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenHorizontal4());
    }

    private async void ScreenVertical4_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenVertical4());
    }

    private async void ScreenCustom4_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreenCustom4());
    }

    private async void ShowApplicationSettingsDialog(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ApplicationSettingsPage());
    }

    private async void ShowFeedBackDialog(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FeedBackWindow());
    }

    private async void OpenBillingPage(object sender, EventArgs e)
    {
        const string BillingPageUrl = @"https://holoviewer-public.web.app/";

        await Browser.Default.OpenAsync(BillingPageUrl);
    }
}
