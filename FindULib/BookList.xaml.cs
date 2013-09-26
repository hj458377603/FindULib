using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace FindULib
{
    public partial class BookList : PhoneApplicationPage
    {
        string htmlStr = string.Empty;
        HtmlDocument htmlDoc;
        WebClient client;
        ObservableCollection<Book> bookList;
        int count = 0;
        int pageIndex = 1;
        string url = string.Empty;
        string keyWord = "关键词传递失败";
        bool loadFinished = false;
        bool isTap = false;

        public BookList()
        {
            InitializeComponent();
            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            htmlDoc = new HtmlDocument();
            bookList = new ObservableCollection<Book>();
        }

        void DoSearch(string keyWord)
        {
            string searchType = "title";
            if (pageIndex == 1)
            {
                url = "http://opac.njue.edu.cn/opac/openlink.php?strText=" + keyWord + "&strSearchType=" + searchType;
            }
            else
            {
                url = "http://opac.njue.edu.cn/opac/openlink.php?location=ALL&title=" + keyWord + "&doctype=ALL&lang_code=ALL&match_flag=forward&displaypg=20&showmode=list&orderby=DESC&sort=CATA_DATE&onlylendable=no&count=" + count + "&with_ebook=&page=" + pageIndex;
            }
            client.DownloadStringAsync(new Uri(url, UriKind.Absolute));

            if (pageIndex == 1)
            {
                client.DownloadStringCompleted += client_DownloadStringCompleted;
            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            htmlStr = e.Result;
            htmlDoc.LoadHtml(htmlStr);

            if (pageIndex == 1)
            {
                HtmlNode countNode = htmlDoc.GetElementbyId("content");
                if (countNode == null)
                {
                    this.loadingProgress.Visibility = Visibility.Collapsed;
                    this.searchTitle.Text = "没有检索到关键词包含\"" + keyWord + "\"的纸本馆藏书目";
                    MessageBox.Show("没有检索到关键词包含\"" + keyWord + "\"的纸本馆藏书目");
                    return;
                }
                string tempNodeText = countNode.ChildNodes.Where(n => n.Name == "div").First().InnerText;
                int tempCount = tempNodeText.IndexOf(" 条") - tempNodeText.IndexOf("到 ") - 2;
                count = int.Parse(tempNodeText.Substring(tempNodeText.IndexOf("到 ") + 2, tempCount));
            }

            HtmlNode bookListNode = htmlDoc.GetElementbyId("search_book_list");
            if (bookListNode != null)
            {
                HtmlNodeCollection books = bookListNode.SelectNodes("li");

                Book book = null;

                foreach (var item in books)
                {
                    book = GetBook(item);

                    ////获取ISBN
                    //htmlStr = client.DownloadString("http://opac.njue.edu.cn/opac/item.php?marc_no=" + book.MarcNo);
                    //int isbnStartIndex = htmlStr.IndexOf("ajax_douban.php?isbn=") + 21;
                    //int isbnLength = htmlStr.IndexOf("\",function(json)") - isbnStartIndex;
                    //book.Isbn = htmlStr.Substring(isbnStartIndex, isbnLength);

                    //////从豆瓣获取数据设置简介和图片
                    //htmlStr = client.DownloadString("https://api.douban.com/v2/book/isbn/" + book.Isbn);
                    //GetDataFormDouban(book, htmlStr);

                    //获取
                    bookList.Add(book);
                }

            }
            this.loadingProgress.Visibility = Visibility.Collapsed;
            loadFinished = true;
            // 查询第一页时需要
            if (pageIndex == 1)
            {
                this.searchTitle.Text = "共检索到" + count + "本关键词包含\"" + keyWord + "\"的图书";
                this.lbBookList.ItemsSource = bookList;
            }
        }

        /// <summary>
        /// 对书名进行解码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static String Unescape(String src)
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Capacity = src.Length;

            int lastPos = 0, pos = 0;
            char ch;
            while (lastPos < src.Length)
            {
                pos = src.IndexOf("%", lastPos);
                if (pos == lastPos)
                {
                    if (src[pos + 1] == 'u')
                    {
                        ch = (char)Convert.ToInt32(src.Substring(pos + 2, 4), 16);
                        tmp.Append(ch);
                        lastPos = pos + 6;
                    }
                    else
                    {
                        ch = (char)Convert.ToInt32(src.Substring(pos + 1, 2), 16);
                        tmp.Append(ch);
                        lastPos = pos + 3;
                    }
                }
                else
                {
                    if (pos == -1)
                    {
                        lastPos = src.Length;
                    }
                    else
                    {
                        lastPos = pos;
                    }
                }
            }
            return src.Substring(0, src.IndexOf(".") + 1) + tmp.ToString();
        }

        /// <summary>
        /// 获取书的各属性
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Book GetBook(HtmlNode node)
        {
            Book book = new Book();

            // 对节点的无效字符进行过滤
            IEnumerable<HtmlNode> nodes = node.ChildNodes.Where(n => n.Name != "#text");
            // 获取li的子节点
            List<HtmlNode> nodeList = nodes.ToList();

            // 书类型(如中文图书）
            book.BookType = nodeList[0].ChildNodes[0].InnerText;

            string marcNoStr = nodeList[0].ChildNodes[1].GetAttributeValue("href", "");

            // MarcNo
            book.MarcNo = marcNoStr.Substring(marcNoStr.IndexOf("marc_no=") + 8);

            // 书名
            book.Name = Unescape(nodeList[0].ChildNodes[1].InnerText.Replace("&#x", "%u").Replace(";", ""));
            if (book.Name.Length >= 20)
            {
                book.Name = book.Name.Substring(0, 20) + "...";
            }
            // 书图书馆编号
            book.LibNo = Unescape(nodeList[0].ChildNodes[2].InnerText.Replace("&#x", "%u").Replace(";", ""));

            string storeStr = nodeList[1].ChildNodes[1].InnerText;

            // 馆藏复本
            book.StoreCount = Convert.ToInt32(storeStr.Substring(storeStr.IndexOf("馆藏复本：") + 5, 1));
            // 可借复本
            book.AvailableCount = Convert.ToInt32(storeStr.Substring(storeStr.IndexOf("可借复本：") + 5, 1));
            if (book.AvailableCount == 0)
            {
                book.AvailableCountImageUrl = "/none.png";
            }
            // 作者
            book.AutorName = Unescape(nodeList[1].ChildNodes[2].InnerText.Replace("&#x", "%u").Replace(";", ""));
            // 出版信息
            string publishStr = Unescape(nodeList[1].ChildNodes[4].InnerText.Replace("&#x", "%u").Replace(";", ""));
            book.PublishMessage = publishStr;
            return book;
        }

        void GetDataFormDouban(Book book, string doubanJsonStr)
        {
            JObject jobj = (JObject)JsonConvert.DeserializeObject(doubanJsonStr);
            book.ImageUrl = jobj["image"].ToString();
            book.Summary = jobj["summary"].ToString();
        }

        private void lbBookList_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.Keys.Contains("keyWord"))
            {
                keyWord = NavigationContext.QueryString["keyWord"];
                DoSearch(keyWord);
            }
        }

        // 获取子类型
        private T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        private void lbBookList_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isTap)
            {
                isTap = false;
                return;
            }

            // 获取listbox的子类型ScrollViewer
            if (loadFinished)
            {
                ScrollViewer scrollViewer = FindChildOfType<ScrollViewer>(lbBookList);//ScrollViewer  scrollBar
                if (scrollViewer == null)
                {
                    throw new InvalidOperationException("error");
                }
                else
                {
                    //判断当前滚动的高度是否大于或者等于scrollViewer实际可滚动高度，如果等于或者大于就证明到底了
                    if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 0.2)
                    {
                        //处理listbox滚动到底的事情
                        if (pageIndex == count / 20 + 1)
                        {
                            MessageBox.Show("到底了，没有了！");
                        }
                        else
                        {
                            pageIndex++;
                            this.loadingProgress.Visibility = Visibility.Visible;
                            loadFinished = false;
                            DoSearch(keyWord);
                        }
                    }
                }
            }
        }

        private void lbBookList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isTap = true;
        }

        private void lbBookList_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isTap = true;
        }
    }
}