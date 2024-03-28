using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using ECommerceBE.Database;

namespace ECommerceBE.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Picture { get; set; }
        public int Inventory { get; set; }

    }



}



