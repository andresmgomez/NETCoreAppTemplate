using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace UserManagementDemo.Web.Helpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ActiveRouteTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private IDictionary<string, object> _routeAttributes;


        [HtmlAttributeName("asp-area")]
        public string Area { get; set; }

        /// <summary>The name of the controller.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        /// <summary>The name of the action method.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        /// <summary>Additional parameters for the route.</summary>
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route")]
        public IDictionary<string, string> RouteAttributes
        {
            get
            {
                if (_routeAttributes == null)
                    _routeAttributes = (IDictionary<string, object>)
                        (IDictionary<string, string>)new Dictionary<string, string>(
                            StringComparer.OrdinalIgnoreCase);

                    return (IDictionary<string, string>)_routeAttributes;
                
            } 
            set
            {
                RouteAttributes = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (IsActiveRoute())
            {
                SetActiveRoute(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool IsActiveRoute()
        {
            string currentController = string.Empty;
            string currentAction = string.Empty;

            if (ViewContext.RouteData.Values["Controller"] != null)
            {
                currentController = ViewContext.RouteData.Values["Controller"].ToString();
            }
            if (ViewContext.RouteData.Values["Action"] != null)
            {
                currentAction = ViewContext.RouteData.Values["Action"].ToString();
            }
            if (Area != null) 
            {
                var value =  _contextAccessor.HttpContext.Request.Path.Value.ToLower()
                    .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(Area) && Area.ToLower() != value) 
                {
                    return false;
                }
            }

            if (Controller != null) 
            {
                if (!string.IsNullOrWhiteSpace(Controller) && Controller.ToLower() != currentController.ToLower())
                {
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(Action) && Action.ToLower() != currentAction.ToLower())
                {
                    return false;
                }
            }

            foreach (KeyValuePair<string, string> routeAttribute in RouteAttributes)
            {
                if (!ViewContext.RouteData.Values.ContainsKey(routeAttribute.Key) ||
                    ViewContext.RouteData.Values[routeAttribute.Key].ToString() != routeAttribute.Value)
                {
                    return false;
                }
            }

            return true;
        }


        private void SetActiveRoute(TagHelperOutput output)
        {
            var classAttribute = output.Attributes.FirstOrDefault(anchor => anchor.Name == "class");

            if (classAttribute == null)
            {
                classAttribute = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttribute);
            } else if (classAttribute.Value == null || classAttribute.Value.ToString().IndexOf("active") < 0)
            {
                if (classAttribute.Value.ToString() == "nav item has-treeview")
                {
                    output.Attributes.SetAttribute("class", classAttribute.Value == null
                        ? "menu-open" : classAttribute.Value.ToString() + "menu-open"); 
                } else {
                    output.Attributes.SetAttribute("class", classAttribute.Value == null
                        ? "active" : classAttribute.Value.ToString() + " active");
                }
            }

        }
    }
}
