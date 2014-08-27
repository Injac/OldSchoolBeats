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

namespace OldSchoolBeats.SignalR {
    /// <summary>
    /// A basic messaging hub.
    /// See it as a base implementation
    /// to send messages to all users,
    /// or to specific users. This can
    /// be uses in many scenarios.
    /// </summary>
    public class MessagingHub:Hub {

        /// <summary>
        /// Broadcasts the specified broadcast messsage.
        /// </summary>
        /// <param name="broadcastMesssage">The broadcast messsage.</param>
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
        public async Task SendToSpecificUserFromSpecificUser(string toUserId, string message) {

            var currentUser = Context.User.Identity.Name;

            var storageTable = this.GetConnectionTable();

            await storageTable.CreateIfNotExistsAsync();

            var userOnline = this.IsUserOnline(toUserId);

            if(userOnline) {


                var user2UserMessage = new SignalRMessage(message,currentUser,toUserId);

                string serializedMessage = string.Empty;

                //This is the recommendation directly coming from GitHub
                //if this has been fixed, please use the async serialization method
                serializedMessage =  await Task.Factory.StartNew<string>(()=> {
                    return JsonConvert.SerializeObject(user2UserMessage);
                });

                Clients.Client(toUserId).receiveMessageFromUser(serializedMessage);

            }


        }




        /// <summary>
        /// Sends to specific user.
        /// </summary>
        /// <param name="toUserId">To user identifier.</param>
        /// <param name="message">The message.</param>
        public async Task SendToSpecificUser(string toUserId, string message) {

            var currentUser = Context.User.Identity.Name;

            var storageTable = this.GetConnectionTable();

            await storageTable.CreateIfNotExistsAsync();

            var userOnline = this.IsUserOnline(toUserId);

            if (userOnline) {

                //Send to specific user
                var user2UserMessage = new SignalRMessage(message,null,toUserId);

                string serializedMessage = string.Empty;

                //This is the recommendation directly coming from GitHub
                //if this has been fixed, please use the async serialization method
                serializedMessage = await Task.Factory.StartNew<string>(() => {
                    return JsonConvert.SerializeObject(user2UserMessage);
                });

                Clients.Client(toUserId).receiveMessageForUser(serializedMessage);

            }
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

            var name = Context.User.Identity.Name;
            var table = GetConnectionTable();
            table.CreateIfNotExists();

            var entity = new SignalRUserStore(
                name.ToLower(),
                Context.ConnectionId);
            var insertOperation = TableOperation.InsertOrReplace(entity);
            table.Execute(insertOperation);


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

            var name = Context.User.Identity.Name;
            var table = GetConnectionTable();

            var deleteOperation = TableOperation.Delete(
            new SignalRUserStore(name, Context.ConnectionId) {
                ETag = "*"
            });
            table.Execute(deleteOperation);

            return base.OnDisconnected(stopCalled);


        }

        /// <summary>
        /// Called when the user is re-connected to this hub.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" />
        /// </returns>
        public override  Task OnReconnected() {


            var currentUser = Context.User.Identity.Name;

            var msg = new SignalRMessage("You have been successfully re-connected.",null,currentUser);

            string jsonContent = string.Empty;

            Task.Factory.StartNew(()=> {

                jsonContent =  JsonConvert.SerializeObject(msg);

            }).Wait();


            Clients.Client(currentUser).reconnectedMessage(jsonContent);

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
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));
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

            var query = new TableQuery<SignalRUserStore>()
            .Where(TableQuery.GenerateFilterCondition(
                       "PartitionKey",
                       QueryComparisons.Equal,
                       userName));

            var queryResult = table.ExecuteQuery(query).ToList();

            if (queryResult.Count == 0) {
                return false;
            }

            return true;


        }

    }
}