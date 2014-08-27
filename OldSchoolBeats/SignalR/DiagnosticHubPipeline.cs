using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.WindowsAzure.Mobile.Service;
using OldSchoolBeats.Services;

namespace OldSchoolBeats.SignalR {
    /// <summary>
    /// The storage pipeline to save
    /// user data that flows through
    /// SignalR.
    /// </summary>
    public class DiagnosticHubPipeline : HubPipelineModule {

        //Get the current services instance
        //So that we can log errors
        //and other stuff.
        public ApiServices Services {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// Will be injected for us.
        /// Check WebApiConfig.cs
        /// for details.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public TableLoggingService Logger {
            get;
            set;
        }

        /// <summary>
        /// This method is called before the connect components of any modules added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" /> are
        /// executed. If this returns false, then those later-added modules and the <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnConnected" /> method will
        /// not be run.
        /// </summary>
        /// <param name="hub">The hub the client has connected to.</param>
        /// <returns>
        /// true, if the connect components of later added modules and the <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnConnected" /> method should be executed;
        /// false, otherwise.
        /// </returns>
        protected override bool OnBeforeConnect(IHub hub) {

            var user = hub.Context.User.Identity.Name;

            var logEntry = new LogEntry(
                string.Format("User: {0}, before-connect", user), DateTime.UtcNow, LogServerity.Informational);

            Task.Factory.StartNew(async () => {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();

            return base.OnBeforeConnect(hub);
        }

        /// <summary>
        /// This method is called after the connect components of any modules added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" /> are
        /// executed and after <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnConnected" /> is executed, if at all.
        /// </summary>
        /// <param name="hub">The hub the client has connected to.</param>
        protected override void OnAfterConnect(IHub hub) {

            var user = hub.Context.User.Identity.Name;

            var logEntry = new LogEntry(
                string.Format("User: {0}, after-connect", user), DateTime.UtcNow, LogServerity.Informational);

            Task.Factory.StartNew(async () => {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();
        }


        /// <summary>
        /// This method is called after the disconnect components of any modules added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" /> are
        /// executed and after <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnDisconnected(System.Boolean)" /> is executed, if at all.
        /// </summary>
        /// <param name="hub">The hub the client has disconnected from.</param>
        /// <param name="stopCalled">true, if stop was called on the client closing the connection gracefully;
        /// false, if the client timed out. Timeouts can be caused by clients reconnecting to another SignalR server in scaleout.</param>
        protected override void OnAfterDisconnect(IHub hub, bool stopCalled) {

            var user = hub.Context.User.Identity.Name;

            var logEntry = new LogEntry(
                string.Format("User: {0}, disconnected", user), DateTime.UtcNow, LogServerity.Informational);

            Task.Factory.StartNew(async () => {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();

            base.OnAfterDisconnect(hub, stopCalled);
        }

        /// <summary>
        /// This method is called before the reconnect components of any modules added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" /> are
        /// executed. If this returns false, then those later-added modules and the <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnReconnected" /> method will
        /// not be run.
        /// </summary>
        /// <param name="hub">The hub the client has reconnected to.</param>
        /// <returns>
        /// true, if the reconnect components of later added modules and the <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnReconnected" /> method should be executed;
        /// false, otherwise.
        /// </returns>
        protected override bool OnBeforeReconnect(IHub hub) {

            var user = hub.Context.User.Identity.Name;

            var logEntry = new LogEntry(
                string.Format("User: {0}, before-reconnect", user), DateTime.UtcNow, LogServerity.Informational);

            Task.Factory.StartNew(async () => {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();

            return base.OnBeforeReconnect(hub);
        }

        /// <summary>
        /// This method is called after the reconnect components of any modules added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" /> are
        /// executed and after <see cref="M:Microsoft.AspNet.SignalR.Hubs.IHub.OnReconnected" /> is executed, if at all.
        /// </summary>
        /// <param name="hub">The hub the client has reconnected to.</param>
        protected override void OnAfterReconnect(IHub hub) {

            var user = hub.Context.User.Identity.Name;

            var logEntry = new LogEntry(
                string.Format("User: {0}, reconnected",user), DateTime.UtcNow, LogServerity.Informational);

            Task.Factory.StartNew(async () => {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();

            base.OnAfterReconnect(hub);
        }

        /// <summary>
        /// This is called when an uncaught exception is thrown by a server-side hub method or the incoming component of a
        /// module added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" />. Observing the exception using this method will not prevent
        /// it from bubbling up to other modules.
        /// </summary>
        /// <param name="exceptionContext">Represents the exception that was thrown during the server-side invocation.
        /// It is possible to change the error or set a result using this context.</param>
        /// <param name="invokerContext">A description of the server-side hub method invocation.</param>
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext) {


            var logEntry = new LogEntry(exceptionContext.Error.ToString(),DateTime.UtcNow,LogServerity.Error);

            Task.Factory.StartNew(async ()=> {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();

            base.OnIncomingError(exceptionContext, invokerContext);
        }

        /// <summary>
        /// This method is called before the outgoing components of any modules added later to the <see cref="T:Microsoft.AspNet.SignalR.Hubs.IHubPipeline" /> are
        /// executed. If this returns false, then those later-added modules and the client-side hub method invocation(s) will not
        /// be executed.
        /// </summary>
        /// <param name="context">A description of the client-side hub method invocation.</param>
        /// <returns>
        /// true, if the outgoing components of later added modules and the client-side hub method invocation(s) should be executed;
        /// false, otherwise.
        /// </returns>
        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context) {

            var called = context.Signal;

            var logEntry = new LogEntry(
                string.Format("Signal: {0}, was called.", called), DateTime.UtcNow, LogServerity.Informational);

            Task.Factory.StartNew(async () => {
                await Logger.WriteLogEntry(logEntry);
            }).Wait();


            return base.OnBeforeOutgoing(context);
        }
    }
}