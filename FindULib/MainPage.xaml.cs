using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using FindULib.Common;

namespace FindULib
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            CommonHelper.CheckVersion();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            string keyWord = this.txtTitle.Text;
            if (keyWord.Trim() == string.Empty)
            {
                MessageHelper.Show("请填写关键词");
            }
            else
            {
                NavigationService.Navigate(new Uri("/Views/BookList.xaml?keyWord=" + Uri.EscapeDataString(keyWord), UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_BackKeyPress_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("真的要离开吗？", "我会等你的", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.txtTitle.Text = "";
        }

        private void CheckNetworkInfomation()
        {
            if (!DeviceNetworkInformation.IsNetworkAvailable)
            {
                MessageHelper.Show("当前网络不可用");
            }
        }

        private void Panorama_Loaded_1(object sender, RoutedEventArgs e)
        {
            CheckNetworkInfomation();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CommonHelper.CheckVersion();
        }
    }
}