using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using TemplateRESTful.Infrastructure.Client.Constants;

namespace TemplateRESTful.Infrastructure.Utilities
{
    public static class WebAjaxRequests
    {
        public static bool isNotyfAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!string.IsNullOrWhiteSpace(request.Headers[FeatureConstants.RequestHeaderKey]))
            {
                return true;
            };
            if (!string.IsNullOrWhiteSpace(request.Headers[FeatureConstants.RequestHeaderKey.ToLower()]))
            {
                return true;
            }

            return false;
        }
    }
}
