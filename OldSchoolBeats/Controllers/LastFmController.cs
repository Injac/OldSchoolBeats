using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Lastfm;
using Lastfm.Services;

namespace OldSchoolBeats.Controllers {
    /// <summary>
    ///  Utilize lastfm to
    ///  get images from artists.
    /// </summary>
    public class LastFmController : ApiController {
        public ApiServices Services {
            get;
            set;
        }

        // POST api/LastFm
        /// <summary>
        /// Posts the specified artist name.
        /// </summary>
        /// <param name="artistName">Name of the artist.</param>
        /// <returns></returns>
        public string Post(string artistName) {

            var apiKey = Services.Settings["LastFMApiKey"];
            var apiSecret = Services.Settings["LastFMSecret"];
            var userName = Services.Settings["LastFMUserName"];
            var password = Services.Settings["LastFMPassword"];

            //Create new lastfm session
            var session = new Session(apiKey,apiSecret);
            //authenticate
            session.Authenticate(userName,Lastfm.Utilities.md5(password));

            //Get album art
            var artist = new Artist(artistName,session);

            var imageUrl = artist.GetImageURL(ImageSize.Large);

            return imageUrl;

        }

    }
}
