using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XOUND.Models;

namespace XOUND.Controllers
{
    public class SoundAPIController : ApiController
    {
        [HttpPost]
        public string InsertAlbum(Album album)
        {

            return "OK";
        }
    }
}
