using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XOUND.Models;

namespace XOUND.Controllers
{
    public class AlbumsController : Controller
    {
        public ActionResult Index()
        {
            XOUNDContext ctx = new XOUNDContext();
            AlbumViewModel avm = new AlbumViewModel();
            avm.Albums = ctx.Albums.OrderBy(x => x.Title).ToList();

            foreach(Album album in avm.Albums)
            {
                album.Tracks = ctx.Tracks.Where(x => x.AlbumID == album.ID).ToList();
            }

            return View(avm);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateByFiles()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Album album)
        {
            XOUNDContext ctx = new XOUNDContext();

            Bitmap bmp = new Bitmap(1, 1);

            System.Net.WebRequest request =
            System.Net.WebRequest.Create(album.ArtworkImageURL);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            Bitmap orig = new Bitmap(responseStream);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(orig, new Rectangle(0, 0, 1, 1));
            }
            Color pixel = bmp.GetPixel(0, 0);
            string avg = System.Drawing.ColorTranslator.ToHtml(pixel);

            album.ArtworkDominantColor = avg;
            //Color backgroundColor = (Color)value;

            var blackContrast = LuminosityContrast(Color.Black, pixel);
            var whiteContrast = LuminosityContrast(Color.White, pixel);

            album.ArtworkContrastColor = blackContrast >= whiteContrast ? ColorTranslator.ToHtml(Color.Black) : ColorTranslator.ToHtml(Color.White);
            album.InsertDate = DateTime.Now;
            album.Active = true;

            ctx.Albums.Add(album);
            ctx.SaveChanges();

            return View();
        }

        [HttpPost]
        public ActionResult CreateByFiles(Album album, IEnumerable<HttpPostedFileBase> files)
        {
            album.TrackCount = files.Count();
            XOUNDContext ctx = new XOUNDContext();

            Bitmap bmp = new Bitmap(1, 1);

            System.Net.WebRequest request =
            System.Net.WebRequest.Create(album.ArtworkImageURL);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            Bitmap orig = new Bitmap(responseStream);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(orig, new Rectangle(0, 0, 1, 1));
            }
            Color pixel = bmp.GetPixel(0, 0);
            string avg = System.Drawing.ColorTranslator.ToHtml(pixel);

            album.ArtworkDominantColor = avg;
            //Color backgroundColor = (Color)value;

            var blackContrast = LuminosityContrast(Color.Black, pixel);
            var whiteContrast = LuminosityContrast(Color.White, pixel);

            album.ArtworkContrastColor = blackContrast >= whiteContrast ? ColorTranslator.ToHtml(Color.Black) : ColorTranslator.ToHtml(Color.White);
            album.InsertDate = DateTime.Now;
            album.Active = true;

            ctx.Albums.Add(album);
            ctx.SaveChanges();
            int ctr = 1;

            foreach(var file in files)
            {
                Track tr = new Track()
                {
                    Title = file.FileName.Replace(".mp3", ""),
                    AudioFile = file,
                    TrackNo = ctr,
                    AlbumID = album.ID
                };
                SaveTrack(tr, ctx);
                ctr++;
            }

            return View();
        }

        private double LuminosityContrast(Color foreground, Color background)
        {
            var foregroundLuminosity = RelativeLuminance(foreground.R, foreground.G, foreground.B);
            var backgroundLuminosity = RelativeLuminance(background.R, background.G, background.B);

            if (foregroundLuminosity > backgroundLuminosity)
            {
                return (foregroundLuminosity + 0.05) / (backgroundLuminosity + 0.05);
            }
            else
            {
                return (backgroundLuminosity + 0.05) / (foregroundLuminosity + 0.05);
            }
        }

        private double RelativeLuminance(byte r, byte g, byte b)
        {
            double rs = ((double)r / 255);
            double gs = ((double)g / 255);
            double bs = ((double)b / 255);
            double R = 0;
            double G = 0;
            double B = 0;

            R = (rs <= 0.03928) ? (double)(rs / 12.92) : Math.Pow(((rs + 0.055) / 1.055), 2.4);
            G = (gs <= 0.03928) ? (double)(gs / 12.92) : Math.Pow(((gs + 0.055) / 1.055), 2.4);
            B = (bs <= 0.03928) ? (double)(bs / 12.92) : Math.Pow(((bs + 0.055) / 1.055), 2.4);

            return ((double)(0.2126 * R)) + ((double)(0.7152 * G)) + ((double)(0.0722 * B));
        }

        protected bool SaveTrack(Models.Track track, XOUNDContext ctx)
        {
            byte[] fileData = null;
            //MemoryStream ms = new MemoryStream();
            //track.AudioFile.InputStream.CopyTo(ms);

            Mp3FileReader reader = new Mp3FileReader(track.AudioFile.InputStream);
            //byte trackNo = GetTrackNo(track.AudioFile.InputStream);
            
            track.DurationMinutes = reader.TotalTime.Minutes;
            track.DurationSeconds = reader.TotalTime.Seconds;

            ctx.Tracks.Add(track);
            ctx.SaveChanges();
            
            using (var binaryReader = new BinaryReader(track.AudioFile.InputStream))
            {
                fileData = binaryReader.ReadBytes(track.AudioFile.ContentLength);
            }

            Models.TrackFile file = new Models.TrackFile()
            {
                TrackID = track.ID,
                Track = fileData
            };

            ctx.TrackFiles.Add(file);
            ctx.SaveChanges();

            return true;
        }

        public byte GetTrackNo(Stream s)
        {
            s.Seek(-95, SeekOrigin.End);
            //…
            s.Seek(-2, SeekOrigin.End);
            return Convert.ToByte(s.ReadByte());
        }
    }
}