using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.ResourceBroker;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace OldSchoolBeats.Controllers {



    /// <summary>
    /// Used to manage resources using SAS-Tokens.
    /// https://github.com/Azure/azure-mobile-services-resourcebroker
    /// </summary>
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class ResourcesController : ResourcesControllerBase {

        [HttpGet]
        public async Task ListBlobsInContainer(string container) {

        }


    }
}
