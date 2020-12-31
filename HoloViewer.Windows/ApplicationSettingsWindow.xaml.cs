using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;

namespace HoloViewer.Windows
{
    /// <summary>
    /// ApplicationSettingsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ApplicationSettingsWindow : Window
    {
        private string startPageUrl;
        private bool isEnableUpdateCheck;

        public bool Result { get; private set; } = false;

        public string StartUpPageUrl { get { return startPageUrl; } set { NotifyPropertyChanged(); startPageUrl = value; } }

        public bool IsEnableUpdateCheck { get { return isEnableUpdateCheck; } set { NotifyPropertyChanged(); isEnableUpdateCheck = value; } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged ([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ApplicationSettingsWindow ()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Button_Click_OK (object sender, RoutedEventArgs e)
        {
            Result = true;

            Close();
        }

        private void Button_Click_Cancel (object sender, RoutedEventArgs e)
        {
            Result = false;

            Close();
        }
    }
}
