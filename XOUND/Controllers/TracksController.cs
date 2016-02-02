using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NAudio;
using NAudio.Wave;

namespace XOUND.Controllers
{
    public class TracksController : Controller
    {
        public ActionResult Create()
        {
            if (Session["albums"] != null)
                ViewBag.Albums = Session["albums"];
            else
            {
                XOUNDContext ctx = new XOUNDContext();
                var items = ctx.Albums.ToList().Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.ID.ToString()
                });
                ViewBag.Albums = items;
                Session["albums"] = items;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Track track)
        {
            XOUNDContext ctx = new XOUNDContext();
            SaveTrack(track, ctx);

            if(Session["albums"] != null)
                ViewBag.Albums = Session["albums"];
            else
            {
                var items = ctx.Albums.ToList().Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.ID.ToString()
                });
                ViewBag.Albums = items;
                Session["albums"] = items;
            }
            return View();
        }

        public ActionResult CreateMulti(int id)
        {
            XOUNDContext ctx = new XOUNDContext();
            var items = ctx.Albums.Where(x => x.ID == id).ToList().Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.ID.ToString()
            });
            ViewBag.Albums = items;
            Session["albums"] = items;
            return View();
        }

        [HttpPost]
        public ActionResult CreateMulti(int id, Models.MultiTrackViewModel multi)
        {
            XOUNDContext ctx = new XOUNDContext();
            foreach(var tr in multi.Tracks.Where(x => x.Title != null))
            {
                SaveTrack(tr, ctx);
            }

            var items = ctx.Albums.Where(x => x.ID == id).ToList().Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.ID.ToString()
            });
            ViewBag.Albums = items;
            Session["albums"] = items;
            return View();
        }

        protected bool SaveTrack(Models.Track track, XOUNDContext ctx)
        {
            Mp3FileReader reader = new Mp3FileReader(track.AudioFile.InputStream);

            track.DurationMinutes = reader.TotalTime.Minutes;
            track.DurationSeconds = reader.TotalTime.Seconds;

            ctx.Tracks.Add(track);
            ctx.SaveChanges();

            byte[] fileData = null;
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
	}
}