using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOUND.Models
{
    public class Album
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string ArtworkImageURL { get; set; }
        public string ArtworkDominantColor { get; set; }
        public string ArtworkContrastColor { get; set; }
        public int TrackCount { get; set; }
        public bool Active { get; set; }
        public DateTime InsertDate { get; set; }
        public string Genre { get; set; }
    }
}