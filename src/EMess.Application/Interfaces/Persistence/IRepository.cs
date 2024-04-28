namespace EMess.Application.Interfaces.Persistence
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetMultiple(string query, object param);
        Task<T> GetSingle(string query, object param);
        Task InsertAsync(T obj);
        void Update(T obj);
        void Delete(object id);
        Task SaveAsync();
    }
}
