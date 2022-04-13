using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp 
{
    internal class Program
    {
        
        static int MaxConnections;
        static int Connections = 0;

        //метод-подписчик для выполнения процесса
        static void Worker(object sender, DoWorkEventArgs e)
        {
            Client client = e.Argument as Client;
            client.Process();
        }

        //метод-подписчик для выполнения действий после завершения процесса
        static void Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Work completed");
            Connections--;
        }

        const int port = 7000;
        static  TcpListener listener;
        
        static void Main(string[] args)
        {
            
            try
            {
                Console.WriteLine("Введите максимальное количество подключений");
                MaxConnections = Int32.Parse(Console.ReadLine());

                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();

                Console.WriteLine("Ожидание подключений...");

                //бесконечный цикл для принятия и обработки сообщений
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    //Client - класс для обработки запроса
                    Client clientObject = new Client(client);
                    if (Connections < MaxConnections)
                    {
                        //при получении нового запроса обрабатываем его с помощью BackgroundWorker
                        Connections++;
                        BackgroundWorker backgroundWorker = new BackgroundWorker();
                        backgroundWorker.WorkerReportsProgress = true;
                        backgroundWorker.DoWork += Worker;
                        backgroundWorker.RunWorkerCompleted += Completed;
                        backgroundWorker.RunWorkerAsync(clientObject);
                    }
                    else
                    {
                        //при недостатке свободных подключений отправляем соотвествующий ответ
                        NetworkStream stream = client.GetStream();
                        string message = "2";
                        byte[] data = new byte[2];
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                        //закрываем поток
                        stream.Close();
                        client.Close();
                    }
                        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
        
    }
}