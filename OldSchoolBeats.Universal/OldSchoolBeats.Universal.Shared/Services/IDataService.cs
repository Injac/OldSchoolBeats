using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OldSchoolBeats.ClientModel;

namespace OldSchoolBeats.Universal.Services {
    public interface IDataService<T> {

        ICollection<T> Items {
            get;
            set;
        }

        T SelectedItem {
            get;
            set;
        }

        void SearchItems(Expression<Func<T, bool>> predicate);

        ICollection<OldSchoolArtist> SearchAndReturnItems(Expression<Func<T, bool>> predicate);

        Task DeleteItem(T item);

        Task AddItem(T item);
        Task UpdateItem(T item, T delta);

    }
}
