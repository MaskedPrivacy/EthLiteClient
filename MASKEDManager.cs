using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Nethereum.Web3;

namespace MaskedManager
{
    public class ERC20Manager
    {

        //This is the contract address of an already deployed smartcontract in the Mainnet
        private static string ContractAddress { get; set; } = "0x6824822886c5c7e09451249e700980fb4cac256f"; //our token contract

        private string FromAddr { get; set; }
        private string FromPrivKey { get; set; }
        private string ToAddr { get; set; }
        private int Amount { get; set; }

        public ERC20Manager(string From, string FromPvtKey, string To, int Qty)
        {
            this.FromAddr = From;
            this.FromPrivKey = FromPvtKey;
            this.ToAddr = To;
            this.Amount = Qty;
        }
        
              
        public async Task BalanceAsync(string WalletAddr)
        {         
            var contractAddress = ContractAddress;
            // Note: in this sample, a special INFURA API key is used
            var url = "https://mainnet.infura.io/v3/b2a3829b4de548e38f1f6c79b3d447b9";

            //no private key we are not signing anything (read only mode)
            var web3 = new Web3(url);

            var balanceOfFunctionMessage = new BalanceOfFunction()
            {
                Owner = WalletAddr,
            };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);

            Console.WriteLine("Balance of token: " + balance);
        }
        
       public async Task TransferAsync()
        {
            //Replace with your own
            var senderAddress = this.FromAddr;
            var receiverAddress = this.ToAddr;
            var privatekey = this.FromPrivKey;

            // Note: in this sample, a special INFURA API key is used
            var url = "https://mainnet.infura.io/v3/b2a3829b4de548e38f1f6c79b3d447b9";

            var web3 = new Web3(new Account(privatekey), url); //creates valid/sign account

            var transactionMessage = new TransferFunction()
            {
                FromAddress = senderAddress,
                To = receiverAddress,
                TokenAmount = 11515240000,
                //Set our own price, this should be a bit high
                Gas = 1000000,

            };

            AccountTransactionSigningInterceptor a = new AccountTransactionSigningInterceptor(privatekey, web3.Client);

            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
            var transactionHash = await transferHandler.SendRequestAsync(ContractAddress, transactionMessage);
            Console.WriteLine("Transfer txHash: " + transactionHash);
        }
    }
        
    [Function("transfer", "bool")]
    public class TransferFunction : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public string To { get; set; }

        [Parameter("uint256", "_value", 2)]
        public BigInteger TokenAmount { get; set; }
    }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }
} 
