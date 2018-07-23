using System;

namespace MVCProject.WebClient
{
    public class RouteInfo<T>
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Func<T, object> RouteValues { get; set; }
        public Func<T, bool> PreRedirectCheck { get; set; }

        public string GetActionUrl(T item) => Utils.UrlHelper.Action(
            actionName: ActionName,
            controllerName: ControllerName,
            routeValues: RouteValues?.Invoke(item)
        );
    }
}