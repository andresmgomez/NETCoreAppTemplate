using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TemplateRESTful.Infrastructure.Mapping;

namespace TemplateRESTful.Web.Controllers
{
    public abstract class RootController<T> : Controller
    {
        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;
        private IMappingViewer _viewerInstance;
        private IMapper _mapperInstance;

        protected IMediator _mediator =>
            _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ILogger<T> _logger =>
            _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();

        protected IMappingViewer _viewer => _viewerInstance ??=
            HttpContext.RequestServices.GetService<IMappingViewer>();

        protected IMapper _mapper =>
            _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

    }
}
