using Microsoft.MobileBlazorBindings.WebView.Windows;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace HoloViewer.Windows
{
    public class MainWindow : FormsApplicationPage
    {
        [STAThread]
        public static void Main()
        {
            var app = new System.Windows.Application();
            app.Run(new MainWindow());
        }

        public MainWindow()
        {
            Forms.Init();
            BlazorHybridWindows.Init();
            LoadApplication(new App());
        }

        protected override void OnActivated (EventArgs e)
        {
            base.OnActivated(e);

            var topAppBar = Template.FindName("PART_TopAppBar", this) as Xamarin.Forms.Platform.WPF.Controls.FormsAppBar; ;

            if (topAppBar != null)
            {
                topAppBar.Height = 0;
            }

            var commandsBar = Template.FindName("PART_CommandsBar", this) as System.Windows.Controls.Grid;

            if (commandsBar != null)
            {
                var titleTextBlock = commandsBar.FindName("PART_System_Title") as System.Windows.Controls.TextBlock;

                if (titleTextBlock != null)
                {
                    titleTextBlock.Text = Application.MainPage.Title;
                }
            }
        }
    }
}
