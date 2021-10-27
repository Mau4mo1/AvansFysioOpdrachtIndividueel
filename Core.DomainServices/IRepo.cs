using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public interface IRepo<T>
    {
        public List<T> Get();
        public T Get(int id);
        public T Get(T entity);
        public void Remove(int id);
        public void Create(T entity);
        public void Update(T entity, int id);
    }
}