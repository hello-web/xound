using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOUND.Models
{
    public class Artist
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArtistImageURL { get; set; }
        public int Year { get; set; }
        public List<Album> Albums { get; set; }
        public List<string> Tags { get; set; }
        public string Genre { get; set; }
    }
}