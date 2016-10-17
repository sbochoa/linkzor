using System;
using System.Globalization;
using System.Web;
using static System.String;

namespace Linkzor {
    public class UrlProvider {
        public static string GetUrl(HttpContextBase httpContext, string scheme, string virtualPath) {
            if (IsNullOrEmpty(virtualPath)) {
                return virtualPath;
            }

            string query;
            virtualPath = TrimQueryStrings(virtualPath, out query);

            var absoluteUrl = IsNullOrEmpty(query) ? GetAbsoluteUrl(httpContext, virtualPath) : GetAbsoluteUrl(httpContext, virtualPath) + query;

            var requestUrl = httpContext.Request.Url;
            var protocol = !IsNullOrEmpty(scheme) ? scheme : Uri.UriSchemeHttp;

            if (requestUrl == null) {
                throw new InvalidOperationException($"Request url not available in this scope");
            }

            var hostName = requestUrl.Host;

            var port = Empty;
            var requestProtocol = requestUrl.Scheme;

            if (string.Equals(protocol, requestProtocol, StringComparison.OrdinalIgnoreCase)) {
                port = requestUrl.IsDefaultPort ? Empty : $":{Convert.ToString(requestUrl.Port, CultureInfo.InvariantCulture)}";
            }

            return protocol + Uri.SchemeDelimiter + hostName + port + absoluteUrl;
        }

        private static string GetAbsoluteUrl(HttpContextBase httpContext, string contentPath) {
            var relativeUrlToDestination = MakeRelative(httpContext.Request.Path, contentPath);
            var absoluteUrlToDestination = MakeAbsolute(httpContext.Request.RawUrl, relativeUrlToDestination);
            return absoluteUrlToDestination;
        }

        public static string MakeAbsolute(string basePath, string relativePath) {
            string query;
            basePath = TrimQueryStrings(basePath, out query);
            return VirtualPathUtility.Combine(basePath, relativePath);
        }

        public static string MakeRelative(string fromPath, string toPath) {
            var relativeUrl = VirtualPathUtility.MakeRelative(fromPath, toPath);
            if (IsNullOrEmpty(relativeUrl) || relativeUrl[0] == '?') {
                // Sometimes VirtualPathUtility.MakeRelative() will return an empty string when it meant to return '.',
                // but links to {empty string} are browser dependent. We replace it with an explicit path to force
                // consistency across browsers.
                relativeUrl = "./" + relativeUrl;
            }
            return relativeUrl;
        }

        private static string TrimQueryStrings(string path, out string query) {
            var queryIndex = path.IndexOf('?');
            if (queryIndex >= 0) {
                query = path.Substring(queryIndex);
                return path.Substring(0, queryIndex);
            }

            query = null;
            return path;
        }
    }
}