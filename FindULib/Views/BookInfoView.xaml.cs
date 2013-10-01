using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using FindULib.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FindULib.Common;

namespace FindULib.Views
{
    public partial class BookInfoView : PhoneApplicationPage
    {
        private WebClient client;
        private Book book = null;
        private string isbn = string.Empty;
        private string authorName = string.Empty;
        private string publishMessage = string.Empty;
        private string name = string.Empty;

        public BookInfoView()
        {
            InitializeComponent();

            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            book = new Book();

            this.LayoutRoot.DataContext = book;
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //获取ISBN
            string htmlStr = e.Result;
            int isbnStartIndex = htmlStr.IndexOf("ajax_douban.php?isbn=") + 21;
            int isbnLength = htmlStr.IndexOf("\",function(json)") - isbnStartIndex;
            isbn = htmlStr.Substring(isbnStartIndex, isbnLength);

            //从豆瓣获取数据设置简介和图片
            client = new WebClient();

            client.DownloadStringAsync(new Uri("https://api.douban.com/v2/book/isbn/" + isbn, UriKind.Absolute));
            client.DownloadStringCompleted += client_DownloadStringFromDoubanCompleted;
        }

        void client_DownloadStringFromDoubanCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                JObject jobj = (JObject)JsonConvert.DeserializeObject(e.Result);
                book.Name = name;
                book.Isbn = "ISBN：" + isbn;
                book.AutorName = "作者：" + authorName;
                //book.PublishMessage = "出版信息：" + publishMessage;
                book.ImageUrl = jobj["image"].ToString();
                book.Summary = jobj["summary"].ToString().Length > 1000 ? jobj["summary"].ToString().Substring(0, 1000) + "..." : jobj["summary"].ToString();
                //book.Name = jobj["title"].ToString();
                book.PublishDate = "出版日期：" + jobj["pubdate"].ToString();
                //book.AutorName = "作者：" + jobj["author"].ToString().Replace("\r\n", "").Replace("[", "").Replace("]", "").Replace("\"", "").Trim();
                book.PublishMessage = "出版社：" + jobj["publisher"].ToString();
                //book.Isbn = "ISBN：" + jobj["isbn10"].ToString();
            }
            catch (System.Net.WebException ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageHelper.HideProgressBar();
                });
                book.Summary = "未能找到该书的详细信息";
                MessageHelper.Show("未能找到该书的详细信息");
            }
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("marcNo"))
            {
                book.MarcNo = NavigationContext.QueryString["marcNo"];
                name = NavigationContext.QueryString["name"];
                authorName = NavigationContext.QueryString["author"];
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageHelper.ShowProgressBar();
                });
                client.DownloadStringAsync(new Uri("http://opac.njue.edu.cn/opac/item.php?marc_no=" + book.MarcNo, UriKind.Absolute));
                client.DownloadStringCompleted += client_DownloadStringCompleted;
            }
            else
            {
                MessageHelper.Show("未能找到该书的详细信息");
            }
        }

        private void Image_ImageOpened_1(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageHelper.HideProgressBar();
            });
        }
    }
}