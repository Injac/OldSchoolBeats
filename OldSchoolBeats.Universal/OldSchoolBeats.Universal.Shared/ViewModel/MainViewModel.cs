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

        /// <summary>
        /// The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

        private OldSchoolArtist _selectedItem;


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

        /// <summary>
        /// Sets and gets the SelectedItem property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public OldSchoolArtist SelectedItem {
            get {
                return _selectedItem;
            }

            set {
                if (_selectedItem == value) {
                    return;
                }

                _selectedItem = value;
                RaisePropertyChanged(SelectedItemPropertyName);
            }
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
        }

        private void InitDesignMode() {
            this.EditAreaVisible = Visibility.Collapsed;
            this.DataService = SimpleIoc.Default.GetInstance<DesignTimeDataService>();
        }


    }
}