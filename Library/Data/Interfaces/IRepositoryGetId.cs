namespace Library.Data.Interfaces {
    public interface IRepositoryGetId<T> {
        int GetId(T data);
    }
}