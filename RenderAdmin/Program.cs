using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using System.Diagnostics;
using RenderAdmin;

namespace RenderAdmin
{
    class Program
    {
        private static int serverPort = 30063;
        private static int clientPort = 30064;
        class ServerImpl : RenderService.RenderServiceBase
        {
            public override Task<RestartRenderRsp> RestartRender(
                RestartRenderReq request, ServerCallContext context)
            {
                RunCmd("C://Users/Administrator/Desktop/ClearFile.bat", "");
                return Task.FromResult(new RestartRenderRsp { ResultCode = 0, ResultMsg = "begin to work" });
            }
        }
        static void Main(string[] args)
        {
            //Channel channel = new Channel();
            //var client = new RenderService.RenderServiceClient(channel);
            Server server = new Server
            {
                Services = { RenderService.BindService(new ServerImpl()) },
                Ports = { new ServerPort("localhost", serverPort, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine("gRPC server listening on port " + serverPort);
            Console.WriteLine("press any exit...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
            //Console.WriteLine("Hello World!");
        }
        static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    string str = myPro.StandardOutput.ReadToEnd();
                    Console.WriteLine(str);
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }
    }
}
