using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOUND.Models
{
    public class ModelBase
    {
        public bool Active { get; set; }
        public DateTime InsertDate { get; set; }

        public ModelBase()
        {
            this.Active = true;
            this.InsertDate = DateTime.Now;
        }
    }
}