using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using MVCProject.BLL;

namespace MVCProject.WebClient {
    public static class Utils {
        public static UrlHelper UrlHelper => new UrlHelper(HttpContext.Current.Request.RequestContext);

        public static List<SelectListItem> GetBillSortMethods() =>
            new List<SelectListItem> {
                new SelectListItem {
                    Text = "Commercialist's full name",
                    Value = BillSort.Commercialist.ToString()
                },
                new SelectListItem {
                    Text = "Credit card type",
                    Value = BillSort.CreditCardType.ToString()
                },
                new SelectListItem {
                    Text = "Date of issue",
                    Value = BillSort.Date.ToString()
                }
            };

        public static List<SelectListItem> GetSortDirections() =>
            new List<SelectListItem> {
                new SelectListItem {
                    Text = "Ascending",
                    Value = bool.TrueString
                },
                new SelectListItem {
                    Text = "Descending",
                    Value = bool.FalseString
                }
            };

        public static List<PropertyInfo> GetProperties<T>(params string[] propertyNamesToSkip) {
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

        public static object GetValue<T>(PropertyInfo prop, T item) {
            try { return prop.GetValue(item); } catch { return CheckInnerProperties(typeof(T)); }

            object CheckInnerProperties(Type type) {
                foreach (var property in type.GetProperties()) {
                    if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string)) {
                        try {
                            var inner = property.GetValue(item);

                            return prop.GetValue(inner);
                        } catch { }

                        CheckInnerProperties(property.PropertyType);
                    }
                }

                return null;
            }
        }
    }
}