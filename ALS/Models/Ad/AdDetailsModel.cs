using ALS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALS.Models.Ad
{
    public class AdDetailsModel
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public Cities City { get; set; }

        public string Content { get; set; }

        public decimal Price { get; set; }

        public DateTime DateAdded { get; set; }

        public List<Pictures> Pictures { get; set; }

        public string Contact { get; set; }
    }
}