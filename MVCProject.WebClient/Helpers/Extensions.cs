using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCProject.WebClient {
    public static class Extensions {
        public static MvcHtmlString Table<T>(
            this HtmlHelper helper,
            List<T> data,
            string[] propertiesToSkip,
            string divClass,
            string tableClass,
            RouteInfo<T> trClickRoute,
            RouteInfo<T> editButtonRoute,
            params string[] headers
        ) {
            bool addEditButton = editButtonRoute != null;

            var properties = Utils.GetProperties<T>(propertiesToSkip);

            //<div class="divClass"></div>
            var mainDiv = new TagBuilder("div");
            mainDiv.AddCssClass(divClass);

            //<table class="tableClass">
            //  <tr></tr>
            //  <tr></tr>
            //</table>
            var table = new TagBuilder("table");
            table.AddCssClass(tableClass);

            //<tr>
            //  <th></th>
            //  <th></th>
            //</tr>
            var headerRow = new TagBuilder("tr");

            foreach (var header in headers) {
                //<th>Name</th>
                var th = new TagBuilder("th");
                th.SetInnerText(header);

                headerRow.InnerHtml += th.ToString();
            }

            if (addEditButton)
                headerRow.InnerHtml += new TagBuilder("th").ToString();

            table.InnerHtml += headerRow.ToString();

            foreach (var item in data) {
                //<tr>
                //  <td></td>
                //  <td></td>
                //</tr>
                var row = new TagBuilder("tr");
                row.MergeAttribute("onclick", $@"location.href = '{Utils.UrlHelper.Action(
                    actionName: trClickRoute.ActionName,
                    controllerName: trClickRoute.ControllerName,
                    routeValues: trClickRoute.RouteValues?.Invoke(item)
                )}'");

                foreach(var property in properties) {
                    //<td>Gustavo</td>
                    var td = new TagBuilder("td");
                    td.SetInnerText(property.GetValue(item).ToString());

                    row.InnerHtml += td.ToString();
                }

                if (addEditButton) {
                    var editButtonTD = new TagBuilder("td");
                    
                    var button = new TagBuilder("a");
                    button.AddCssClass("btn btn-primary");
                    button.MergeAttribute("href", Utils.UrlHelper.Action(
                        actionName: editButtonRoute.ActionName,
                        controllerName: editButtonRoute.ControllerName,
                        routeValues: editButtonRoute.RouteValues?.Invoke(item)
                    ));
                    button.SetInnerText("Edit");

                    editButtonTD.InnerHtml += button.ToString();
                    row.InnerHtml += editButtonTD.ToString();
                }

                table.InnerHtml += row.ToString();
            }

            mainDiv.InnerHtml += table.ToString();

            return new MvcHtmlString(mainDiv.ToString());
        }

        public static MvcHtmlString Table<T>(
            this HtmlHelper helper,
            List<T> data,
            string[] propertiesToSkip,
            string divClass,
            string tableClass,
            RouteInfo<T> trClickRoute,
            params string[] headers
        ) => helper.Table(data, propertiesToSkip, divClass, tableClass, trClickRoute, null, headers);

        public static MvcHtmlString Table<T>(
            this HtmlHelper helper,
            List<T> data,
            string[] propertiesToSkip,
            string divClass,
            string tableClass,
            params string[] headers
        ) => helper.Table(data, propertiesToSkip, divClass, tableClass, null, null, headers);
    }
}