using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    static class PalindromeChecker
    {
         static public bool CheckPalindrome(string Message)
        {
            for (int i = 0; i < Message.Length; i++)
                if (Message[i] != Message[Message.Length - i - 1]) return false;
            return true;
        }
    }
}
