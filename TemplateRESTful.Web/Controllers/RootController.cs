using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Service.Client.Features;

namespace TemplateRESTful.Web.Controllers
{
    public abstract class RootController<T> : Controller
    {
        private NotificationService notificationInstance;
        private ILogger<T> _loggerInstance;
        private IMediator _mediatorInstance;
        private IMapper _mapperInstance;
        protected NotificationService _notificationService =>
          notificationInstance ??= HttpContext.RequestServices.
          GetService<NotificationService>();

        protected ILogger<T> _logger =>
            _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected IMediator _mediator =>
            _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IMapper _mapper =>
            _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

    }
}
