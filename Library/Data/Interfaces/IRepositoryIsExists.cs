namespace Library.Data.Interfaces {
    public interface IRepositoryIsExists<T> {
        bool IsExists(T data);
    }
}