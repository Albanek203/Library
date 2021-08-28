namespace Library.Data.Interfaces {
    public interface IRepositoryLogin<T> {
        int Login(T data);
        bool Register(T data);
    }
}