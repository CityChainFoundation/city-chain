using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Stratis.Bitcoin.Features.WebSocket
{
    public class ControllerMethod
    {
        public Type Type { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Attributes { get; set; }

        public string ReturnType { get; set; }

        public ParameterInfo[] Parameters { get; set; }
    }

    public class ControllerMethodList
    {
        public List<ControllerMethod> List = new List<ControllerMethod>();
    }
}
