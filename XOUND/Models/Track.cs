using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace XOUND.Models
{
    public class Track : ModelBase
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? TrackNo { get; set; }
        public int DurationMinutes { get; set; }
        public int DurationSeconds { get; set; }
        public int AlbumID { get; set; }

        [NotMapped]
        public HttpPostedFileBase AudioFile { get; set; }
    }
}