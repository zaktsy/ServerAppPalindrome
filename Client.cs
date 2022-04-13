using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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

        //в палиндромах сравнивается строка посимвольно, соотвественно строчные и заглавные буквы - разные символы
        private static bool IsPalindrom(string str)
        {
            for (int i = 0; i < str.Length; i++)
                if (str[i] != str[str.Length - i - 1]) return false;
            return true;
        }

        //метод получения и обработки запроса
        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[1024]; 
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    //имитация длительного процесса выполнения
                    Thread.Sleep(5000);
                    
                    Console.WriteLine(message);

                    //отправка результата
                    message = IsPalindrom(message)? "1": "0";
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
                
            }
        }
    }
}
