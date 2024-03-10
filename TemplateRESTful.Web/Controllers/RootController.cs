using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Infrastructure.Client.Services;
using TemplateRESTful.Service.Features.Notifications;

namespace TemplateRESTful.Web.Controllers
{
    public abstract class RootController<T> : Controller
    {
        private IRenderRazorView _viewerInstance;
        private INotyfNotificationService notificationInstance;
        private ILogger<T> _loggerInstance;
        private IMediator _mediatorInstance;
        private IMapper _mapperInstance;

        protected IRenderRazorView _viewer => _viewerInstance ??=
            HttpContext.RequestServices.GetService<IRenderRazorView>();
        protected INotyfNotificationService _notificationService =>
          notificationInstance ??= HttpContext.RequestServices.
          GetService<INotyfNotificationService>();

        protected ILogger<T> _logger =>
            _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected IMediator _mediator =>
            _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IMapper _mapper =>
            _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

    }
}
