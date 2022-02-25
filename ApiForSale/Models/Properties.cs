using System.ComponentModel.DataAnnotations.Schema;


namespace ApiForSale.Models
{
    public class Properties
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double LandSize { get; set; }
        public double ConstructionSize { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public Boolean KitchenAndLiving { get; set; }
        public double Price { get; set; }
        public int Bedrooms { get; set; }
        public Boolean HasPool { get; set; }
        public string? Details { get; set; }
        public byte[]? Photo1 { get; set; }
        public byte[]? Photo2 { get; set; }
        public byte[]? Photo3 { get; set; }
        public byte[]? Photo4 { get; set; }
        public byte[]? Photo5 { get; set; }
        public byte[]? Photo6 { get; set; }
        public byte[]? Photo7 { get; set; }
        public byte[]? Photo8 { get; set; }
        public string City { get; set; }
        public int Parkings { get; set; }
        public Boolean Sold { get; set; }
        public int UserId { get; set; }
        public int Bathrooms { get; set; }
    }
}
