using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.MobileServices;
using OldSchoolBeats.ClientModel;
using OldSchoolBeats.Universal.Services;
using Windows.UI.Xaml;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using OldSchoolBeats.Universal.Messaging;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Threading;
using System;

namespace OldSchoolBeats.Universal.ViewModel {
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase {

        public IDataService<OldSchoolArtist> DataService {
            get;
            set;
        }


        public INavigationService NavService {
            get;
            set;
        }

        public ILoginService LoginService {
            get;
            set;
        }


        private BindableOldSchoolArtist newArtist;

        public BindableOldSchoolArtist NewArtist {
            get {
                return newArtist;
            }

            set {
                newArtist = value;
            }
        }


        public SignalRMessage SignalRUserMessage {
            get;
            set;
        }


        private ObservableCollection<SignalRMessage> signalrMessages;

        public ObservableCollection<SignalRMessage> SignalrMessages {
            get {
                return signalrMessages;
            }

            set {
                signalrMessages = value;
            }
        }

        private ObservableCollection<string> signalrBroadcastMessages;

        public ObservableCollection<string> SignalrBroadcastMessages {
            get {
                return signalrBroadcastMessages;
            }

            set {
                signalrBroadcastMessages = value;

            }
        }



        /// <summary>
        /// The <see cref="EditAreaVisible" /> property's name.
        /// </summary>
        public const string EditAreaVisiblemPropertyName = "EditAreaVisible";

        private Visibility _editAreaVisible;

        /// <summary>
        /// Sets and gets the EditAreaVisible property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility EditAreaVisible {
            get {
                return _editAreaVisible;
            }

            set {
                if (_editAreaVisible == value) {
                    return;
                }

                _editAreaVisible = value;
                RaisePropertyChanged(EditAreaVisiblemPropertyName);
            }
        }


        private Visibility _addAreaVisible;

        public Visibility AddAreaVisible {
            get {
                return _addAreaVisible;
            }

            set {
                _addAreaVisible = value;
                RaisePropertyChanged("AddAreaVisible");
            }
        }

        private RelayCommand<BindableOldSchoolArtist> addNewArtistCommand;

        public RelayCommand<BindableOldSchoolArtist> AddNewArtistCommand {

            get {
                return this.addNewArtistCommand;
            }

            set {
                this.addNewArtistCommand = value;
            }

        }

        private RelayCommand<OldSchoolArtist> deleteArtistCommand;

        public RelayCommand<OldSchoolArtist> DeleteArtistCommand {

            get {
                return this.deleteArtistCommand;
            }

            set {
                this.deleteArtistCommand = value;
            }

        }


        private RelayCommand editArtistCommand;

        public RelayCommand EditArtistCommand {

            get {
                return this.editArtistCommand;
            }

            set {
                this.editArtistCommand = value;
            }

        }

        private RelayCommand<string> lookupArtistImageCommand;

        public RelayCommand<string> LookupArtistImageCommand {

            get {
                return this.lookupArtistImageCommand;
            }

            set {
                this.lookupArtistImageCommand = value;
            }

        }



        private RelayCommand crudActionCommand;

        public RelayCommand CrudActionCommand {

            get {
                return this.crudActionCommand;
            }

            set {
                this.crudActionCommand = value;
            }

        }


        private RelayCommand cancelCommand;

        public RelayCommand CancelCommand {

            get {
                return this.cancelCommand;
            }

            set {
                this.cancelCommand = value;
            }

        }


        private RelayCommand logoutCommand;

        public RelayCommand LogoutCommand {

            get {
                return this.logoutCommand;
            }

            set {
                this.logoutCommand = value;
            }

        }


        private RelayCommand<string> signalRBroadcastCommand;

        public RelayCommand<string> SignalRBroadcastCommand {

            get {
                return this.signalRBroadcastCommand;
            }

            set {
                this.signalRBroadcastCommand = value;
            }

        }

        private RelayCommand<SignalRMessage> signalRToSpecificUserCommand;

        public RelayCommand<SignalRMessage> SignalRToSpecificUserCommand {

            get {
                return this.signalRToSpecificUserCommand;
            }

            set {
                this.signalRToSpecificUserCommand = value;
            }

        }

        private RelayCommand<SignalRMessage> signalRFromUserToUserCommand;

        public RelayCommand<SignalRMessage> SignalRFromUserToUserCommand {

            get {
                return this.signalRFromUserToUserCommand;
            }

            set {
                this.signalRFromUserToUserCommand = value;
            }

        }

        private RelayCommand<string> navigate;

        public RelayCommand<string> Navigate {
            get {
                return navigate;
            }

            set {
                navigate = value;
            }
        }

        private RelayCommand executeDelteCommand;

        public RelayCommand ExecuteDelteCommand {
            get {
                return executeDelteCommand;
            }

            set {
                executeDelteCommand = value;
            }
        }

        private RelayCommand loginCommand;

        public RelayCommand LoginCommand {
            get {
                return loginCommand;
            }

            set {
                loginCommand = value;
            }
        }


        private RelayCommand<string> toggleEdit;

        public RelayCommand<string> ToggleEdit {
            get {
                return toggleEdit;
            }

            set {
                toggleEdit = value;
            }
        }

        private OldSchoolArtist currentArtist {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {



            if (ViewModelBase.IsInDesignModeStatic) {
                // Code runs in Blend --> create design time data.
                //Let us feed the model with some sample data
                InitDesignMode();
            }

            else {
                // Code runs "for real"
                //Get the real thing
                InitRuntimeMode();
            }
        }

        private void InitRuntimeMode() {
            try {

                #if WINDOWS_APP
                this.EditAreaVisible = Visibility.Collapsed;
                this.AddAreaVisible = Visibility.Collapsed;
                #endif

                this.DataService = SimpleIoc.Default.GetInstance<IDataService<OldSchoolArtist>>();
                this.NavService = SimpleIoc.Default.GetInstance<INavigationService>();
                this.LoginService = SimpleIoc.Default.GetInstance<ILoginService>();
                this.SignalrBroadcastMessages = new ObservableCollection<string>();
                this.SignalrMessages = new ObservableCollection<SignalRMessage>();

                this.NewArtist = new BindableOldSchoolArtist();
                this.SignalRUserMessage = new SignalRMessage();

                this.AddNewArtistCommand = new RelayCommand<BindableOldSchoolArtist>(AddNewArtist);
                this.DeleteArtistCommand = new RelayCommand<OldSchoolArtist>(DeleteArtist);
                this.EditArtistCommand = new RelayCommand(EditArtist);
                this.LookupArtistImageCommand = new RelayCommand<string>(LookupArtist);
                this.CrudActionCommand = new RelayCommand(ExecuteUpdate);
                this.CancelCommand = new RelayCommand(Cancel);
                this.SignalRBroadcastCommand = new RelayCommand<string>(SendBroadCast, CanExecuteSignalRMessageSend);
                this.SignalRFromUserToUserCommand = new RelayCommand<SignalRMessage>(SendFromToUser, CanExecuteSignalRMessageSendFromToUser);
                this.SignalRToSpecificUserCommand = new RelayCommand<SignalRMessage>(SendToSpecificUser, CanExecuteSignalRMessageSendToSpecificUser);
                this.Navigate = new RelayCommand<string>(NavigateAction);
                this.LogoutCommand = new RelayCommand(Logout, CanExecuteLogout);
                this.ExecuteDelteCommand = new RelayCommand(ExecuteDelete);
                this.LoginCommand = new RelayCommand(Login);
                this.ToggleEdit = new RelayCommand<string>(ToggleEditAction);



            }

            catch (System.Exception ex) {

                throw;
            }

        }



        private void ToggleEditAction(string action) {



            if(action.Equals("add")) {
                this.AddAreaVisible = Visibility.Visible;
            }

            if(action.Equals("edit")) {
                this.EditAreaVisible = Visibility.Visible;
                this.EditArtist();
            }
        }

        private async void ExecuteDelete() {
            await this.DataService.DeleteItem(this.DataService.SelectedItem);
        }

        private void NavigateAction(string pageName) {
            this.NavService.NavigateTo(pageName);
        }

        private bool CanExecuteSignalRMessageSendToSpecificUser(SignalRMessage arg) {
            return App.HubConnection.State == ConnectionState.Connected;
        }

        private bool CanExecuteSignalRMessageSendFromToUser(SignalRMessage arg) {
            return App.HubConnection.State == ConnectionState.Connected;
        }

        private bool CanExecuteSignalRMessageSend(string arg) {
            return App.HubConnection.State == ConnectionState.Connected;
        }

        private async void SendToSpecificUser(SignalRMessage signalrData) {

            await App.HubProxy.Invoke("SendToSpecificUser", new object[] { signalrData.ToUser, signalrData.Message });
        }

        private async void SendFromToUser(SignalRMessage signalrData) {
            await App.HubProxy.Invoke("SendToSpecificUserFromSpecificUser", new object[] { signalrData.ToUser, signalrData.Message });
        }

        private async void SendBroadCast(string message) {
            await App.HubProxy.Invoke("Broadcast", new object[] { message });
        }



        private bool CanExecuteLogout() {

            bool canLogout = false;

            Task.Run(async ()=> {
                canLogout = await this.LoginService.UserLoggedIn();
            }).Wait();

            return canLogout;
        }

        private async void Logout() {
            await this.LoginService.LogOut();
        }

        private async void Login() {


            await DispatcherHelper.RunAsync(async () => {
                if (!await this.LoginService.UserLoggedIn()) {
                    var success = await this.LoginService.Login();

                    if (success) {
                        App.User = this.LoginService.MobUser;

                        await this.ConnectToSignalR();

                        await this.DataService.FillItems();
                    }

                    else {
                        App.User = null;
                    }
                }

                else {
                    App.User = this.LoginService.MobUser;
                    await this.ConnectToSignalR();
                    await  this.DataService.FillItems();
                }
            });

        }

        private void Cancel() {

            this.DataService.SelectedItem = null;
            #if WINDOWS_APP
            this.EditAreaVisible = Visibility.Collapsed;
            this.AddAreaVisible = Visibility.Collapsed;
            #endif
            #if WINDOWS_PHONE_APP
            this.Navigate.Execute("MainPage");
            #endif
        }

        private async void ExecuteUpdate() {


            await DataService.UpdateItem(this.DataService.DataContext ,this.DataService.SelectedItem);

            #if WINDOWS_PHONE_APP
            this.Navigate.Execute("MainPage");
            #endif
            #if WINDOWS_APP
            this.EditAreaVisible = Visibility.Collapsed;
            #endif
        }

        private  void LookupArtist(string artistName) {

            //Items are directly filled and bound
            DataService.SearchItems(a=>a.Artist.Equals(artistName));

        }

        private void EditArtist() {


            this.DataService.DataContext = new BindableOldSchoolArtist() {
                YearsArchive = this.DataService.SelectedItem.YearsArchive,
                Artist = this.DataService.SelectedItem.Artist,
                ImageUrl = this.DataService.SelectedItem.ImageUrl,
                RelatedStyles = this.DataService.SelectedItem.RelatedStyles
            };
            #if WINDOWS_PHONE_APP
            this.Navigate.Execute("EditArtist");
            #endif

        }

        private void DeleteArtist(OldSchoolArtist artist) {

            var dialogMessage = new ShowDialogMessage();

            dialogMessage.Yes = this.ExecuteDelteCommand;
            dialogMessage.No = this.CancelCommand;


            Messenger.Default.Send<ShowDialogMessage>(dialogMessage, MessagingIdentifiers.DELETE_CONFIRM_MESSAGE);



        }

        private async void AddNewArtist(BindableOldSchoolArtist artist) {

            var oldArtist = new OldSchoolArtist() {
                Artist = artist.Artist, ImageUrl = artist.ImageUrl, RelatedStyles = artist.RelatedStyles, YearsArchive = artist.YearsArchive
            };

            await this.DataService.AddItem(oldArtist);

            this.NewArtist = new BindableOldSchoolArtist();

            #if WINDOWS_APP
            this.EditAreaVisible = Visibility.Collapsed;
            #endif

            #if WINDOWS_PHONE_APP
            this.Navigate.Execute("MainPage");
            #endif

            #if WINDOWS_APP
            this.AddAreaVisible = Visibility.Collapsed;
            #endif
        }

        private void InitDesignMode() {

            this.EditAreaVisible = Visibility.Visible;
            this.DataService = SimpleIoc.Default.GetInstance<DesignTimeDataService>();
        }

        private async Task ConnectToSignalR() {

            App.HubConnection = new HubConnection(App.MobileService.ApplicationUri.AbsoluteUri);



            if (App.User != null) {

                App.HubConnection.Headers["x-zumo-auth"] = App.User.MobileServiceAuthenticationToken;

            }

            else {
                return;
            }




            //Creating the hub proxy. That allows us to send and receive
            //Messages,sexy.
            App.HubProxy = App.HubConnection.CreateHubProxy("MessagingHub");

            try {

                if (App.HubConnection.State == ConnectionState.Disconnected) {
                    App.HubConnection.StateChanged += HubConnection_StateChanged;
                    await App.HubConnection.Start();
                }
            }

            catch (Microsoft.AspNet.SignalR.Client.Infrastructure.StartException ex) {

                throw;
            }

            App.HubProxy.On<string>("receiveMessageFromUser", async (msg) => {


                var message = await this.DesirializeSignalRMEssage(msg);
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    this.SignalrMessages.Add(message);
                });
            });



            App.HubProxy.On<string>("broadcastMessage", (msg) => {


                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    this.SignalrBroadcastMessages.Add(msg);
                });

            });

            App.HubProxy.On<string>("receiveMessageForUser", async (msg) => {

                var message = await this.DesirializeSignalRMEssage(msg);
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    this.SignalrMessages.Add(message);
                });
            });





        }

        void HubConnection_StateChanged(StateChange obj) {

            //You can check here the state of the signalr connection.
        }


        private async Task<SignalRMessage> DesirializeSignalRMEssage(string message) {

            return await Task.Run<SignalRMessage>(() => {

                return JsonConvert.DeserializeObject<SignalRMessage>(message);

            });

        }


    }
}