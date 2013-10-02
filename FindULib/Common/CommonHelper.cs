using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml;

namespace FindULib.Common
{
    public class CommonHelper
    {
        public static void CheckVersion()
        {
            WebClient client = new WebClient();
            client.DownloadStringAsync(new Uri("http://files.cnblogs.com/fb-boy/config.xml", UriKind.Absolute));
            client.DownloadStringCompleted += client_DownloadStringCompleted; ;
        }

        static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string xmlString = e.Result;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(e.Result);
            MemoryStream stream = new MemoryStream(buffer);
            XmlReader reader = XmlReader.Create(stream);
            string version = string.Empty;
            string description = string.Empty;
            string contentIdentifier = string.Empty;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if ("version" == reader.LocalName)
                    {
                        version = reader.ReadInnerXml();
                    }
                    else if ("description" == reader.LocalName)
                    {
                        description = reader.ReadInnerXml();
                    }
                    else if ("contentIdentifier" == reader.LocalName)
                    {
                        contentIdentifier = reader.ReadInnerXml();
                    }
                }
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBoxResult result = MessageBox.Show("发现新版本", "更新", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    MarketplaceDetailTask task = new MarketplaceDetailTask();
                    task.ContentIdentifier = "068822b5-cea6-4ea3-afbc-e867887e4cfa";
                    task.Show();
                }
                //MessageHelper.Show("description:" + description);
            });
        }
    }
}
