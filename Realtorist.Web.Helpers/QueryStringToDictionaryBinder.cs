using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Realtorist.Web.Helpers
{
    /// <summary>
    /// Provides binding for the <see cref="Dictionary{string, string}" /> from query params
    /// </summary>
    public class QueryStringToDictionaryBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var query = bindingContext.HttpContext.Request.Query;
            var expectedStart = bindingContext.FieldName.ToLower() + "[";
            const string expectedEnd = "]";

            var dictionary = query.Keys
                .Where(k => k.ToLower().StartsWith(expectedStart) && k.EndsWith(expectedEnd))
                .ToDictionary(k => k.Remove(k.Length - expectedEnd.Length, expectedEnd.Length).Remove(0, expectedStart.Length), v => query[v].FirstOrDefault() ?? string.Empty);

            bindingContext.Result = ModelBindingResult.Success(dictionary);
            return Task.CompletedTask;
        }
    }
}