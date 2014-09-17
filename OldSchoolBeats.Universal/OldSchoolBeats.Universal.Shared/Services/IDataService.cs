using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OldSchoolBeats.ClientModel;
using Microsoft.WindowsAzure.MobileServices;
using OldSchoolBeats.Universal.ViewModel;

namespace OldSchoolBeats.Universal.Services {
    public interface IDataService<T> {

        MobileServiceCollection<T, T> Items {
            get;
            set;
        }

        T SelectedItem {
            get;
            set;
        }

        BindableOldSchoolArtist DataContext {
            get;
            set;
        }

        void SearchItems(Expression<Func<T, bool>> predicate);

        Task FillItems();

        ICollection<T> SearchAndReturnItems(Expression<Func<T, bool>> predicate);

        Task DeleteItem(T item);

        Task AddItem(T item);
        Task UpdateItem(BindableOldSchoolArtist item, T delta);

    }
}
