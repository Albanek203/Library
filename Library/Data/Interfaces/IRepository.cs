namespace Library.Data.Interfaces {
    public interface IRepository<T> {
        void Add(T data);
        T Find(T data);
        T FindAll(T data);
    }
}