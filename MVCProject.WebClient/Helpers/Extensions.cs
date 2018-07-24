using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using MVCProject.BLL;

namespace MVCProject.WebClient {
    public static class Extensions {
        #region Pager Settings

        private const int maxPageCount = 10;
        private const int pagerLeft = 5;
        private const int pagerRight = 4;

        #endregion

        public static MvcHtmlString Table<T>(
            this HtmlHelper helper,
            PagedList<T> data,
            string[] propertiesToSkip,
            string rowIDProperty,
            string divClass,
            string tableClass,
            RouteInfo<T> trClickRoute,
            RouteInfo<T> editButtonRoute,
            RouteInfo<int> pagerRoute,
            params string[] headers
        ) {
            //All properties of type T
            var properties = Utils.GetProperties<T>();
            //Properties that are displayed (exclusions are in parameter propertiesToSkip)
            var displayProperties = new List<PropertyInfo>();
            //Property whose value will be put into tr's id attribute
            PropertyInfo idProperty = null;

            foreach (var prop in properties) {
                if (!propertiesToSkip.Contains(prop.Name))
                    displayProperties.Add(prop);

                if (prop.Name == rowIDProperty)
                    idProperty = prop;
            }

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

            if (editButtonRoute != null)
                headerRow.InnerHtml += new TagBuilder("th").ToString();

            table.InnerHtml += headerRow.ToString();

            foreach (var item in data.GetItemsFromCurrentPage()) {
                //<tr>
                //  <td></td>
                //  <td></td>
                //</tr>
                var row = new TagBuilder("tr");

                if (trClickRoute.Check(item))
                    row.MergeAttribute("onclick", $"location.href = '{trClickRoute.GetActionUrl(item)}'");

                if (idProperty != null)
                    row.MergeAttribute("id", Utils.GetValue(idProperty, item)?.ToString());

                foreach (var property in displayProperties) {
                    //<td>Gustavo</td>
                    var td = new TagBuilder("td");
                    td.SetInnerText(Utils.GetValue(property, item)?.ToString());

                    row.InnerHtml += td.ToString();
                }

                if (editButtonRoute.Check(item)) {
                    var editButtonTD = new TagBuilder("td");

                    var button = new TagBuilder("a");
                    button.AddCssClass("btn btn-primary");
                    button.MergeAttribute("href", editButtonRoute.GetActionUrl(item));
                    button.SetInnerText("Edit");

                    editButtonTD.InnerHtml += button.ToString();
                    row.InnerHtml += editButtonTD.ToString();
                }

                table.InnerHtml += row.ToString();
            } //foreach (<tr>)

            var pager = new TagBuilder("div");
            pager.MergeAttribute("class", "pager");

            var buttonGroup = new TagBuilder("div");
            buttonGroup.MergeAttribute("class", "btn-group");

            #region Pager Logic

            //flags whether or not to show first or last page buttons
            var left = data.CurrentPage > pagerLeft + 1;
            var right = data.PageCount > maxPageCount && (data.PageCount - data.CurrentPage) > pagerRight;

            if (left)
                pager.InnerHtml += PagerButton("<<", 1, true, pagerRoute);

            //left most button (beside first page button)
            var start = data.CurrentPage <= pagerLeft ? 1 : data.CurrentPage - pagerLeft;
            //right most button (besided last page button)
            var end = start + maxPageCount - 1;

            //testing for edge cases
            var endDiff = data.PageCount - end;

            //example: PageCount = 30, end = 35 (start = 26)
            //endDiff will be negative, that amount of pages needs to be added to the left
            if(endDiff < 0)
            {
                //add the pages to the left (endDiff is negative)
                start += endDiff; 
                //set the end to be equal to the PageCount
                end = data.PageCount; 

                //edge case for the left side
                if (start < 1)
                    start = 1;
            }

            for (int i = start; i <= end; i++)
                buttonGroup.InnerHtml += PagerButton(i, i == data.CurrentPage, pagerRoute);

            pager.InnerHtml += buttonGroup.ToString();

            if (right)
                pager.InnerHtml += PagerButton(">>", data.PageCount, true, pagerRoute);

            #endregion

            mainDiv.InnerHtml += table.ToString();
            mainDiv.InnerHtml += pager.ToString();

            return new MvcHtmlString(mainDiv.ToString());
        }

        public static MvcHtmlString Table<T>(
            this HtmlHelper helper,
            PagedList<T> data,
            string[] propertiesToSkip,
            string rowIDProperty,
            string divClass,
            string tableClass,
            RouteInfo<T> trClickRoute,
            RouteInfo<int> pagerRoute,
            params string[] headers
        ) => helper.Table(data, propertiesToSkip, rowIDProperty, divClass, tableClass, trClickRoute, null, pagerRoute, headers);

        public static MvcHtmlString Table<T>(
            this HtmlHelper helper,
            PagedList<T> data,
            string[] propertiesToSkip,
            string rowIDProperty,
            string divClass,
            string tableClass,
            RouteInfo<int> pagerRoute,
            params string[] headers
        ) => helper.Table(data, propertiesToSkip, rowIDProperty, divClass, tableClass, null, null, pagerRoute, headers);

        #region Table helpers

        private static string PagerButton(string content, int page, bool primary, RouteInfo<int> pagerRoute) {
            var button = new TagBuilder("a");

            button.MergeAttribute("class", primary
                ? "btn btn-primary"
                : "btn btn-default");

            button.SetInnerText(content);

            if (pagerRoute.Check(page))
                button.MergeAttribute("href", pagerRoute.GetActionUrl(page));

            return button.ToString();
        }

        private static string PagerButton(int page, bool primary, RouteInfo<int> pagerRoute)
            => PagerButton(page.ToString(), page, primary, pagerRoute); 

        #endregion

        public static bool Check<T>(this RouteInfo<T> routeInfo, T item) {
            if (routeInfo == null)
                return false;

            return routeInfo.PreRedirectCheck?.Invoke(item) != false;
        }
    }
}