using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OldSchoolBeats.ClientModel;
using System.Net.Http;
using GalaSoft.MvvmLight;
using OldSchoolBeats.Universal.ViewModel;
using System.Linq;

namespace OldSchoolBeats.Universal.Services {
    public class OldSchoolArtistsDataService:ObservableObject,IDataService<OldSchoolArtist> {

        MobileServiceClient _client;

        MobileServiceCollection<OldSchoolArtist, OldSchoolArtist> items;

        public MobileServiceCollection<OldSchoolArtist,OldSchoolArtist> Items {
            get {
                return items;
            }

            set {
                items = value;
                RaisePropertyChanged("Items");
            }
        }

        public OldSchoolArtist SelectedItem {
            get;
            set;
        }

        private BindableOldSchoolArtist dataContext;

        public BindableOldSchoolArtist DataContext {
            get {
                return dataContext;
            }

            set {
                dataContext = value;
                RaisePropertyChanged("DataContext");
            }
        }

        public OldSchoolArtistsDataService(MobileServiceClient client) {
            this._client = client;
            this.DataContext = new BindableOldSchoolArtist();
        }

        public async Task FillItems() {

            var query = _client.GetTable<OldSchoolArtist>().Take(10);

            this.Items = await query.ToCollectionAsync<OldSchoolArtist>();
        }

        public void SearchItems(System.Linq.Expressions.Expression<Func<OldSchoolArtist, bool>> predicate) {

            var query = _client.GetTable<OldSchoolArtist>().Where(predicate);
            this.Items = new MobileServiceCollection<OldSchoolArtist>(query);
        }

        public async Task DeleteItem(OldSchoolArtist item) {

            await _client.GetTable<OldSchoolArtist>().DeleteAsync(item);

            var query = _client.GetTable<OldSchoolArtist>().Take(10);


            this.Items = await query.ToCollectionAsync<OldSchoolArtist>();

        }

        public async Task AddItem(OldSchoolArtist item) {


            var url = await _client.InvokeApiAsync<string>("LastFM", HttpMethod.Post, new Dictionary<string, string>() {
                { "artistName", item.ImageUrl
                }
            });


            if(string.IsNullOrEmpty(url)) {
                url = "http://lorempixel.com/g/150/150/";
            }

            item.ImageUrl = url;

            await _client.GetTable<OldSchoolArtist>().InsertAsync(item);
            var query = _client.GetTable<OldSchoolArtist>().Take(10);


            this.Items = await query.ToCollectionAsync<OldSchoolArtist>();

        }

        public async Task UpdateItem(BindableOldSchoolArtist delta, OldSchoolArtist item) {

            var dbItems = await _client.GetTable<OldSchoolArtist>().Where(i => i.ImageUrl == item.ImageUrl).ToListAsync();

            var dbItem = dbItems[0];

            dbItem.Artist = delta.Artist;
            dbItem.RelatedStyles = delta.RelatedStyles;
            dbItem.YearsArchive = delta.YearsArchive;


            await _client.GetTable<OldSchoolArtist>().UpdateAsync(dbItem);

            var query = _client.GetTable<OldSchoolArtist>().Take(10);


            this.Items = await query.ToCollectionAsync<OldSchoolArtist>();
        }



        public ICollection<OldSchoolArtist> SearchAndReturnItems(System.Linq.Expressions.Expression<Func<OldSchoolArtist, bool>> predicate) {

            var query = _client.GetTable<OldSchoolArtist>().Where(predicate);
            var items =  new MobileServiceCollection<OldSchoolArtist>(query);

            return items;
        }
    }
}
