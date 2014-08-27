using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OldSchoolBeats.SignalR {

    /// <summary>
    /// What kind of message do
    /// we have?
    /// </summary>
    public enum MessageType {
        BroadcastMessage,
        SingleUserMessage,
        UserToUserMessage
    }
}