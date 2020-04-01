using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sweepnet
{
    public static class Logging
    {
        public static void AddLog(String ev)
        {
#if DEBUG
            Console.WriteLine(DateTime.Now.ToString("[hh:mm:ss] dd/MM/yyyy: ") + ev);
#endif
        }
    }
}
