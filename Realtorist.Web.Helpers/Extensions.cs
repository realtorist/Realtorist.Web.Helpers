using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Realtorist.Models.Helpers;
using Realtorist.Models.Listings;
using Realtorist.Models.Listings.Details;

namespace Realtorist.Web.Helpers
{
    /// <summary>
    /// Contains helpful extensions for web projects
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets absolute url for the content
        /// </summary>
        /// <param name="url">URL helper</param>
        /// <param name="path">Path to file</param>
        /// <returns>Absolute URL</returns>
        public static string ContentAbsolute(this IUrlHelper url, string path)
        {
            if (path.IsNullOrEmpty()) throw new ArgumentException($"Parameter '{nameof(path)}' shouldn't be empty");
            
            var request = url.ActionContext.HttpContext.Request;
            var urlBuilder = new System.UriBuilder()
            {
                Host = request.Host.Host,
                Scheme = request.Scheme,
                Path = url.Content(path),
                Query = null,
            };

            if (request.Host.Port.HasValue) urlBuilder.Port = request.Host.Port.Value;

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Gets address as URL argument
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Address encoded as URL argument</returns>
        public static string GetAsUrlParameter(this Address address) => WebUtility.UrlEncode($"{address.StreetAddress} {address.City}".ToLower().Replace(' ', '-'));

        /// <summary>
        /// Gets relative URL to the listing
        /// </summary>
        /// <param name="listing">Listing</param>
        /// <param name="url">URL helper</param>
        /// <returns>Relative URL</returns>
        public static string GetUrl(this Listing listing, IUrlHelper url) => url.Action("Details", "Property", new { id = listing.Id, address = listing.Address.GetAsUrlParameter() });

        /// <summary>
        /// Gets absolute URL to the listing
        /// </summary>
        /// <param name="listing">Listing</param>
        /// <param name="url">URL helper</param>
        /// <returns>Absolute URL</returns>
        public static string GetUrlAbsolute(this Listing listing, IUrlHelper url) 
        {
            var request = url.ActionContext.HttpContext.Request;
            var urlBuilder = new System.UriBuilder()
            {
                Host = request.Host.Host,
                Scheme = request.Scheme,
                Path = listing.GetUrl(url),
                Query = null,
            };

            if (request.Host.Port.HasValue) urlBuilder.Port = request.Host.Port.Value;

            return urlBuilder.ToString();
        }
    }
}