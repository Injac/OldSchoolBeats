using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Description;
using OldSchoolBeats.DataObjects;
using OldSchoolBeats.Models;

namespace OldSchoolBeats.Controllers {
    public class OldSchoolArtistController : TableController<OldSchoolArtist> {
        protected override void Initialize(HttpControllerContext controllerContext) {
            base.Initialize(controllerContext);
            OldSchoolBeatsContext context = new OldSchoolBeatsContext();
            DomainManager = new EntityDomainManager<OldSchoolArtist>(context, Request, Services);

            //This shows how to set the XmlDocumentationProvider to suck your XML comments on
            //Controller actions into your sample page.
            this.Configuration.SetDocumentationProvider(new XmlDocumentationProvider(this.Services));
        }


        /// <summary>
        /// Gets all old school artists.
        /// </summary>
        /// <returns></returns>
        public IQueryable<OldSchoolArtist> GetAllOldSchoolArtist() {
            return Query();
        }


        /// <summary>
        /// Gets the old school artist.
        /// </summary>
        /// <param name="id">The id of the artist we want to retrieve.</param>
        /// <returns></returns>
        public SingleResult<OldSchoolArtist> GetOldSchoolArtist(string id) {
            return Lookup(id);
        }


        /// <summary>
        /// Patches the old school artist.
        /// </summary>
        /// <param name="id">The identifier of the artist to change.</param>
        /// <param name="patch">The patch. This is the object that contains the changes we want to apply.</param>
        /// <returns></returns>
        public Task<OldSchoolArtist> PatchOldSchoolArtist(string id, Delta<OldSchoolArtist> patch) {
            return UpdateAsync(id, patch);
        }


        /// <summary>
        /// Posts the old school artist.
        /// </summary>
        /// <param name="item">The new artist to add.</param>
        /// <returns></returns>
        [ResponseType(typeof(OldSchoolArtist))]
        public async Task<IHttpActionResult> PostOldSchoolArtist(OldSchoolArtist item) {
            OldSchoolArtist current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }


        /// <summary>
        /// Deletes the old school artist.
        /// </summary>
        /// <param name="id">The id of the artist to delete.</param>
        /// <returns></returns>
        public Task DeleteOldSchoolArtist(string id) {
            return DeleteAsync(id);
        }

    }
}