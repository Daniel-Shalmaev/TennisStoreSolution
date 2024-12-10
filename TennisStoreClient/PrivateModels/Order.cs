namespace TennisStoreClient.PrivateModels
{
    public class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
        public double SubTotal => Quantity * Price;
        
    }
}
