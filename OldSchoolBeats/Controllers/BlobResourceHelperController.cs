using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using OldSchoolBeats.Models;

namespace OldSchoolBeats.Controllers {

    /// <summary>
    /// Helps us to manage blob-resources.
    /// </summary>
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class BlobResourceHelperController : ApiController {

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public ApiServices Services {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the storage account.
        /// </summary>
        /// <value>
        /// The storage account.
        /// </value>
        private CloudStorageAccount storageAccount {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the BLOB client.
        /// </summary>
        /// <value>
        /// The BLOB client.
        /// </value>
        private CloudBlobClient blobClient {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        private CloudBlobContainer container {
            get;
            set;
        }

        /// <summary>
        /// Gets all blobs in container.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/getallblobs")]
        public async Task<IEnumerable<CloudBlockBlob>> GetAllBlobsInContainer(BlobManipulationData data) {

            await InitBlobContainer(data);

            var blobs = container.ListBlobs().OfType<CloudBlockBlob>();

            return blobs;

        }


        /// <summary>
        /// Pages the blobs in container.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/pageblobs")]
        public async Task<IEnumerable<CloudBlockBlob>> PageBlobsInContainer(BlobManipulationData data) {

            await InitBlobContainer(data);

            var blobs = container.ListBlobs().OfType<CloudBlockBlob>().Skip(data.Skip).Take(data.Take);

            return blobs;
        }

        /// <summary>
        /// Gets the BLOB count.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/getblobcount")]
        public async Task<int> GetBlobCount(BlobManipulationData data) {

            return await Task.Run<int>(async () => {

                await InitBlobContainer(data);

                //Lazy. Therefore convert that thing to a list.
                return container.ListBlobs().ToList().Count;

            });

        }


        /// <summary>
        /// Uploads the BLOB.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadblob")]
        public async Task UploadBlob(BlobManipulationData data) {

            await InitBlobContainer(data);

            var blob = container.GetBlockBlobReference(data.BlobName);

            await blob.UploadFromByteArrayAsync(data.BlobData, 0, data.BlobData.Length);


        }

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/deleteblob")]
        public async Task DeleteBlob(BlobManipulationData data) {

            await InitBlobContainer(data);

            var blob = container.GetBlockBlobReference(data.BlobName);

            await blob.DeleteIfExistsAsync();

        }


        /// <summary>
        /// Renames the BLOB.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/renameblob")]
        public async Task RenameBlob(BlobManipulationData data) {

            await InitBlobContainer(data);

            var blob = container.GetBlockBlobReference(data.BlobName);

            var exists = await blob.ExistsAsync();


            if (exists) {

                var renameTo = container.GetBlockBlobReference(data.NewBlobName);

                //copy the blob to rename to the new one
                await renameTo.StartCopyFromBlobAsync(blob);

                //delete the existing blob
                await blob.DeleteAsync();

            }
        }

        /// <summary>
        /// Initializes the BLOB container.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private async Task InitBlobContainer(BlobManipulationData data) {

            var storageConnection = Services.Settings["StorageConnectionString"];

            storageAccount = CloudStorageAccount.Parse(storageConnection);

            blobClient = storageAccount.CreateCloudBlobClient();

            container = blobClient.GetContainerReference(data.ContainerName);

            await container.CreateIfNotExistsAsync();
        }


        /// <summary>
        /// Downloads the BLOB using sas resource URL.
        /// </summary>
        /// <param name="sasUrl">The sas URL.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/dowloadblobsas")]
        public async Task<byte[]> DownloadBlobUsingSASResourceUrl(string sasUrl) {

            HttpClient cl = new HttpClient();

            var blobBytes = await cl.GetByteArrayAsync(sasUrl);

            return blobBytes;
        }

    }
}
