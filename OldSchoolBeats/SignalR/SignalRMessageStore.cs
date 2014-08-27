using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace OldSchoolBeats.SignalR {
    /// <summary>
    /// This is the Azure table
    /// we use to store the data
    /// sent over the SignalR Pipeline.
    /// </summary>
    public class SignalRMessageStore:TableEntity {

        /// <summary>
        /// We will save JSON data here.
        /// Simple as that.
        /// </summary>
        /// <value>
        /// The message to store.
        /// </value>
        public string MessageToStore {
            get;
            set;
        }


        public SignalRMessageStore(string mobileServicesUserId) {

            if(string.IsNullOrEmpty(mobileServicesUserId) || string.IsNullOrWhiteSpace(mobileServicesUserId)) {
                throw new ArgumentException("Parameter cannot be null, empty or whitespace.","mobileServicesUserId");
            }

            this.PartitionKey = mobileServicesUserId;
            this.RowKey = DateTime.UtcNow.Ticks.ToString("d19");
        }

    }
}