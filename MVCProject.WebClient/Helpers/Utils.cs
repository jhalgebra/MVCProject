using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.WebClient {
    public static class Utils {
        public static UrlHelper UrlHelper => new UrlHelper(HttpContext.Current.Request.RequestContext);

        public static List<PropertyInfo> GetProperties<T>(string[] propertyNamesToSkip) {
            var properties = new List<PropertyInfo>();

            LookForInnerProperties(typeof(T));

            void LookForInnerProperties(Type root) {
                foreach (var property in root.GetProperties()) {
                    if (propertyNamesToSkip != null && propertyNamesToSkip.Contains(property.Name))
                        continue;

                    var type = property.PropertyType;

                    if (type.IsValueType || type == typeof(string))
                        properties.Add(property);
                    else
                        LookForInnerProperties(type);
                }
            }

            return properties;
        }
    }
}