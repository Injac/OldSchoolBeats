using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldSchoolBeats.Services {
    /// <summary>
    /// The blueprint for any kind
    /// of logging service.
    /// </summary>
    public interface ILoggingService {

        Task WriteLogEntry(LogEntry logEntry);

    }
}
