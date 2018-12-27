using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.Bitcoin.Features.Api
{
    public class ApiWebSocketFactory : IApiWebSocketFactory
    {
        List<ApiWebSocket> List;

        public ApiWebSocketFactory()
        {
            List = new List<ApiWebSocket>();
        }

        public void Add(ApiWebSocket uws)
        {
            List.Add(uws);
        }

        //when disconnect
        public void Remove(string username)
        {
            List.Remove(Client(username));
        }

        public List<ApiWebSocket> All()
        {
            return List;
        }

        public List<ApiWebSocket> Others(ApiWebSocket client)
        {
            return List.Where(c => c.Username != client.Username).ToList();
        }

        public ApiWebSocket Client(string username)
        {
            return List.First(c => c.Username == username);
        }
    }
}
