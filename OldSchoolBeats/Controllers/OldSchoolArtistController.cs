using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using OldSchoolBeats.DataObjects;
using OldSchoolBeats.Models;

namespace OldSchoolBeats.Controllers
{
    public class OldSchoolArtistController : TableController<OldSchoolArtist>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            OldSchoolBeatsContext context = new OldSchoolBeatsContext();
            DomainManager = new EntityDomainManager<OldSchoolArtist>(context, Request, Services);
        }

        // GET tables/OldSchoolArtist
        public IQueryable<OldSchoolArtist> GetAllOldSchoolArtist()
        {
            return Query(); 
        }

        // GET tables/OldSchoolArtist/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<OldSchoolArtist> GetOldSchoolArtist(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/OldSchoolArtist/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<OldSchoolArtist> PatchOldSchoolArtist(string id, Delta<OldSchoolArtist> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/OldSchoolArtist/48D68C86-6EA6-4C25-AA33-223FC9A27959
        /// <summary>
        /// Posts the old school artist.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<IHttpActionResult> PostOldSchoolArtist(OldSchoolArtist item)
        {
            OldSchoolArtist current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/OldSchoolArtist/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteOldSchoolArtist(string id)
        {
             return DeleteAsync(id);
        }

    }
}