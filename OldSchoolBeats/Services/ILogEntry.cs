using System;
namespace OldSchoolBeats.Services {
    public interface ILogEntry {
        DateTime Date {
            get;
            set;
        }
        string LogMessage {
            get;
            set;
        }
        LogServerity Severity {
            get;
            set;
        }
    }
}
