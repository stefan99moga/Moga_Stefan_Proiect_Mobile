using SQLite;

namespace Moga_Stefan_Proiect.Models
{
    [Table("OrderLocations")]
    public class OrderLocations
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int OrderNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
