using System;

namespace MVCProject.WebClient {
    public class RouteInfo<T> {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Func<T, object> RouteValues { get; set; }
    }
}