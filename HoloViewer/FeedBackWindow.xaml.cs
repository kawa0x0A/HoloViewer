using System.Windows.Input;

namespace HoloViewer;

public partial class FeedBackWindow : ContentPage
{
    public string Version { get; } = FeedBackInfoValue.VersionString;

    public string ReleaseDate { get; } = FeedBackInfoValue.ReleaseDate;

    public string SoftwareLicense { get; } = FeedBackInfoValue.SoftwareLicense;

    public string MailAddress { get; } = FeedBackInfoValue.MailAddress;

    public string RepositoryPageUrl { get; } = FeedBackInfoValue.RepositoryPageUrl;

    public string TwitterProfilePageUrl { get; } = FeedBackInfoValue.TwitterProfilePageUrl;

    public string FeedBackDescription { get; } = FeedBackInfoValue.FeedBackDescription;

    public ICommand ClickMailAddress => new Command(async () => await Clipboard.Default.SetTextAsync(MailAddress));

    public ICommand ClickTwitterLink => new Command(async () => await Browser.Default.OpenAsync(TwitterProfilePageUrl));

    public ICommand ClickGithubLink => new Command(async () => await Browser.Default.OpenAsync(RepositoryPageUrl));

    public FeedBackWindow()
    {
        InitializeComponent();

        BindingContext = this;
    }
}
