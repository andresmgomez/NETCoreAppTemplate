using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Client.Services
{
    public interface IRenderRazorView
    {
        Task<string> ConvertPartialViewToHTML<T>(string viewName, T viewModel);
    }
}
