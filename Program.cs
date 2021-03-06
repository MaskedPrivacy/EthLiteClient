/*
Digital property of the Masked Privacy Group.
If you wish to copy the code for your own uses, please give credits directed to my github account, thank you  <('-')>

*/

using MaskedManager;
using System;
using System.Threading;

namespace MaskedManager
{
    class Program
    {
        static void Main(string[] args)
        {
            //We are using command line args to implement automation in future devleopment - these args could be multiple file entries specifying your sending wallet and receiver if a transfer is made.           
            string FromAddr = args[0];
            string ToAddr = args[1];
            int Amount = 0;

            try
            {
                Amount = Convert.ToInt32(args[2]);
            }
            catch
            {
                Console.WriteLine("invalid command line argument: Amount [2]");
                return;
            }

            ERC20Manager MManager = new ERC20Manager(FromAddr, "<privkey>", ToAddr, Amount);

            //Fetch wallet balances in MyWallets.txt file (one wallet hash per line in file)
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader("MyWallets.txt");
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line + ": ");
                MManager.BalanceAsync(line).Wait();
                Console.WriteLine(" ");
            }

            file.Close();
            
            //Basic transfer() example, works fine.
            MManager.SendTokens("0x089F02A3611CE62885277535926903c1b3323cD1c", 1).Wait();


            //Console.WriteLine("Transfering...");
            MManager.TransferAsync().Wait();

           MManager.HideBalance("0xMyWalletAddress");
        }
    }
}
