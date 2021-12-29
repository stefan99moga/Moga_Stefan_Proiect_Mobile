using SQLite;

namespace Moga_Stefan_Proiect.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int OrderNumber { get; set; }
        public string Adress { get; set; }
        public string PaymentMethod { get; set; }
        public double CoordonateLat { get; set; }
        public double CoordonateLogi { get; set; }
    }
}
