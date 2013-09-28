using FindULib.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FindULib.Common
{
    public class MessageHelper
    {
        private static Toast toast = new Toast();
        private static Popup pop = new Popup();
        private static Popup popProgressBar = new Popup();

        static MessageHelper()
        {
            pop.Child = toast;
            toast.ShowCompleted += new EventHandler(Mbc_ShowCompleted);
        }

        private static void Mbc_ShowCompleted(object sender, EventArgs e)
        {
            pop.IsOpen = false;
        }

        /// <summary>
        ///  信息提示信息
        /// </summary>
        /// <param name="text">提示信息</param>
        public static void Show(string text)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (Application.Current.RootVisual != null)
                {
                    pop.IsOpen = true;
                }
                toast.Show(text);
            });

        }

        /// <summary>
        ///  显示进度度
        /// </summary>
        public static void ShowProgressBar()
        {
            ProgressBarControl pbc = new ProgressBarControl();
            popProgressBar.Child = pbc;
            if (Application.Current.RootVisual != null)
            {
                popProgressBar.IsOpen = true;
            }
        }

        /// <summary>
        ///  隐藏进度条
        /// </summary>
        public static void HideProgressBar()
        {
            if (Application.Current.RootVisual != null)
            {
                popProgressBar.IsOpen = false;
            }
        }
    }
}
