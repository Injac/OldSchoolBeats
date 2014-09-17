using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OldSchoolBeats.ClientModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OldSchoolBeats.Universal {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Storage : Page {

        //Where do we start to page
        private const int PAGING_SKIP = 0;
        //How many items do we take at a time
        private const int PAGING_TAKE = 10;

        //Which page are we on?
        private static int currentPage;




        private RelayCommand pageNext;

        public RelayCommand PageNext {
            get {
                return pageNext;
            }

            set {
                pageNext = value;
            }
        }

        private RelayCommand pagePrevious;

        public RelayCommand PagePrevious {
            get {
                return pagePrevious;
            }

            set {
                pagePrevious = value;
            }
        }


        private RelayCommand<string> download;

        public RelayCommand<string> Download {
            get {
                return download;
            }

            set {
                download = value;
            }
        }

        private RelayCommand upload;

        public RelayCommand Upload {
            get {
                return upload;
            }

            set {
                upload = value;
            }
        }

        private RelayCommand<string> delete;

        public RelayCommand<string> Delete {
            get {
                return delete;
            }

            set {
                delete = value;
            }
        }


        public Storage() {
            this.InitializeComponent();

            this.PagePrevious = new RelayCommand(PagePrev);
            this.PageNext = new RelayCommand(PageNxt);
            this.Upload = new RelayCommand(Upld);
            this.Download = new RelayCommand<string>(Dwnld);
            this.Delete = new RelayCommand<string>(Del);
            this.DataContext = this;
            this.Loaded += Storage_Loaded;
        }

        private async void Del(string blobName) {

            var queryObject = new BlobManipulationData();

            queryObject.BlobName = queryObject.BlobName = (string)lstBlobs.SelectedValue;
            queryObject.ContainerName = "testcontainer";

            var dataToken = JToken.FromObject(queryObject);
            var blobsJToken = await App.MobileService.InvokeApiAsync("deleteblob", dataToken);


        }

        private async void Dwnld(string blobName) {


            var queryObject = new BlobManipulationData();

            queryObject.BlobName = (string) lstBlobs.SelectedValue;
            queryObject.ContainerName = "testcontainer";


            var dataToken = JToken.FromObject(queryObject);

            var blobsJToken = await App.MobileService.InvokeApiAsync("dowloadblob", dataToken);

            var blobData = blobsJToken.ToObject<byte[]>();

            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile storageFile = await folder.CreateFileAsync(queryObject.BlobName, CreationCollisionOption.ReplaceExisting);

            var stream = await storageFile.OpenStreamForWriteAsync();

            await stream.WriteAsync(blobData,0,blobData.Length);

            stream.Dispose();

        }

        private async void Upld() {


            var filePicker = new FileOpenPicker();

            filePicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;

            filePicker.FileTypeFilter.Clear();

            filePicker.FileTypeFilter.Add("*");

            var selectedFile = await filePicker.PickSingleFileAsync();

            if(selectedFile != null) {


                var stream = await selectedFile.OpenStreamForReadAsync();

                stream.Position = 0;

                var fileData = new byte[stream.Length];

                await stream.ReadAsync(fileData,0, (int) stream.Length);

                var queryObject = new BlobManipulationData();

                queryObject.BlobName = selectedFile.Name;
                queryObject.BlobData = fileData;
                queryObject.ContainerName = "testcontainer";

                var dataToken = JToken.FromObject(queryObject);

                await App.MobileService.InvokeApiAsync("uploadblob",dataToken);
            }


        }

        private async void PageNxt() {

            var queryObject = new BlobManipulationData();

            queryObject.Skip = 10 * currentPage++;
            queryObject.Take = PAGING_TAKE;
            queryObject.ContainerName = "testcontainer";

            var dataToken = JToken.FromObject(queryObject);
            var blobsJToken = await App.MobileService.InvokeApiAsync("pageblobs", dataToken);


            var blobs = JsonConvert.DeserializeObject(blobsJToken.ToString());

            var source = blobsJToken.Values<string>("name").ToList<string>();

            this.lstBlobs.ItemsSource = source;
        }

        private async void PagePrev() {

            var queryObject = new BlobManipulationData();

            if(currentPage > 0) {
                queryObject.Skip = 10 * --currentPage;
            }

            else {
                queryObject.Skip = 0;
            }

            queryObject.Take = PAGING_TAKE;
            queryObject.ContainerName = "testcontainer";

            var dataToken = JToken.FromObject(queryObject);
            var blobsJToken = await App.MobileService.InvokeApiAsync("pageblobs", dataToken);



            var blobs = JsonConvert.DeserializeObject(blobsJToken.ToString());

            var source = blobsJToken.Values<string>("name").ToList<string>();

            this.lstBlobs.ItemsSource = source;
        }

        async void Storage_Loaded(object sender, RoutedEventArgs e) {

            await this.LoadFirstTenBlobEntries();

        }

        private async Task LoadFirstTenBlobEntries() {

            var queryObject = new BlobManipulationData();

            queryObject.Skip = PAGING_SKIP;
            queryObject.Take = PAGING_TAKE;
            queryObject.ContainerName = "testcontainer";

            var dataToken = JToken.FromObject(queryObject);
            var blobsJToken = await App.MobileService.InvokeApiAsync("pageblobs",dataToken);

            var blobs = JsonConvert.DeserializeObject(blobsJToken.ToString());

            var source = blobsJToken.Values<string>("name").ToList<string>();

            this.lstBlobs.ItemsSource = source;

        }

    }
}
