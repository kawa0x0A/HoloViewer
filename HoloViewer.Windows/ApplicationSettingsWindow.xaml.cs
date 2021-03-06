using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            public class HashTagSettingsDataSet : INotifyPropertyChanged
            {
                private bool isUseHashTag;
                private string hashTagName;

                public bool IsUseHashTag { get { return isUseHashTag; } set { isUseHashTag = value; NotifyPropertyChanged(); } }

                public string HashTagName { get { return hashTagName; } set { hashTagName = value; NotifyPropertyChanged(); } }

                public event PropertyChangedEventHandler PropertyChanged;

                private void NotifyPropertyChanged ([CallerMemberName] string propertyName = "")
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private string startPageUrl;
            private string captureSavePath;
            private bool isEnableAutoInsertHashTagYoutubeTag;
            private ObservableCollection<HashTagSettingsDataSet> isUseHashTags;
            private bool isEnableAutoInsertHashTagHoloViewer;
            private bool isEnableUpdateCheck;

            public string StartUpPageUrl { get { return startPageUrl; } set { startPageUrl = value; NotifyPropertyChanged(); } }

            public string CaptureSavePath { get { return captureSavePath; } set { captureSavePath = value; NotifyPropertyChanged(); } }

            public bool IsEnableAutoInsertHashTagYoutubeTag { get { return isEnableAutoInsertHashTagYoutubeTag; } set { isEnableAutoInsertHashTagYoutubeTag = value; NotifyPropertyChanged(); } }

            public ObservableCollection<HashTagSettingsDataSet> IsUseHashTags { get { return isUseHashTags; } set { isUseHashTags = value; NotifyPropertyChanged(); } }

            public bool IsEnableAutoInsertHashTagHoloViewer { get { return isEnableAutoInsertHashTagHoloViewer; } set { isEnableAutoInsertHashTagHoloViewer = value; NotifyPropertyChanged(); } }

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
