using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using XOUND.Models;

namespace XOUND.Controllers
{
    public class SoundAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAudioFile(int trackID)
        {
            XOUNDContext ctx = new XOUNDContext();
            var track = ctx.TrackFiles.Where(x => x.TrackID == trackID).FirstOrDefault();

            MemoryStream ms = new MemoryStream(track.Track);

            HttpResponseMessage Response = new HttpResponseMessage();
            Response.Content = new StreamContent(ms);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mp3");
            Response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            Response.Content.Headers.ContentDisposition.FileName = "play.mp3";
            return Response;
        }
    }
}
