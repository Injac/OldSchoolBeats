using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using OldSchoolBeats.ClientModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OldSchoolBeats.Universal.Services {
    public class DesignTimeDataService:IDataService<OldSchoolArtist> {


        public ICollection<OldSchoolArtist> Items {
            get;
            set;
        }

        private ObservableCollection<OldSchoolArtist> _sampleData;

        public DesignTimeDataService() {

            this._sampleData = new ObservableCollection<OldSchoolArtist>();

            this._sampleData.Add(new OldSchoolArtist() {
                Artist="Grandmaster Flash",RelatedStyles="Groove",
                YearsArchive="1984",
                ImageUrl = "http://lorempixel.com/g/150/150/"
            });

            this._sampleData.Add(new OldSchoolArtist() {
                Artist = "RUN DMC",
                RelatedStyles = "Funk",
                YearsArchive = "1984",
                ImageUrl="http://lorempixel.com/g/150/150/"
            });

            this._sampleData.Add(new OldSchoolArtist() {
                Artist = "Curtis Blow",
                RelatedStyles = "Rap",
                YearsArchive = "1994",
                ImageUrl = "http://lorempixel.com/g/150/150/"
            });

            this._sampleData.Add(new OldSchoolArtist() {
                Artist = "Naughty By Nature",
                RelatedStyles = "RNB",
                YearsArchive = "1993",
                ImageUrl="http://lorempixel.com/g/150/150/"

            });

            this._sampleData.Add(new OldSchoolArtist() {
                Artist = "Tag Team",
                RelatedStyles = "OldSchool",
                YearsArchive = "1993",
                ImageUrl="http://lorempixel.com/g/150/150/"
            });

            this._sampleData.Add(new OldSchoolArtist() {
                Artist = "Cypress Hill",
                RelatedStyles = "OldSchool",
                YearsArchive = "1993",
                ImageUrl="http://lorempixel.com/g/150/150/"
            });


            this._sampleData.Add(new OldSchoolArtist() {
                Artist = "Young MC",
                RelatedStyles = "OldSchool",
                YearsArchive = "1989",
                ImageUrl = "http://lorempixel.com/g/150/150/"
            });

            this.Items = _sampleData;
        }

        public void SearchItems(Expression<Func<OldSchoolArtist, bool>> predicate) {

            this.Items = (ICollection<OldSchoolArtist>) this._sampleData.Where(predicate.Compile());
        }

        public Task DeleteItem(OldSchoolArtist item) {

            return Task.Run(()=> {
                this._sampleData.Remove(item);
                this.Items = (ICollection<OldSchoolArtist>) this._sampleData;
            });
        }

        public Task AddItem(OldSchoolArtist item) {
            return Task.Run(() => {
                this._sampleData.Add(item);
                this.Items = (ICollection<OldSchoolArtist>)this._sampleData;
            });
        }

        public  Task UpdateItem(OldSchoolArtist item, OldSchoolArtist delta) {

            return Task.Run(() => {
                item.Artist = delta.Artist;
                item.RelatedStyles = delta.RelatedStyles;
                item.YearsArchive = delta.YearsArchive;

                var index = this._sampleData.IndexOf(item);
                this._sampleData[index] = item;

                this.Items = (ICollection<OldSchoolArtist>)this._sampleData;
            });
        }
    }
}
