using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace OldSchoolBeats.SignalR {
    /// <summary>
    /// This is where we save our users permanently
    /// during SignalR sessions.
    /// </summary>
    public class SignalRUserStore:TableEntity {

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRUserStore"/> class.
        /// </summary>
        public SignalRUserStore() {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRUserStore"/> class.
        /// </summary>
        /// <param name="mobileServiceUserId">The mobile service user identifier.</param>
        /// <param name="userAlias">The user alias.</param>
        /// <param name="signalRConnectionId">The signal r connection identifier.</param>
        public SignalRUserStore(string mobileServiceUserId,string signalRConnectionId) {
            this.PartitionKey = mobileServiceUserId;
            this.RowKey = signalRConnectionId;
        }

    }
}