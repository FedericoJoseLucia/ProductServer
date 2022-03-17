using ProductServer.Domain.SeedWork;

namespace ProductServer.Domain.Aggregates.Product
{
    public class ProductState : Enumeration
    {
        private ProductState(int id, string name) : base(id, name) { }

        public static ProductState Enabled => new(1, "Enabled");
        public static ProductState Suspended => new(2, "Suspended");
        public static ProductState Disabled => new(3, "Disabled");

        public static ProductState GetById(int id) => id switch 
        { 
            1 => Enabled, 
            2 => Suspended,
            3 => Disabled,
            _ => throw new ArgumentOutOfRangeException(string.Format(Resources.Domain.ProductStateNotFound, id))
        };
    }
}
