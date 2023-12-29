using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using TemplateRESTful.Infrastructure.Server;

namespace TemplateRESTful.API.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServerLogger _logManager;

        public ErrorMiddleware(RequestDelegate next, IServerLogger logManager)
        {
            _next = next;
            _logManager = logManager;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = ServerResponse<string>.FailedMessage(error.Message);

                switch (error)
                {
                    case InvalidException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        // _logManager.WarningLog(error.Message);
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        // _logManager.InformationLog(error.Message);
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logManager.CriticalLog(error.Message);
                        break;
                }

                var serverResponse = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(serverResponse);
            }
        }
    }
}