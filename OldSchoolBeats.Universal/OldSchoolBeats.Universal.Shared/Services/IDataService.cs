using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OldSchoolBeats.Universal.Services {
    public interface IDataService<T> {

        ICollection<T> Items {
            get;
            set;
        }

        void SearchItems(Expression<Func<T, bool>> predicate);

        Task DeleteItem(T item);

        Task AddItem(T item);
        Task UpdateItem(T item, T delta);

    }
}
