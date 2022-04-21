using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class PalindromesProcessing
    {
        private SemaphoreSlim semaphore;

        const int port = 7000;
        TcpListener listener;

        public async Task ProcessingAsync(int MaxConnections)
        {
            semaphore = new SemaphoreSlim(MaxConnections, MaxConnections);

            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                //Client - класс для обработки запроса
                Client clientObject = new Client(client);

                if (semaphore.CurrentCount != 0)
                {
                    semaphore.Wait();

                    Task t = Task.Run(async () =>
                    {
                        await clientObject.Process();
                        semaphore.Release();
                    });
                }
                else
                {
                    using (var stream = client.GetStream())
                    {
                        ResponseSender responseSender = new ResponseSender();
                        PalindromeСandidate pal = new PalindromeСandidate() { Status = Status.Queue.ToString() };
                        responseSender.SendResponse(pal, stream);
                    }
                }
            }
        }
    }
}
