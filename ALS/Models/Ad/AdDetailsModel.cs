namespace ALS.Models.Ad
{
    using ALS.Data;
    using System;
    using System.Collections.Generic;

    public class AdDetailsModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public Cities City { get; set; }

        public string Content { get; set; }

        public decimal Price { get; set; }

        public DateTime DateAdded { get; set; }

        public List<Pictures> Pictures { get; set; }

        public string Contact { get; set; }

        public string UserName { get; set; }

        public bool IsAuthor(string name)
        {
            return this.UserName.Equals(name);
        }
    }
}