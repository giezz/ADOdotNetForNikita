using System.Data.Entity;
using Entity.models;

namespace Entity
{
    public class DeliveryContext
    {
        public DbSet<Delivery> Deliveries { get; set; }
        
        public DeliveryContext() : base(nameOrConnectionString: "PGConnectionString")
}