using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Persistence.Data.Containers
{
    public interface IUserActions<T> where T : class
    {
        IList<T> Get();
        IList<T> Read();
        void Add(T action);
        void Clear();
       
    }
}
