using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OldSchoolBeats.ClientModel {
    /// <summary>
    /// The model that we use to
    /// work with BlockBlob data
    /// </summary>
    public class BlobManipulationData {
        [JsonProperty(DefaultValueHandling=DefaultValueHandling.Ignore,PropertyName="blobName")]
        public string BlobName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        /// <value>
        /// The name of the container.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore,PropertyName="containerName")]
        public string ContainerName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the new name of the BLOB.
        /// </summary>
        /// <value>
        /// The new name of the BLOB.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore,PropertyName="newBlobName")]
        public string NewBlobName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the BLOB data.
        /// </summary>
        /// <value>
        /// The BLOB data.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore,PropertyName="blobData")]
        public byte[] BlobData {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the skip.
        /// </summary>
        /// <value>
        /// The skip.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include,PropertyName="skip")]
        public int Skip {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        /// <value>
        /// The take.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore,PropertyName="take")]
        public int Take {
            get;
            set;
        }
    }
}