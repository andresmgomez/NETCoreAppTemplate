using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Data.Actions
{
    public interface ITempActionsContainer
    {
        T Get<T>(string data) where T : class;
        T Peek<T>(string data) where T : class;
        void Add(string data, object entity);
        bool Remove(string data);
    }
}
