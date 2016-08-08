using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputRemote.Server;
using InputRemote.Server.Http;
using System.Threading;

namespace InputRemote.Server.Console
{
    class Program
    {
        static int Main()
        {
            HttpServer();
            return 0;
        }
        static void HttpServer() {
            var httpServer = new StaticHttpServer("web", 8080);
            httpServer.Start();
        }
        static void WsServer()
        {
            var server = new EmbeddedServer(233);
            server.Start();
            System.Console.ReadKey();
        }
    }
}
