using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Stratis.Bitcoin.Features.Api
{
    public class ApiWebSocket
    {
        public WebSocket WebSocket { get; set; }
        public string Username { get; set; }
    }
}
