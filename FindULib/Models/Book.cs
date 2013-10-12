using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FindULib.Models
{
    public class Book : INotifyPropertyChanged
    {
        private string marcNo;
        private string libNo;
        private string isbn;
        private string name;
        private string autorName;
        private string imageUrl;
        private string summary;
        private string publishMessage;
        private int storeCount;
        private int availableCount;
        private string bookType;
        private string availableCountImageUrl;
        private string publishDate;

        public string MarcNo
        {
            get
            {
                return marcNo;
            }
            set
            {
                if (marcNo != value)
                {
                    marcNo = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("MarcNo"));
                    }
                }
            }
        }
        public string LibNo
        {
            get
            {
                return libNo;
            }
            set
            {
                if (libNo != value)
                {
                    libNo = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LibNo"));
                    }
                }
            }
        }
        public string Isbn
        {
            get
            {
                return isbn;
            }
            set
            {
                if (isbn != value)
                {
                    isbn = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Isbn"));
                    }
                }
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }
        public string AutorName
        {
            get
            {
                return autorName;
            }
            set
            {
                if (autorName != value)
                {
                    autorName = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AutorName"));
                    }
                }
            }
        }
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ImageUrl"));
                    }
                }
            }
        }
        public string Summary
        {
            get
            {
                return summary;
            }
            set
            {
                if (summary != value)
                {
                    summary = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Summary"));
                    }
                }
            }
        }
        public string PublishMessage
        {
            get
            {
                return publishMessage;
            }
            set
            {
                if (publishMessage != value)
                {
                    publishMessage = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PublishMessage"));
                    }
                }
            }
        }
        public int StoreCount
        {
            get
            {
                return storeCount;
            }
            set
            {
                if (storeCount != value)
                {
                    storeCount = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("StoreCount"));
                    }
                }
            }
        }
        public int AvailableCount
        {
            get
            {
                return availableCount;
            }
            set
            {
                if (availableCount != value)
                {
                    availableCount = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AvailableCount"));
                    }
                }
            }
        }
        public string BookType
        {
            get
            {
                return bookType;
            }
            set
            {
                if (bookType != value)
                {
                    bookType = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BookType"));
                    }
                }
            }
        }
        public string AvailableCountImageUrl
        {
            get
            {
                return availableCountImageUrl;
            }
            set
            {
                if (availableCountImageUrl != value)
                {
                    availableCountImageUrl = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AvailableCountImageUrl"));
                    }
                }
            }
        }

        public string PublishDate
        {
            get
            {
                return publishDate;
            }
            set
            {
                if (publishDate != value)
                {
                    publishDate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PublishDate"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
