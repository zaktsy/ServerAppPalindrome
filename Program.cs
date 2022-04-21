using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp 
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите максимальное количество подключений");
            int MaxConnections = Int32.Parse(Console.ReadLine());
           
            Console.WriteLine("Ожидание подключений...");
            PalindromesProcessing palindromesProcessing = new PalindromesProcessing();
            await palindromesProcessing.ProcessingAsync(MaxConnections);
        }
    }
}