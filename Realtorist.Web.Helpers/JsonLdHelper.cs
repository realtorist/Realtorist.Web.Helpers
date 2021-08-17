using Realtorist.Models.Helpers;
using Realtorist.Models.Settings;

namespace Realtorist.Web.Helpers
{
    /// <summary>
    /// Contains method for get JSON-LD object
    /// </summary>
    public static class JsonLdHelper
    {
        /// <summary>
        /// Gets default JSON-LD properties for website
        /// </summary>
        /// <param name="websiteSettings">Website settings</param>
        /// <param name="profile">Profile settings</param>
        /// <param name="socialSettings">Social settings</param>
        /// <returns>JSON-LD object</returns>
        public static string GetDafaultJsonLdProperties(WebsiteSettings websiteSettings, ProfileSettings profile, SocialSettings socialSettings)
        {
            return $@"
    {{
        ""@context"": ""http://schema.org/"",
        ""@type"": ""Organization"",
        ""name"": ""{profile.FullName}"",
        ""logo"": ""{websiteSettings.Logo}"",
        ""telephone"": ""{profile.Phone.FormatPhoneNumber()}"",
        ""url"": ""https://{websiteSettings.WebsiteAddress}"",
        ""sameAs"": [""{socialSettings.InstagramProfileUrl}"", ""{socialSettings.FacebookProfileUrl}""],
        ""contactPoint"": {{
            ""@type"": ""ContactPoint"",
            ""telephone"": ""{profile.Phone.FormatPhoneNumber()}"",
            ""availableLanguage"": [""en""]
        }}
    }},
    {{
        ""@context"": ""http://schema.org"",
        ""@type"": ""WebSite"", 
        ""name"": ""{websiteSettings.WebsiteName}"", 
        ""alternateName"": ""settings.WebsiteAddress"", 
        ""url"": ""https://{websiteSettings.WebsiteAddress}""
                  }}";
        }
    }
}