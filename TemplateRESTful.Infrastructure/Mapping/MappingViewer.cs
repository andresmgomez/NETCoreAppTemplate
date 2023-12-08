using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TemplateRESTful.Infrastructure.Mapping
{
    public class MappingViewer : IMappingViewer
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;
        private readonly IRazorPageActivator _razorActivator;

        public MappingViewer(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider, IHttpContextAccessor httpContext,
            IRazorPageActivator razorActivator, IActionContextAccessor actionContext)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;

            _httpContext = httpContext;
            _actionContext = actionContext;
            _razorActivator = razorActivator;
        }

        public async Task<string> RenderViewToString<T>(string viewName, T model)
        {
            var actionContext = new ActionContext(
                _httpContext.HttpContext,
                _httpContext.HttpContext.GetRouteData(),
                _actionContext.ActionContext.ActionDescriptor
            );

            using (var stringWriter = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindPage(actionContext, viewName);

                if (viewResult.Page == null)
                {
                    throw new ArgumentNullException($"The following view {viewName} could not be found.");
                }

                var razorView = new RazorView(
                    _razorViewEngine,
                    _razorActivator, new List<IRazorPage>(), viewResult.Page,
                    HtmlEncoder.Default, new System.Diagnostics.DiagnosticListener("ViewRenderService")
                );

                var viewContext = new ViewContext(
                    actionContext, razorView, new ViewDataDictionary<T>(new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                    {
                        Model = model
                    },

                    new TempDataDictionary(_httpContext.HttpContext, _tempDataProvider),
                    stringWriter,
                    new HtmlHelperOptions()
                );

                var viewPage = viewResult.Page;
                viewPage.ViewContext = viewContext;

                _razorActivator.Activate(viewPage, viewContext);
                await viewPage.ExecuteAsync();

                return viewPage.ToString();
            }
        }
    }
}
