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
    public class ResponseSender
    {
        public async Task SendResponse(PalindromeСandidate pal,Stream stream)
        {
            string jsonString = JsonSerializer.Serialize(pal);

            var data = Encoding.Unicode.GetBytes(jsonString);
            await stream.WriteAsync(data, 0, data.Length);
        }
    }
}
