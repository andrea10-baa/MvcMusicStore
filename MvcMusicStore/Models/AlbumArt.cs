using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace MvcMusicStore.Models
{
    public static class AlbumArt
    {
        public const string Default = "~/Content/sample-placeholder.jpeg";

        public static string Resolve(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return Default;

            // Si es una dirección externa, se usa tal cual
            if (url.StartsWith("http", System.StringComparison.OrdinalIgnoreCase)) return url;

            string path = url.StartsWith("~") ? url : "~" + (url.StartsWith("/") ? url : "/" + url);
            string physical = HostingEnvironment.MapPath(path);

            return (physical != null && File.Exists(physical)) ? path : Default;
        }
    }
}