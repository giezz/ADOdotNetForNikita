using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.models
{
    public class Delivery
    {
        [Key] [Column("number_of_delivery")] 
        public int NumberOfDelivery { get; set; }
        
        [Column("price_of_delivery")] 
        public double PriceOfDelivery { get; set; }
        
        [Column("address_of_delivery")] 
        public string AddressOfDelivery { get; set; }
        
        [Column("description_of_delivery")] 
        public string DescriptionOfDelivery { get; set; }
    }
}