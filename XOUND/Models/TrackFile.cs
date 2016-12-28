using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOUND.Models
{
    public class TrackFile : ModelBase
    {
        public int ID { get; set; }
        public int TrackID { get; set; }
        public byte[] Track { get; set; }
    }
}