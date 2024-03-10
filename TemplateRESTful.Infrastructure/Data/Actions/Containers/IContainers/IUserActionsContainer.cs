using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Data.Containers
{
    public interface IUserActionsContainer<T> where T : class
    {
        IList<T> Get();
        IList<T> Read();
        void Add(T action);
        void Clear();
       
    }
}
