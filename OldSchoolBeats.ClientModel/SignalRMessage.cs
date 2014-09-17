using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OldSchoolBeats.ClientModel {
    /// <summary>
    /// The message to be serializd
    /// and be sent out via SignalR.
    /// </summary>
    public class SignalRMessage {


        /// <summary>
        /// From user.
        /// </summary>
        /// <value>
        /// The user the message comes from.
        /// </value>
        [JsonProperty]
        public string FromUser {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets to user.
        /// </summary>
        /// <value>
        /// To which user to send the message
        /// </value>
        [JsonProperty]
        public string ToUser {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message text.
        /// </value>
        [JsonProperty]
        public string Message {
            get;
            set;
        }


        /// <summary>
        /// Gets the message type to send.
        /// </summary>
        /// <value>
        /// The message type to send.
        /// </value>
        [JsonProperty]
        public MessageType MessageTypeToSend {
            get;
            private set;
        }




    }
}