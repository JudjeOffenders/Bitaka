namespace ALS.Models.Ad
{
    using ALS.Data;
    using System.Collections.Generic;

    public class AdDeletingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public List<Pictures> Pictures { get; set; }

        //public Pictures MainPicture { get; set; }
    }
}