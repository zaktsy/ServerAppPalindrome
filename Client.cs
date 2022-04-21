using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp
{
    public class Client
    {
        public TcpClient client;
        public Client(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        //метод получения и обработки запроса
        public async Task Process()
        {
            try
            {
                string message = String.Empty;
                using (var stream = client.GetStream())
                {
                    byte[] data = new byte[1024];
                    await stream.ReadAsync(data, 0, data.Length);
                    string resp = Encoding.Unicode.GetString(data);
                    resp = resp.Replace("\0", "");
                    PalindromeСandidate pal = JsonSerializer.Deserialize<PalindromeСandidate>(resp);


                    Console.WriteLine("Получено: " + pal.Text);

                    //имитация длительного процесса выполнения
                    Thread.Sleep(5000);
                    
                    Console.WriteLine("Закончена обработка: " + pal.Text);


                    if (client.Connected)
                    {
                        Status status = PalindromeChecker.CheckPalindrome(pal.Text) ? Status.Yes : Status.No;
                        pal.Status = status.ToString();
                        ResponseSender responseSender = new ResponseSender();
                        responseSender.SendResponse(pal, stream);
                    } 
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                }
            }
        }
    }
}
