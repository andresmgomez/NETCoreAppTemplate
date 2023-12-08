using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Mapping
{
    public interface IMappingViewer
    {
        Task<string> RenderViewToString<T>(string viewName, T model);
    }
}
