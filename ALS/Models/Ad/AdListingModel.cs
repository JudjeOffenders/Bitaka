namespace ALS.Models.Ad
{
    using ALS.Data;

    public class AdListingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public Pictures MainPicture { get; set; }

        public AdStatus Status { get; set; }

    }
}