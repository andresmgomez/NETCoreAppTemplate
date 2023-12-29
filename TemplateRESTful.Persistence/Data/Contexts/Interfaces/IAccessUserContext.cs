using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Persistence.Data.Contexts
{
    public interface IAccessUserContext
    {
        public string UserId { get; }
        public string UserName { get; }
    }
}
