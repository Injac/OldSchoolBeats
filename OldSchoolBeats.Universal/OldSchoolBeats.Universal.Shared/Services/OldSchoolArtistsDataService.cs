using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OldSchoolBeats.ClientModel;

namespace OldSchoolBeats.Universal.Services {
    public class OldSchoolArtistsDataService:IDataService<OldSchoolArtist> {

        MobileServiceClient _client;

        public ICollection<OldSchoolArtist> Items {
            get;
            set;
        }

        public OldSchoolArtist SelectedItem {
            get;
            set;
        }

        public object DataContext {
            get;
            set;
        }

        public OldSchoolArtistsDataService(MobileServiceClient client) {
            this._client = client;
        }

        public void SearchItems(System.Linq.Expressions.Expression<Func<OldSchoolArtist, bool>> predicate) {

            var query = _client.GetTable<OldSchoolArtist>().Where(predicate);
            this.Items = new MobileServiceCollection<OldSchoolArtist>(query);
        }

        public async Task DeleteItem(OldSchoolArtist item) {

            await _client.GetTable<OldSchoolArtist>().DeleteAsync(item);
            var query = _client.GetTable<OldSchoolArtist>().Take(10);
            this.Items = new MobileServiceCollection<OldSchoolArtist>(query);

        }

        public async Task AddItem(OldSchoolArtist item) {

            await _client.GetTable<OldSchoolArtist>().InsertAsync(item);
            var query = _client.GetTable<OldSchoolArtist>().Take(10);
            this.Items = new MobileServiceCollection<OldSchoolArtist>(query);
        }

        public async Task UpdateItem(OldSchoolArtist item, OldSchoolArtist delta) {

            item.Artist = delta.Artist;
            item.RelatedStyles = delta.RelatedStyles;
            item.YearsArchive = delta.YearsArchive;

            await _client.GetTable<OldSchoolArtist>().UpdateAsync(item);

            var query = _client.GetTable<OldSchoolArtist>().Take(10);
            this.Items = new MobileServiceCollection<OldSchoolArtist>(query);
        }





        public ICollection<OldSchoolArtist> SearchAndReturnItems(System.Linq.Expressions.Expression<Func<OldSchoolArtist, bool>> predicate) {
            var query = _client.GetTable<OldSchoolArtist>().Where(predicate);
            var items =  new MobileServiceCollection<OldSchoolArtist>(query);

            return items;
        }
    }
}
