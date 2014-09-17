using System;
using System.Collections.Generic;
using System.Linq;


namespace OldSchoolBeats.ClientModel {

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