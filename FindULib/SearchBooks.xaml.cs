using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FindULib
{
    public partial class SearchBooks : PhoneApplicationPage
    {
        public SearchBooks()
        {
            InitializeComponent();
            
            List<Book> list = new List<Book>();
            
        }
    }
}