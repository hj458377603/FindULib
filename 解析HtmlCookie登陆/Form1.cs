using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 解析HtmlCookie登陆
{
    public partial class Form1 : Form
    {
        public CookieContainer CC = new CookieContainer();

        string url = "https://passport.cnblogs.com/login.aspx";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = HttpWebRequest.Create("https://passport.cnblogs.com/login.aspx") as HttpWebRequest;
            request.Method = "POST";
            request.Referer = "https://passport.cnblogs.com/login.aspx";
            request.Host = "passport.cnblogs.com";

            string param = "__EVENTTARGET=&__EVENTARGUMENT="
                + "&__VIEWSTATE=/wEPDwULLTE1MzYzODg2NzZkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQtjaGtSZW1lbWJlcm1QYDyKKI9af4b67Mzq2xFaL9Bt"
                + "&__EVENTVALIDATION=/wEdAAUyDI6H/s9f+ZALqNAA4PyUhI6Xi65hwcQ8/QoQCF8JIahXufbhIqPmwKf992GTkd0wq1PKp6+/1yNGng6H71Uxop4oRunf14dz2Zt2+QKDEIYpifFQj3yQiLk3eeHVQqcjiaAP"
                + "&tbUserName=locas&tbPassword=hj1991120&btnLogin=登++录&txtReturnUrl=";

            byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
            request.ContentLength = data.Length;
            request.CookieContainer = CC;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:26.0) Gecko/20100101 Firefox/26.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string res = reader.ReadToEnd();
            this.richTextBox1.Text = res;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            //myRequest.Method = "GET";
            //myRequest.CookieContainer = CC;  //将COOKIE保存在CC
            ////myRequest.Connection = "keep-alive";
            //HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            //Stream vfimg = myResponse.GetResponseStream();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "https://passport.cnblogs.com/login.aspx";
            string indata = "__EVENTTARGET=&__EVENTARGUMENT="
                + "&__VIEWSTATE=/wEPDwULLTE1MzYzODg2NzZkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQtjaGtSZW1lbWJlcm1QYDyKKI9af4b67Mzq2xFaL9Bt"
                + "&__EVENTVALIDATION=/wEdAAUyDI6H/s9f+ZALqNAA4PyUhI6Xi65hwcQ8/QoQCF8JIahXufbhIqPmwKf992GTkd0wq1PKp6+/1yNGng6H71Uxop4oRunf14dz2Zt2+QKDEIYpifFQj3yQiLk3eeHVQqcjiaAP"
                + "&tbUserName=locas&tbPassword=hj1991120&btnLogin=登++录&txtReturnUrl=http://www.cnblogs.com/";

            url = "http://opac.njue.edu.cn/reader/redr_verify.php";
            indata = "number=1120130547&passwd=152150&select=cert_no&returnUrl=";

            url = "http://219.219.191.243:8081/m/reader/login.action;jsessionid=D01D26A71ACF073E5019798AAD14DD28";
            indata = "number=1120130547&passwd=152150&type=1&user_login=登录";
            string outdata = "";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(indata);

            //新建一个HttpWebRequest 
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            //一定要设置ContentType
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;
            myHttpWebRequest.Method = "POST";

            //新建一个CookieContainer来存放Cookie集合 
            CookieContainer myCookieContainer = new CookieContainer();

            //设置HttpWebRequest的CookieContainer为刚才建立的那个myCookieContainer 
            myHttpWebRequest.CookieContainer = myCookieContainer;

            //把数据写入HttpWebRequest的Request流 
            Stream myRequestStream = myHttpWebRequest.GetRequestStream();
            myRequestStream.Write(data, 0, data.Length);
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));

            //关闭打开对象 
            myRequestStream.Flush();
            myRequestStream.Close();

            //新建一个HttpWebResponse 
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            //把数据从HttpWebResponse的Response流中读出 
            Stream myResponseStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            outdata = myStreamReader.ReadToEnd();

            //关闭打开的对象 
            myStreamReader.Close();
            myResponseStream.Close();

            SaveCookie(myHttpWebRequest.CookieContainer);

            ////显示"登录" 
            ////拿到了Cookie，再进行请求就能直接读取到登录后的内容了 
            //myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            ////刚才那个CookieContainer已经存有了Cookie,把它附加到HttpWebRequest中则能直接通过验证 
            //myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            ////读取登陆后的内容
            //myResponseStream = myHttpWebResponse.GetResponseStream();
            //myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            //outdata = myStreamReader.ReadToEnd();

            ////关闭打开的对象 
            //myStreamReader.Close();
            //myResponseStream.Close();

            //Console.WriteLine(outdata);
            //this.webBrowser1.DocumentText = outdata;
        }


        private void SaveCookie(object obj)
        {
            FileStream fileStream = File.Create("F://test.txt");
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();

            using (fileStream)
            {
                //这里就是进行序列化了
                formatter.Serialize(fileStream, obj);
            }
        }

        private T LoadCookie<T>() where T : class
        {
            System.Runtime.Serialization.IFormatter formatterTo = new BinaryFormatter();

            //打开一个文件流
            Stream stream = new FileStream("F://test.txt", FileMode.Open, FileAccess.Read);

            using (stream)
            {
                //这里就是反进行序列化了
                return (formatterTo.Deserialize(stream) as T);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = "http://219.219.191.243:8081/m/reader/login.action;jsessionid=D01D26A71ACF073E5019798AAD14DD28";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            //一定要设置ContentType
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.Method = "POST";

            myHttpWebRequest.CookieContainer = LoadCookie<CookieContainer>();
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream myResponseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string outdata = myStreamReader.ReadToEnd();

            //关闭打开的对象 
            myStreamReader.Close();
            myResponseStream.Close();

            Console.WriteLine(outdata);
            this.webBrowser1.DocumentText = outdata;
        }

        private void Go(string str)
        {
            string url = string.Empty;

            switch (str)
            {
                case "1":
                    {
                        url = "http://219.219.191.243:8081/m/reader/info.action;jsessionid=D01D26A71ACF073E5019798AAD14DD28";
                        break;
                    }
                case "2":
                    {
                        url = "http://219.219.191.243:8081/m/reader/lend_list.action;jsessionid=D01D26A71ACF073E5019798AAD14DD28";
                        break;
                    }
            }
            GetInfo(url);
        }

        private void GetInfo(string url)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            //一定要设置ContentType
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.Method = "POST";

            myHttpWebRequest.CookieContainer = LoadCookie<CookieContainer>();
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream myResponseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string outdata = myStreamReader.ReadToEnd();

            //关闭打开的对象 
            myStreamReader.Close();
            myResponseStream.Close();

            Console.WriteLine(outdata);
            this.webBrowser1.DocumentText = outdata;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Go(this.textBox2.Text);
        }
    }
}
