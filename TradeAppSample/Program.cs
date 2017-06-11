using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAppSample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine(@"引数は必ず2つ指定してください。
引数1: アクセストークン
引数2: アカウント");
            }
            string key = args[0];
            int accountId = int.Parse(args[1]);
            var app = new TradeApp(key, accountId);
            app.Run();
        }
    }
}
