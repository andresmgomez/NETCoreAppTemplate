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

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
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
                        // custom response error
                        response.StatusCode = (int)HttpStatusCode.BadRequest; 
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var serverResponse = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(serverResponse);
            }
        }
    }
}