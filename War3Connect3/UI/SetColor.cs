using System;
using System.Collections.Generic;
using System.Text;

namespace War3Connect3.UI
{
    public class SetColor : IDisposable
    {
        public SetColor(ConsoleColor c)
        {
            Console.ForegroundColor = c;
        }

        public void Dispose()
        {
            Console.ResetColor();
        }
    }
}
