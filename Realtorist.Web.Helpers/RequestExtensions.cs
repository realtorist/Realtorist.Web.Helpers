using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtorist.Web.Helpers
{
    /// <summary>
    /// Helper class with useful extensions to work with <see cref="HttpRequest"/>
    /// </summary>
    public static class RequestExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();


        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// 
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        /// <summary>
        /// Gets errors dictionary with field-error pairs for the ModelState
        /// </summary>
        /// <param name="modelState">Model state</param>
        /// <returns>Dictionary with field-error pairs for the ModelState</returns>
        public static Dictionary<string, string> GetModelStateValidationErrors(this ModelStateDictionary modelState)
        {
            if (modelState is null) throw new ArgumentNullException(nameof(modelState));
            
            var modelErrors = new Dictionary<string, string>();
            foreach (var pair in modelState)
            {
                foreach (var modelError in pair.Value.Errors)
                {
                    modelErrors.Add(pair.Key, modelError.ErrorMessage);
                }
            }
            
            return modelErrors;
        }

        /// <summary>
        /// Gets filters from the Query string
        /// </summary>
        /// <param name="request">Http Request</param>
        /// <returns>Filters</returns>
        public static Dictionary<string, string> GetFilters(this HttpRequest request)
        {
            const string expectedStart = "filter[";
            const string expectedEnd = "]";

            return request.Query.Keys
                .Where(k => k.ToLower().StartsWith(expectedStart) && k.EndsWith(expectedEnd))
                .ToDictionary(k => k.Remove(k.Length - expectedEnd.Length, expectedEnd.Length).Remove(0, expectedStart.Length), v => request.Query[v].FirstOrDefault() ?? string.Empty);
        }

        /// <summary>
        /// Writes IActionResult to the result
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="result">Result to write</param>
        /// <typeparam name="TResult">IActionResult</typeparam>
        public static Task WriteResultAsync<TResult>(this HttpContext context, TResult result) where TResult : IActionResult
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.RequestServices.GetService(typeof(IActionResultExecutor<TResult>)) as IActionResultExecutor<TResult>;

            if (executor == null)
            {
                throw new InvalidOperationException($"No result executor for '{typeof(TResult).FullName}' has been registered.");
            }

            var routeData = context.GetRouteData() ?? EmptyRouteData;

            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }
    }
}