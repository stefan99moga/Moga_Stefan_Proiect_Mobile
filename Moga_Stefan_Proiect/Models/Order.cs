using SQLite;

namespace Moga_Stefan_Proiect.Models
{
    [Table("Order")]
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int OrderNumber { get; set; }
        public string Adress { get; set; }
        public string PaymentMethod { get; set; }
    }
}
