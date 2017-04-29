
using System;
using System.Collections.Generic;

namespace SqlEditor.WebApi.Data
{
    public interface IRepository<T>
    {
        T Save(T model);
        void Remove (Guid model);
        T FindByName(string name);
        IEnumerable<T> GetAll();
    }

}