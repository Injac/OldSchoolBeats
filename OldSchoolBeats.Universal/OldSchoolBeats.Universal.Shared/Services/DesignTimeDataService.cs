using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using OldSchoolBeats.ClientModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OldSchoolBeats.Universal.ViewModel;

namespace OldSchoolBeats.Universal.Services {
    public class DesignTimeDataService:IDataService<OldSchoolArtist> {


        public MobileServiceCollection<OldSchoolArtist,OldSchoolArtist> Items {
            get;
            set;
        }


        public OldSchoolArtist SelectedItem {
            get;
            set;
        }

        public BindableOldSchoolArtist DataContext {
            get;
            set;
        }

        private ObservableCollection<OldSchoolArtist> _sampleData;

        public Task FillItems() {
            throw new NotImplementedException();
        }

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

            foreach(var i in _sampleData) {
                this.Items.Add(i);
            }
        }

        public void SearchItems(Expression<Func<OldSchoolArtist, bool>> predicate) {

            var items =  (ICollection<OldSchoolArtist>) this._sampleData.Where(predicate.Compile());

            foreach(var i in items) {
                this.Items.Add(i);
            }
        }

        public Task DeleteItem(OldSchoolArtist item) {

            throw new NotImplementedException();
        }

        public Task AddItem(OldSchoolArtist item) {
            throw new NotImplementedException();
        }

        public  Task UpdateItem(BindableOldSchoolArtist item, OldSchoolArtist delta) {

            throw new NotImplementedException();
        }


        public ICollection<OldSchoolArtist> SearchAndReturnItems(Expression<Func<OldSchoolArtist, bool>> predicate) {
            throw new NotImplementedException();
        }
    }
}
