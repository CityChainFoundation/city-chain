using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.Bitcoin.Features.Api
{
    public interface IApiWebSocketFactory
    {
        void Add(ApiWebSocket uws);
        void Remove(string username);
        List<ApiWebSocket> All();
        List<ApiWebSocket> Others(ApiWebSocket client);
        ApiWebSocket Client(string username);
    }
}
