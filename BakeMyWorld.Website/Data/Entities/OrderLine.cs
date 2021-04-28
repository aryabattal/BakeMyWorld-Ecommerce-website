using System.ComponentModel.DataAnnotations;

namespace BakeMyWorld.Website.Data.Entities
{
    public class OrderLine
    {
        public OrderLine(int id, Cake cake, int quantity)
        {
            Id = id;
            Cake = cake;
            Quantity = quantity;
        }

        public OrderLine(int cakeId, int quantity)
        {
            CakeId = cakeId;
            Quantity = quantity;
        }

        public int Id { get; protected set; }
        public Cake Cake { get; set; }
        public int CakeId { get; set; }

        [Required]
        public int Quantity { get; protected set; }
    }
}