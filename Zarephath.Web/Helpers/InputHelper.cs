using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Zarephath.Web.Helpers
{
    public static class InputHelper
    {
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool readOnly)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);

            // Add more parameters
            if (readOnly)
            { routeValueDictionary.Add("readonly", "readonly"); }

            return htmlHelper.TextBoxFor(expression, routeValueDictionary);
        }
    }
}