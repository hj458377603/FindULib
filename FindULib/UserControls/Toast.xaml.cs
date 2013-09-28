using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;

namespace FindULib.UserControls
{
    public partial class Toast : UserControl
    {
        public event EventHandler ShowCompleted;

        public Toast()
        {
            InitializeComponent();
        }

        public void Show(string text)
        {
            this.txtMessage.Text = text;
            Storyboard storyBoard = (Storyboard)this.Resources["storyBoardWithHold"];
            storyBoard.Begin();
            storyBoard.Completed += new EventHandler(StoryBoard_Completed);
        }

        private void StoryBoard_Completed(object sender, EventArgs e)
        {
            ShowCompleted(sender, e);
        }
    }
}
