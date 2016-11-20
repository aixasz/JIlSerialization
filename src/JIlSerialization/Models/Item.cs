using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JIlSerialization.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public int Size { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
