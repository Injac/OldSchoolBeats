using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Microsoft.WindowsAzure.Mobile.Service;

namespace OldSchoolBeats.SignalR {
    /// <summary>
    /// A basic messaging hub.
    /// See it as a base implementation
    /// to send messages to all users,
    /// or to specific users. This can
    /// be uses in many scenarios.
    /// </summary>

    [HubName("MessagingHub")]

    public class MessagingHub:Hub {


        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public ApiServices Services {
            get;
            set;
        }

        /// <summary>
        /// Broadcasts the specified broadcast messsage.
        /// </summary>
        /// <param name="broadcastMesssage">The broadcast messsage.</param>
        [AuthorizeLevel(AuthorizationLevel.User)]
        public void Broadcast(string broadcastMesssage) {


            Clients.All.broadcastMessage(broadcastMesssage);

        }



        /// <summary>
        /// Sends to specific user from specific user.
        /// </summary>
        /// <param name="fromUserId">From user identifier.</param>
        /// <param name="toUserId">To user identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [AuthorizeLevel(AuthorizationLevel.User)]
        public async Task SendToSpecificUserFromSpecificUser(string toUserId, string message) {


            var currentUser = ((ServiceUser)Context.User).Id;

            var storageTable = this.GetConnectionTable();

            await storageTable.CreateIfNotExistsAsync();

            var userOnline = this.IsUserOnline(toUserId);

            if(userOnline) {



                var query = new TableQuery<SignalRUserStore>()
                .Where(TableQuery.GenerateFilterCondition(
                           "PartitionKey",
                           QueryComparisons.Equal,
                           toUserId));

                var queryResult = storageTable.ExecuteQuery(query).FirstOrDefault();

                var user2UserMessage = new SignalRMessage(message,currentUser,toUserId);

                string serializedMessage = string.Empty;

                //This is the recommendation directly coming from GitHub
                //if this has been fixed, please use the async serialization method
                serializedMessage =  await Task.Factory.StartNew<string>(()=> {
                    return JsonConvert.SerializeObject(user2UserMessage);
                });

                Clients.Client(queryResult.RowKey).receiveMessageFromUser(serializedMessage);

            }


        }




        /// <summary>
        /// Sends to specific user.
        /// Not used in the sample.
        /// </summary>
        /// <param name="toUserId">To user identifier.</param>
        /// <param name="message">The message.</param>
        [AuthorizeLevel(AuthorizationLevel.User)]
        public async Task SendToSpecificUser(string toUserId, string message) {

            var currentUser = ((ServiceUser)Context.User).Id.Split(':')[1];

            var storageTable = this.GetConnectionTable();

            await storageTable.CreateIfNotExistsAsync();




            var query = new TableQuery<SignalRUserStore>()
            .Where(TableQuery.GenerateFilterCondition(
                       "PartitionKey",
                       QueryComparisons.Equal,
                       toUserId));

            var queryResult = storageTable.ExecuteQuery(query).FirstOrDefault();

            //Send to specific user
            var user2UserMessage = new SignalRMessage(message,null,toUserId);

            string serializedMessage = string.Empty;

            //This is the recommendation directly coming from GitHub
            //if this has been fixed, please use the async serialization method
            serializedMessage = await Task.Factory.StartNew<string>(() => {
                return JsonConvert.SerializeObject(user2UserMessage);
            });

            Clients.Client(queryResult.RowKey).receiveMessageForUser(serializedMessage);


        }

        /// <summary>
        /// Called when the connection connects to this hub instance.
        /// Code taken from:
        /// http://www.asp.net/signalr/overview/signalr-20/hubs-api/mapping-users-to-connections
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" />
        /// </returns>

        public override Task OnConnected() {

            try {
                var currentUser = ((ServiceUser)Context.User).Id;
                var id = Context.ConnectionId;

                var table = GetConnectionTable();
                table.CreateIfNotExists();

                var entity = new SignalRUserStore(
                    currentUser.Split(':')[1],
                    Context.ConnectionId);
                var insertOperation = TableOperation.InsertOrReplace(entity);
                table.Execute(insertOperation);
            }

            catch (Exception ex) {

                Debug.WriteLine(ex.ToString());

            }


            return base.OnConnected();
        }

        /// <summary>
        /// Called when a connection has disconnected gracefully from this hub instance,
        /// i.e. stop was called on the client.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" />
        /// </returns>

        public override Task OnDisconnected(bool stopCalled) {

            var name = ((ServiceUser)Context.User).Id.Split(':')[1];
            var table = GetConnectionTable();

            try {
                if (!string.IsNullOrEmpty(name)) {
                    var deleteOperation = TableOperation.Delete(
                    new SignalRUserStore(name, Context.ConnectionId) {
                        ETag = "*"
                    });
                    TableOperation retrieveOperation = TableOperation.Retrieve<SignalRUserStore>(name, Context.ConnectionId);
                    TableResult retrievedResult = table.Execute(retrieveOperation);
                    SignalRUserStore checkEntity = retrievedResult.Result as SignalRUserStore;

                    if (checkEntity != null) {
                        table.Execute(deleteOperation);
                    }
                }
            }

            catch (Exception ex) {

                //throw;
            }

            return base.OnDisconnected(stopCalled);


        }

        /// <summary>
        /// Called when the user is re-connected to this hub.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" />
        /// </returns>
        public override Task OnReconnected() {

            try {
                var currentUser = ((ServiceUser)Context.User).Id.Split(':')[1];
                var id = Context.ConnectionId;

                var table = GetConnectionTable();
                table.CreateIfNotExists();

                var entity = new SignalRUserStore(
                    currentUser,
                    Context.ConnectionId);
                var insertOperation = TableOperation.InsertOrReplace(entity);
                table.Execute(insertOperation);
            }

            catch (Exception ex) {

                Debug.WriteLine(ex.ToString());

            }




            return base.OnReconnected();


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
                    Services.Settings["StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference("signalrconnections");
        }

        /// <summary>
        /// Determines whether [is user online] [the specified user name].
        /// code taken from sample:
        /// http://www.asp.net/signalr/overview/signalr-20/hubs-api/mapping-users-to-connections
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Parameter cannot be null, empty or whitespace.</exception>
        private bool IsUserOnline(string userName) {


            if(string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(userName)) {

                throw new ArgumentException("Parameter cannot be null, empty or whitespace.",userName);


            }

            var table = GetConnectionTable();

            var partitionKey = userName.Split(':')[1];

            string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

            var query = new TableQuery<SignalRUserStore>().Where(pkFilter);

            var queryResult = table.ExecuteQuery(query);


            if (queryResult.Count() == 0) {
                return false;
            }

            return true;


        }

    }
}