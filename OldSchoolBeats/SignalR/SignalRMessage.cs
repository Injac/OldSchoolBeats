using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace OldSchoolBeats.SignalR {
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
        private string FromUser {
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
        private string ToUser {
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
        private string Message {
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


        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRMessage"/> class.
        /// To send a broadcast message, just omit the fromUser and toUser
        /// parameters.
        /// To send a message to a specific user, set toUser and omit fromUser.
        /// To send a message from on user, to another user set fromUser and toUser.
        /// Message cannot be omitted and is obligatory.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        /// <param name="message">The message.</param>
        public SignalRMessage (string message,string fromUser=null,string toUser=null) {

            //We have a broadcast message here
            if((string.IsNullOrEmpty(toUser) || string.IsNullOrWhiteSpace(toUser)) &&
                    (string.IsNullOrEmpty(fromUser) || string.IsNullOrWhiteSpace(fromUser))) {

                this.MessageTypeToSend = MessageType.BroadcastMessage;
            }

            //We have a single user message here
            if (!(string.IsNullOrEmpty(toUser) || !string.IsNullOrWhiteSpace(toUser)) &&
                    (string.IsNullOrEmpty(fromUser) || string.IsNullOrWhiteSpace(fromUser))) {

                this.MessageTypeToSend = MessageType.SingleUserMessage;
            }

            //We have a user to user message
            if (!(string.IsNullOrEmpty(toUser) || !string.IsNullOrWhiteSpace(toUser)) &&
                    (!string.IsNullOrEmpty(fromUser) || !string.IsNullOrWhiteSpace(fromUser))) {

                this.MessageTypeToSend = MessageType.UserToUserMessage;
            }


            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) {
                throw new ArgumentException("Parameter cannot be null, empty or whitespace", "message");
            }

            //Let's keep up the performance
            if(message.Length > 200) {
                message = message.Substring(0,199);
            }



            this.FromUser = fromUser;
            this.ToUser = toUser;
            this.Message = message;
        }

    }
}