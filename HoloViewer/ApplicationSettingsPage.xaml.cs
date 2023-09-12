using CommunityToolkit.Maui.Storage;

namespace HoloViewer;

public partial class ApplicationSettingsPage : ContentPage
{
    public ApplicationSettingsPage()
    {
        InitializeComponent();

        ThemeColorLight.IsChecked = false;
        ThemeColorDark.IsChecked = false;
        ThemeColorSystem.IsChecked = false;

        switch (ApplicationSettings.Current.Theme)
        {
            case AppTheme.Light:
                ThemeColorLight.IsChecked = true;
                break;

            case AppTheme.Dark:
                ThemeColorDark.IsChecked = true;
                break;

            case AppTheme.Unspecified:
                ThemeColorSystem.IsChecked = true;
                break;
        }

        BindingContext = ApplicationSettings.Current;
    }

    private async void Button_Click_Select_Directory(object sender, EventArgs e)
    {
        var result = await FolderPicker.Default.PickAsync(default);

        if ((result != null) && (result.IsSuccessful))
        {
            ApplicationSettings.Current.CaptureSavePath = Path.GetFullPath(result.Folder.Path);
        }

        Focus();
    }

    private void Button_Click_HashTag_Remove(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var hashTag = (HashTag)button.BindingContext;

        ApplicationSettings.Current.HashTags.Remove(hashTag);

        ApplicationSettings.Save();
    }

    private void RadioButtonLight_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Application.Current.UserAppTheme = AppTheme.Light;

        ApplicationSettings.Current.Theme = AppTheme.Light;

        ApplicationSettings.Save();
    }

    private void RadioButtonDark_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Application.Current.UserAppTheme = AppTheme.Dark;

        ApplicationSettings.Current.Theme = AppTheme.Dark;

        ApplicationSettings.Save();
    }

    private void RadioButtonSystem_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Application.Current.UserAppTheme = AppTheme.Unspecified;

        ApplicationSettings.Current.Theme = AppTheme.Unspecified;

        ApplicationSettings.Save();
    }

    protected override bool OnBackButtonPressed()
    {
        ApplicationSettings.Save();

        return base.OnBackButtonPressed();
    }
}
