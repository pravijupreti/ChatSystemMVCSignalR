namespace ChatSystemMVC
{
     public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get();
        T GetById(int id);
        void Add(T t);
        void Remove(int id);
        T Update(T t);
    }
}