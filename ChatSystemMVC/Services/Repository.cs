using ChatSystemMVC.ApplicationDBcontext;
using Microsoft.EntityFrameworkCore;

namespace ChatSystemMVC.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _Context;

        public Repository(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        public void Add(T t)
        {
            _Context.Set<T>().Add(t);
        }

        public IEnumerable<T> Get()
        {
            return _Context.Set<T>().ToList();
        }

        public  T GetById(int id)
        {
            return _Context.Set<T>().Find(id);
        }

        public void Remove(int id)
        {
            T t = _Context.Set<T>().Find(id);
            _Context.Set<T>().Remove(t);
        }

        public T Update(T t)
        {
            _Context.Attach(t);
            _Context.Entry(t).Property(p => p).IsModified = true;
            _Context.SaveChanges();
            return t;
        }
    }
}
