using ALS.Data;
using System;
using System.Collections.Generic;
namespace ALS.Models.Ad
{
    using ALS.Data;

    public class AdStatusListModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public DateTime DateAdded { get; set; }

        public AdStatus Status { get; set; }

    }
}