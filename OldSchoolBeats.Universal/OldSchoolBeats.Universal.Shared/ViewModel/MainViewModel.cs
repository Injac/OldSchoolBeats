using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.MobileServices;
using OldSchoolBeats.ClientModel;
using OldSchoolBeats.Universal.Services;
using Windows.UI.Xaml;

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


        public OldSchoolArtist NewArtist {
            get {
                return new OldSchoolArtist();
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



        private RelayCommand<OldSchoolArtist> addNewArtistCommand;

        public RelayCommand<OldSchoolArtist> AddNewArtistCommand {

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


        private RelayCommand<OldSchoolArtist> editArtistCommand;

        public RelayCommand<OldSchoolArtist> EditArtistCommand {

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
            this.EditAreaVisible = Visibility.Collapsed;

            this.DataService = SimpleIoc.Default.GetInstance<IDataService<OldSchoolArtist>>();

            this.AddNewArtistCommand = new RelayCommand<OldSchoolArtist>(AddNewArtist);
            this.DeleteArtistCommand = new RelayCommand<OldSchoolArtist>(DeleteArtist);
            this.EditArtistCommand = new RelayCommand<OldSchoolArtist>(EditArtist);
            this.LookupArtistImageCommand = new RelayCommand<string>(LookupArtist);
            this.CrudActionCommand = new RelayCommand(ExecuteUpdate);
            this.CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel() {

            this.DataService.SelectedItem = null;
            this.EditAreaVisible = Visibility.Collapsed;
        }

        private void ExecuteUpdate() {

            var dataService = (OldSchoolArtistsDataService)this.DataService;
            DataService.UpdateItem(dataService.DataContext as OldSchoolArtist,this.DataService.SelectedItem);

        }

        private  void LookupArtist(string artistName) {

            //Items are directly filled and bound
            DataService.SearchItems(a=>a.Artist.Equals(artistName));

        }

        private void EditArtist(OldSchoolArtist artist) {

            var dataService = (OldSchoolArtistsDataService) this.DataService;
            dataService.DataContext = artist;

        }

        private async void DeleteArtist(OldSchoolArtist artist) {

            await this.DataService.DeleteItem(artist);
        }

        private async void AddNewArtist(OldSchoolArtist artist) {

            await this.DataService.AddItem(artist);
        }

        private void InitDesignMode() {

            this.EditAreaVisible = Visibility.Visible;
            this.DataService = SimpleIoc.Default.GetInstance<DesignTimeDataService>();
        }


    }
}