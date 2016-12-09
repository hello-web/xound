using System;

namespace XOUND.Models
{
    public class ModelBase
    {
        public int ID { get; set; }
        public DateTime InsertDate { get; set; }
        public bool Active { get; set; }
    }
}