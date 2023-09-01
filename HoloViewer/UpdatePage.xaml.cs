using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HoloViewer;

public partial class UpdatePage : ContentPage
{
    public partial class UpdateDataSet : ObservableObject
    {
        [ObservableProperty]
        private string stateTextValue;

        [ObservableProperty]
        private float stateProgressValue;

        [ObservableProperty]
        private string functionTextValue;

        [ObservableProperty]
        private string functionProgressTextValue;

        [ObservableProperty]
        private float functionProgressValue;

        public bool IsCancelDownload { get; set; } = false;
    }

    public UpdateDataSet BindingUpdateDataSet { get; set; } = new UpdateDataSet();

    public UpdatePage()
    {
        InitializeComponent();

        BindingContext = BindingUpdateDataSet;
    }

    private void Button_Click(object sender, EventArgs e)
    {
        BindingUpdateDataSet.IsCancelDownload = true;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        BindingUpdateDataSet.IsCancelDownload = true;
    }
}
