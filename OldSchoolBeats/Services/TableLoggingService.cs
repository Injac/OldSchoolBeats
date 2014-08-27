using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace OldSchoolBeats.Services {

    /// <summary>
    /// A logging service, that allows us
    /// to use a Azure table as a logging
    /// store.
    /// </summary>
    public class TableLoggingService:ILoggingService {

        private static CloudTable logTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableLoggingService"/> class.
        /// </summary>
        public TableLoggingService() {

            logTable = this.GetConnectionTable();
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task WriteLogEntry(LogEntry logEntry) {

            if(logTable != null) {


                await logTable.CreateIfNotExistsAsync();

                var insertOperation = TableOperation.InsertOrReplace(logEntry);

                await logTable.ExecuteAsync(insertOperation);

            }


        }


        /// <summary>
        /// Gets the table to save user data to
        /// code taken from sample:
        /// http://www.asp.net/signalr/overview/signalr-20/hubs-api/mapping-users-to-connections
        /// Very good sample, check it out!
        /// </summary>
        /// <returns></returns>
        private CloudTable GetConnectionTable() {
            var storageAccount =
                CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference("logtable");
        }
    }
}