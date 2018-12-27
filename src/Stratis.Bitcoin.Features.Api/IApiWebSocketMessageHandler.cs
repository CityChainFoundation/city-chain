using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Stratis.Bitcoin.Features.Api
{
    public interface IApiWebSocketMessageHandler
    {
        Task SendInitialMessages(ApiWebSocket userWebSocket);
        Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, ApiWebSocket userWebSocket, IApiWebSocketFactory wsFactory);
        Task BroadcastOthers(byte[] buffer, ApiWebSocket userWebSocket, IApiWebSocketFactory wsFactory);
        Task BroadcastAll(byte[] buffer, ApiWebSocket userWebSocket, IApiWebSocketFactory wsFactory);
    }
}
