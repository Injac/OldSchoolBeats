using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace OldSchoolBeats.Services {

    /// <summary>
    /// The entry to log.
    /// </summary>
    public class LogEntry : TableEntity,ILogEntry {

        /// <summary>
        /// Gets or sets the log message for the log-entry.
        /// </summary>
        /// <value>
        /// The log message.
        /// </value>
        public string LogMessage {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date for the log-entry.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the severity for the log-entry.
        /// </summary>
        /// <value>
        /// The severity.
        /// </value>
        public LogServerity Severity {
            get;
            set;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        /// <param name="date">The date.</param>
        /// <param name="severity">The severity.</param>
        /// <exception cref="System.ArgumentException">
        /// Parameter cannot be null, empty or whitespace.;logMessage
        /// or
        /// Parameter cannot be null.;date
        /// or
        /// Parmeter cannot be null.;severity
        /// </exception>
        public LogEntry(string logMessage,DateTime? date,LogServerity? severity) {

            if(string.IsNullOrEmpty(logMessage) || string.IsNullOrWhiteSpace(logMessage)) {

                throw new ArgumentException("Parameter cannot be null, empty or whitespace.","logMessage");

            }

            if(date == null) {

                throw new ArgumentException("Parameter cannot be null.","date");

            }


            if(severity == null) {
                throw new ArgumentException("Parmeter cannot be null.","severity");
            }

            //Set the "name" of the enum as partition key
            this.PartitionKey = Enum.GetName(typeof(LogServerity),Severity);
            this.RowKey = this.Date.ToString("d");

        }


    }
}