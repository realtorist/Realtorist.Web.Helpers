using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Realtorist.Web.Helpers
{
    /// <summary>
    /// Contains methods to create new <see cref="IHtmlHelper"/> for the model
    /// </summary>
    public static class HtmlHelperFactoryExtensions
    {
        public static IHtmlHelper<TModel> HtmlHelperFor<TModel>(this IHtmlHelper htmlHelper)
        {
            return HtmlHelperFor(htmlHelper, default(TModel));
        }

        public static IHtmlHelper HtmlHelperFor(this IHtmlHelper htmlHelper, Type modelType)
        {
            return HtmlHelperFor(
                htmlHelper, 
                Activator.CreateInstance(modelType));
        }

        public static IHtmlHelper<TModel> HtmlHelperFor<TModel>(this IHtmlHelper htmlHelper, TModel model)
        {
            return HtmlHelperFor(htmlHelper, model, null);
        }

        public static IHtmlHelper<TModel> HtmlHelperFor<TModel>(this IHtmlHelper htmlHelper, TModel model, string htmlFieldPrefix)
        {
            ViewDataDictionary<TModel> newViewData;
            var runtimeType = htmlHelper.ViewData.ModelMetadata.ModelType;
            if (runtimeType != null && typeof(TModel) != runtimeType && typeof(TModel).IsAssignableFrom(runtimeType))
            {
                newViewData = new ViewDataDictionary<TModel>(htmlHelper.ViewData, model);
            }
            else
            {
                newViewData = new ViewDataDictionary<TModel>(htmlHelper.MetadataProvider, new ModelStateDictionary())
                {
                    Model = model
                };
            }

            if (!string.IsNullOrEmpty(htmlFieldPrefix))
                newViewData.TemplateInfo.HtmlFieldPrefix = newViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldPrefix);

            ViewContext newViewContext = new ViewContext(htmlHelper.ViewContext, htmlHelper.ViewContext.View, newViewData, htmlHelper.ViewContext.Writer);

            var newHtmlHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper<TModel>>();
            ((HtmlHelper<TModel>)newHtmlHelper).Contextualize(newViewContext);
            return newHtmlHelper;
        }
    }
}
