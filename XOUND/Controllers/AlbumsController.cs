using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XOUND.Models;

namespace XOUND.Controllers
{
    public class AlbumsController : Controller
    {
        //
        // GET: /Albums/
        public ActionResult Index()
        {
            XOUNDContext ctx = new XOUNDContext();
            AlbumViewModel avm = new AlbumViewModel();
            avm.Albums = ctx.Albums.ToList();
            return View(avm);
        }

        public ActionResult Create()
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
	}
}