using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using OldSchoolBeats.Models;
using OldSchoolBeats.DataObjects;
using System.Xml;
using System.Text;

namespace OldSchoolBeats.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class OldSchoolFeedsController : ApiController
    {
        public ApiServices Services { get; set; }

        private ServiceUser currentMobileServicesUser;

        // GET api/OldSchoolFeeds
        /// <summary>
        /// Returns RSS20 compatible feed from our
        /// artist entries in the database.
        /// If there are no entries, only the base
        /// data of the feed will be delivered.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {

            if (this.User.Identity.IsAuthenticated)
            {
                this.currentMobileServicesUser = (ServiceUser)this.User;
            }


            //Creating the main-feed thing

            SyndicationFeed feed =
                new SyndicationFeed("Old School Feed", "The coolest old-school rappers.",
                    new Uri("http://www.allmusic.com/subgenre/old-school-rap-ma0000002762/albums"), "OLDSFEED", DateTime.Now);

            feed.Language = "en-US";
            

            List<OldSchoolArtist> artists = new List<OldSchoolArtist>();
            List<SyndicationItem> feedItems = new List<SyndicationItem>();

            //Getting the data from the db
            using (var db = new OldSchoolBeatsContext())
            {

                //Selecting the first 50 entries
                artists = db.OldSchoolArtists.OrderBy(a=>a.CreatedAt).Take(50).ToList();

            }


            foreach (var artist in artists)
            {
                var rssEntry = new SyndicationItem()
                {
                    Title = new TextSyndicationContent(artist.Artist, TextSyndicationContentKind.Plaintext),
                    Id = new Guid().ToString(),
                    PublishDate = DateTime.Now,
                    Summary = new TextSyndicationContent(

                        string.Format("{0} was active between {1} and was added to our database {2}.", artist.Artist, artist.YearsArchive, artist.CreatedAt),
                        TextSyndicationContentKind.Plaintext

                        ),
                    Copyright = new TextSyndicationContent("ALLMUSIC.COM", TextSyndicationContentKind.Plaintext)


                };

                feedItems.Add(rssEntry);

            }

            //Write our feed to a string builder
            //using a XML-Writer instance
            var sb = new StringBuilder();

            XmlWriter w = XmlWriter.Create(sb);

            feed.SaveAsRss20(w);

            w.Flush();
            w.Close();

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            //The standard encoding for the feed is UTF-16, which is Unicode, little-endian encoding
            //And the media-type is "application/rss+xml"
            response.Content = new StringContent(sb.ToString(), Encoding.Unicode, "application/rss+xml");

            return response;
        }
    }
}
