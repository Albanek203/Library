using System.Collections.Generic;

namespace Library.Data.Interfaces {
    public interface IRepository<T> {
        void           Add(T     data);
        T              Find(T    data);
        IEnumerable<T> FindAll(T data);
    }
}