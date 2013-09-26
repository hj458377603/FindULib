using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FindULib
{
    public class Book
    {
        public string MarcNo { get; set; }
        public string LibNo { get; set; }
        public string Isbn { get; set; }
        public string Name { get; set; }
        public string AutorName { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string PublishMessage { get; set; }
        public int StoreCount { get; set; }
        public int AvailableCount { get; set; }
        public string BookType { get; set; }
        public DateTime PublishDate { get; set; }
        public string AvailableCountImageUrl { get; set; }
    }
}
