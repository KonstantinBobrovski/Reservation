using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;

namespace Reservation.Views.Helpers
{
    public static class MyInputHelpers
    {
        public static IHtmlContent InputWithValidationMessageFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression)
        {
            var input = htmlHelper.TextBoxFor(expression,new {
                @class= "form-control"
            });
            var validationMessage = htmlHelper.ValidationMessageFor(expression);
            var label = htmlHelper.LabelFor(expression);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"mb-3 form-floating\">");
            using (var writer = new System.IO.StringWriter())
            {

                input.WriteTo(writer, HtmlEncoder.Default);
                label.WriteTo(writer, HtmlEncoder.Default);

                validationMessage.WriteTo(writer, HtmlEncoder.Default);
                stringBuilder.Append(writer.ToString());
            }
          
            stringBuilder.Append("</div>");


            return htmlHelper.Raw(stringBuilder.ToString());
        }

        public static IHtmlContent PasswordWithValidationMessageFor<TModel, TResult>(
       this IHtmlHelper<TModel> htmlHelper,
       Expression<Func<TModel, TResult>> expression)
        {

            var input = htmlHelper.PasswordFor(expression, new
            {
                @class = "form-control"
            });
            var validationMessage = htmlHelper.ValidationMessageFor(expression);
            var stringBuilder = new StringBuilder();
            var label = htmlHelper.LabelFor(expression);
            stringBuilder.Append("<div class=\"mb-3 form-floating\">");
            using (var writer = new System.IO.StringWriter())
            {

                input.WriteTo(writer, HtmlEncoder.Default);
                label.WriteTo(writer, HtmlEncoder.Default);

                validationMessage.WriteTo(writer, HtmlEncoder.Default);
                stringBuilder.Append(writer.ToString());
            }

            stringBuilder.Append("</div>");


            return htmlHelper.Raw(stringBuilder.ToString());
        }

    }


}
