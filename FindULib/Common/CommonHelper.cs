using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace FindULib.Common
{
    public class CommonHelper
    {
        private static bool showMessage = false;

        #region 检查版本更新
        /// <summary>
        /// 检查版本更新
        /// </summary>
        public static void CheckVersion(bool showMessage = false)
        {
            CommonHelper.showMessage = showMessage;
            // webClient下载xml文件
            WebClient client = new WebClient();
            client.DownloadStringAsync(new Uri(AppConfig.VERSION_CONFIG_URL, UriKind.Absolute));
            client.DownloadStringCompleted += client_DownloadStringCompleted;
        }

        static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // 读取xml
                string xmlString = e.Result;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(e.Result);
                MemoryStream stream = new MemoryStream(buffer);
                XmlReader reader = XmlReader.Create(stream);

                // 定义变量
                string version = string.Empty;
                string description = string.Empty;
                string contentIdentifier = string.Empty;
                string releaseDate = string.Empty;

                // 分析xml
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "version")
                        {
                            version = reader.ReadInnerXml();
                        }
                        else if (reader.LocalName == "description")
                        {
                            description = reader.ReadInnerXml();
                        }
                        else if (reader.LocalName == "contentIdentifier")
                        {
                            contentIdentifier = reader.ReadInnerXml();
                        }
                        else if (reader.LocalName == "releaseDate")
                        {
                            releaseDate = reader.ReadInnerXml();
                        }
                    }
                }

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // 发现新版本
                    if (version.CompareTo(AppConfig.VERSION) > 0)
                    {
                        MessageBoxResult result = MessageBox.Show("发现新版本", "更新", MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            MarketplaceDetailTask task = new MarketplaceDetailTask();
                            task.ContentIdentifier = contentIdentifier;
                            task.Show();
                        }
                    }
                    else if (showMessage)
                    {
                        MessageHelper.Show("已经是最新版本");
                    }
                });

            }
            catch (WebException webException)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageHelper.Show("网络错误");
                });
            }
            catch (Exception exception)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageHelper.Show("未知错误");
                });
            }
        }

        #endregion

        #region 独立存储操作

        /// <summary>
        /// 载入Xml文件
        /// </summary>
        /// <returns></returns>
        private static XDocument LodXml()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            if (!store.DirectoryExists(AppConfig.STORE_DIRECTORY_NAME))
            {
                store.CreateDirectory(AppConfig.STORE_DIRECTORY_NAME);
            }
            IsolatedStorageFileStream storeStream = new IsolatedStorageFileStream(AppConfig.STORE_DIRECTORY_NAME + "\\" + AppConfig.STORE_FILE_NAME, FileMode.OpenOrCreate, store);
            StreamReader sr = new StreamReader(storeStream);
            string xml = sr.ReadToEnd();
            sr.Close();
            storeStream.Close();

            XDocument doc;
            if (string.IsNullOrEmpty(xml))
            {
                doc = new XDocument();
            }
            else
            {
                doc = XDocument.Parse(xml);
            }
            return doc;
        }

        /// <summary>
        /// 保存属性值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SaveProperty(SettingKey key, string value)
        {
            try
            {
                XDocument doc = LodXml();

                if (doc.Element("Property") != null)
                {
                    foreach (XElement element in doc.Descendants("Property"))
                    {
                        if (element.Element(key.ToString()) != null)
                        {
                            element.Element(key.ToString()).Value = value;
                            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                            IsolatedStorageFileStream storeStream = new IsolatedStorageFileStream(AppConfig.STORE_DIRECTORY_NAME + "\\" + AppConfig.STORE_FILE_NAME, FileMode.Truncate, store);
                            doc.Save(storeStream);
                            storeStream.Close();
                            return true;
                        }
                    }
                }
                else
                {
                    XElement elent = new XElement("Property");
                    doc.Add(elent);
                }

                XElement ent = new XElement(key.ToString(), value);
                doc.Element("Property").Add(ent);
                IsolatedStorageFile store1 = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream storeStream1 = new IsolatedStorageFileStream(AppConfig.STORE_DIRECTORY_NAME + "\\" + AppConfig.STORE_FILE_NAME, FileMode.Truncate, store1);
                doc.Save(storeStream1);
                storeStream1.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 保存属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveProperty<T>(SettingKey key, T value) where T : class
        {
            SaveProperty(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProperty(SettingKey key)
        {
            try
            {
                XDocument doc = LodXml();
                if (doc.Element("Property") != null)
                {
                    foreach (XElement element in doc.Descendants("Property"))
                    {
                        if (element.Element(key.ToString()) != null)
                        {
                            return element.Element(key.ToString()).Value;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetProperty<T>(SettingKey key) where T : class
        {
            string str = GetProperty(key);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            T t = JsonConvert.DeserializeObject<T>(str);
            return t;
        }

        /// <summary>
        /// 移除属性值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveProperty(SettingKey key)
        {
            try
            {
                XDocument doc = LodXml();
                if (doc.Element("Property") != null)
                {
                    foreach (XElement element in doc.Descendants("Property"))
                    {
                        if (element.Element(key.ToString()) != null)
                        {
                            element.Element(key.ToString()).Remove();
                            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                            IsolatedStorageFileStream storeStream = new IsolatedStorageFileStream(AppConfig.STORE_DIRECTORY_NAME + "\\" + AppConfig.STORE_FILE_NAME, FileMode.Truncate, store);
                            doc.Save(storeStream);
                            storeStream.Close();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setKey"></param>
        /// <param name="t"></param>
        public static void SaveStorage<T>(SettingKey setKey, T t) where T : class
        {
            IsolatedStorageSettings.ApplicationSettings[setKey.ToString()] = t;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// 取得值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setKey"></param>
        /// <returns></returns>
        public static T GetStorage<T>(SettingKey setKey) where T : class
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(setKey.ToString()))
                return IsolatedStorageSettings.ApplicationSettings[setKey.ToString()] as T;
            else
                return default(T);
        }

        public static void SaveImageFile(BitmapImage bitmapImage, string imageFileName)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isf.DirectoryExists(AppConfig.STORE_IMAGE_DIRECTORY_NAME))
            {
                isf.CreateDirectory(AppConfig.STORE_IMAGE_DIRECTORY_NAME);
            }
            IsolatedStorageFileStream storeStream = new IsolatedStorageFileStream(AppConfig.STORE_IMAGE_DIRECTORY_NAME + "\\" + imageFileName, FileMode.OpenOrCreate, isf);

            BitmapSource bitmapSource = bitmapImage as BitmapSource;
            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);

            writeableBitmap.SaveJpeg(storeStream, bitmapSource.PixelWidth, bitmapSource.PixelHeight, 0, 100);

            storeStream.Flush();
            storeStream.Close();
        }
        #endregion
    }
}
