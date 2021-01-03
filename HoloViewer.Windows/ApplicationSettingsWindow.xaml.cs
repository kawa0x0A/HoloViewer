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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HoloViewer.Windows
{
    /// <summary>
    /// ApplicationSettingsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ApplicationSettingsWindow : Window
    {
        public class ApplicationSettingsDataSet : INotifyPropertyChanged
        {
            private string startPageUrl;
            private string captureSavePath;
            private bool isEnableUpdateCheck;

            public string StartUpPageUrl { get { return startPageUrl; } set { startPageUrl = value; NotifyPropertyChanged(); } }

            public string CaptureSavePath { get { return captureSavePath; } set { captureSavePath = value; NotifyPropertyChanged(); } }

            public bool IsEnableUpdateCheck { get { return isEnableUpdateCheck; } set { isEnableUpdateCheck = value; NotifyPropertyChanged(); } }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged ([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool Result { get; private set; } = false;

        public ApplicationSettingsDataSet CurrentApplicationSettingsDataSet { get; set; } = new ApplicationSettingsDataSet();

        public ApplicationSettingsWindow ()
        {
            InitializeComponent();

            DataContext = CurrentApplicationSettingsDataSet;
        }

        private void Button_Click_Select_Directory (object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CurrentApplicationSettingsDataSet.CaptureSavePath = System.IO.Path.GetFullPath(dialog.FileName);
            }

            Focus();
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
