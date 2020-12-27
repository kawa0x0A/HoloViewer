using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace HoloViewer.Windows
{
    /// <summary>
    /// UpdateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public class UpdateDataSet : INotifyPropertyChanged
        {
            private string stateTextValue;
            private float stateProgressValue;
            private string functionTextValue;
            private string functionProgressTextValue;
            private float functionProgressValue;

            public string StateTextValue { get { return stateTextValue; } set { NotifyPropertyChanged(); stateTextValue = value; } }

            public float StateProgressValue { get { return stateProgressValue; } set { NotifyPropertyChanged(); stateProgressValue = value; } }

            public string FunctionTextValue { get { return functionTextValue; } set { NotifyPropertyChanged(); functionTextValue = value; } }

            public string FunctionProgressTextValue { get { return functionProgressTextValue; } set { NotifyPropertyChanged(); functionProgressTextValue = value; } }

            public float FunctionProgressValue { get { return functionProgressValue; } set { NotifyPropertyChanged(); functionProgressValue = value; } }

            public bool IsCancelDownload { get; set; } = false;

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged ([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public UpdateDataSet UpdateDataSetValue { get; set; } = new UpdateDataSet();

        public UpdateWindow ()
        {
            InitializeComponent();

            this.DataContext = UpdateDataSetValue;
        }

        private void Button_Click (object sender, RoutedEventArgs e)
        {
            UpdateDataSetValue.IsCancelDownload = true;
        }

        private void Window_Closed (object sender, EventArgs e)
        {
            UpdateDataSetValue.IsCancelDownload = true;
        }
    }
}
