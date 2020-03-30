//using ConsoleApp1.Model;
using NBitcoin;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thaismartcontract.WalletService;
using Thaismartcontract.WalletService.Model;
using Thaismartcontract.API;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1
{

    class Program
    {
        static HttpClient httpClient = new HttpClient();
        //static async Task Main(string[] args)
        //{
        //    Network network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
        //    //Network network = Network.Main;
        //    var generator1 = new DigibyteHDGenerator("Thai");
        //    Console.WriteLine(generator1.Mnemonic);
        //    foreach (var index in generator1.Indices)
        //    {
        //        Console.Write(index + " ");
        //    }
        //    Console.WriteLine();
        //    var generator2 = new DigibyteHDGenerator("Thai", "ค่า ขจัด ระบํา เดช ด่าน หรอก บาง นาก จดจํา ป้อง กลืน บวม เล็ง โหย นัก");
        //    Console.WriteLine("  Master Private Key : " + generator2.GetMasterKey());
        //    Console.WriteLine("  Master Public  Key : " + generator2.GetMasterPublicKey());
        //    Console.WriteLine("Extended Private Key : " + generator2.GetExtenedPrivateKey());
        //    Console.WriteLine("Extended Public  Key : " + generator2.GetExtenedPublicKey());
        //    Console.WriteLine("BIP44 Private Key 1  : " + generator2.KeyPair(0).SecretKeyWif);
        //    Console.WriteLine("BIP44 Public  Key 1  : " + generator2.KeyPair(0).PublicKeyWif);
        //    Console.ReadLine();
        //    //var mnemo = new Mnemonic("key fuel finger prosper tower medal fitness echo visit parent absurd panda lava midnight pink fun frame entire",
        //    //    Wordlist.English);
        //    //ExtKey masterKey = mnemo.DeriveExtKey();
        //    //Console.WriteLine(masterKey.ToString(network));
        //    //ExtPubKey masterPubKey = masterKey.Neuter();

        //    //var xprivateKey = masterKey.Derive(new KeyPath("m/44'/20'/0'/0"));
        //    ////var xpublicKey = masterPubKey.Derive(new KeyPath("m/44'/0'/0'"));
        //    //Console.WriteLine(xprivateKey.GetWif(network));
        //    //Console.WriteLine(xprivateKey.Neuter().GetWif(network));
        //    ////Console.WriteLine(xpublicKey.GetWif(network));
        //    //for (uint i = 0; i < 3; i++)
        //    //{
        //    //    Console.WriteLine(i);
        //    //    Console.WriteLine("Private key " + xprivateKey.Derive(i).PrivateKey.ToString(network));

        //    //    Console.WriteLine("Expected Pubkey " +xprivateKey.Derive(i).PrivateKey.PubKey.ToString(network));
        //    //    //Console.WriteLine("Public key " + xpublicKey.Derive(i).PubKey.ToString(network));
        //    //}


        //    //Task.Run(() => Test2()).Wait();
        //    //Console.ReadLine();

        //    //var testDecimal = 100000000m;
        //    //var testbyte = BitConverterExtension.GetBytes(testDecimal);
        //    //var testString = BitConverter.ToString(testbyte).Replace("-","");

        //    //var backByte = BitConverterExtension.ToDecimal(testString);
        //    //await Test2();
        //    //await Test1();
        //}


        //[DllImportAttribute("user32.dll")]
        static KeyService keyService;
        static CryptoKeyPair currentKeyPair;

        static List<Account> myAccounts = new List<Account>();
        static List<AccountService> myaccountServices = new List<AccountService>();
        static bool resetBit = false;
        static List<WalletContract> mycontracts;

        static List<WalletService> mywalletServices = new List<WalletService>();
        static ContractService contractService;
        static BitcoinSecret privateKey;
        static BitcoinPubKeyAddress publicKey;
        static WalletService walletService;
        static ContactService contactService;
        static int pageNo = 0;
        static int pageSize = 15;

        static bool isUpdated = false;
        static Ledger shownLedger;
        static byte[] PubkeyHash;
        static bool IsDone = false;
        static WalletContract resultContract;


        static async Task Main()
        {
            //Network network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            //// D9vDi1QLMwQk7J5YhUUzdys8sBbVtVVnpC
            //BitcoinSecret creator = new BitcoinSecret("KxGZX6nqKqQ7ZdFzyQMwSDMAsW1cEUZ3BRXgWQqEDNKMJhwxKMTp", network);

            //WalletContract mycontract = null;
            ////var contract = Encoding.UTF8.GetBytes("PIGER");
            ////var token = Encoding.UTF8.GetBytes("PIG");
            ////mycontract = await contractService.CreateContract(creator, contract, token, 100000m, 2);
            ////Console.WriteLine(mycontract.ID);
            ////Console.WriteLine("Contract is issued: " + mycontract.ID);
            //var api = new DigibyteAPI(new APIOptions { BaseURL = "https://digibyteblockexplorer.com" });
            //var contractService = new ContractService(@"tsc-wallet.db", api);
            //mycontract = await contractService.FindContract("3ecb54e7c2551f27cf5944e386f05b27e1f0e4d7222d2cb2e8e784ef6560e30f");
            //var walletService = contractService.CreateWalletService(mycontract, creator);
            //var ledger1 = walletService.CreateLedger(OperationCode.Issue);
            //await walletService.BroadcastLedger(ledger1);

            //Console.WriteLine(ledger1.TxId);
            //Console.ReadLine();
            // D5AFMqnpcmFh9GyC8UcPCMupStiAN3jMwX
            //BitcoinSecret user = new BitcoinSecret("L3DYWvGyVZuFiqEMDRgBuRJKvutssRczXmTxxUA9piJkq3LSQZAn", network);
            //var contractService = new ContractService(@"tsc-wallet.db", api);
            //WalletContract mycontract = null;
            //mycontract = await contractService.FindContract("165dc1f670fb2c89046f47de718c4818d6c47fefa3ba152ed914fd82ae7b29f1");
            //if (mycontract == null)
            //keyService = new KeyService("1234");
            //var result = keyService.ParseSeed("นิ้ว ร่วง สมัย ทักษะ กวาด กว่า แอร์ น้ําใจ ล่ะ หมวย ทูล เลิศ ดาว สมรส เจอร์");
            //if (result != null)
            //{
            //    Console.WriteLine(result.Seed + " and " + result.SecretKeyWif);
            //    currentKeyPair = result;
            //    keyService.SaveKey(currentKeyPair);
            //}
            //else
            //{
            //    Console.WriteLine("Retry");
            //}

            //contractService = new ContractService(@"tsc-wallet.db", new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));   //รุบุที่อยู่รหัสสัญญา

            //var walletContract = await contractService.FindContract("165dc1f670fb2c89046f47de718c4818d6c47fefa3ba152ed914fd82ae7b29f1"); //เตรียมรหัสสัญญา

            //string _contract = "165dc1f670fb2c89046f47de718c4818d6c47fefa3ba152ed914fd82ae7b29f1";
            //mycontracts = contractService.FindLocalContract();
            //foreach(var contract in mycontracts)
            //{
            //    if(contract.ID == _contract)
            //    {
            //        Console.WriteLine("seen!!");
            //        continue;
            //    }
            //}


            //string combildtext = "abc?def;ghi";
            //string combildtext = "abc?def";
            //string combildtext = "abc;ghi";
            //string combildtext = "abc";
            //int point1 = combildtext.IndexOf('?');
            //int point2 = combildtext.IndexOf(';');

            //string publickey = "";
            //string amount = "";
            //string contract = "";
            ////Console.WriteLine(point1+"\n"+ point2);
            //if (point1 != -1 && point2 != -1)
            //{
            //    publickey = combildtext.Substring(0, point1);
            //    amount = combildtext.Substring(point1 + 1, point2 - (point1 + 1));
            //    contract = combildtext.Substring(point2 + 1);
            //}
            //else if (point1 != -1 && point2 == -1)
            //{
            //    publickey = combildtext.Substring(0, point1);
            //    amount = combildtext.Substring(point1 + 1);
            //}
            //else if (point1 == -1 && point2 != -1)
            //{
            //    publickey = combildtext.Substring(0, point2);
            //    contract = combildtext.Substring(point2 + 1);
            //}
            //else
            //{
            //    publickey = combildtext.Substring(0);
            //}
            //Console.WriteLine(publickey + "\n" + amount + "\n" + contract);

            //try
            //{
            //    keyService = new KeyService("12341");
            //    var key = keyService.GetKey();
            //}
            //catch
            //{

            //}
            keyService = new KeyService("1234");
            var savedKey = keyService.GetKey();
            //privateKey = savedKey.SecretKey;
            //PubkeyHash = savedKey.PublicKey.Hash.ToBytes();

            //Console.WriteLine(savedKey.SecretKey);



            contactService = new ContactService();
            //contractService = new ContractService(@"tsc-wallet.db", new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));   //รุบุที่อยู่รหัสสัญญา
            contractService = new ContractService(@"tsc-wallet.db", new DigibyteAPI(new APIOptions() { BaseURL = "https://explorer.thaismartcontract.com" }));
            Network network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
            BitcoinSecret creator = new BitcoinSecret("L3DYWvGyVZuFiqEMDRgBuRJKvutssRczXmTxxUA9piJkq3LSQZAn", network);
            var mycontracts = await contractService.FindContract("19d625fff665bb2a8ffae5b0b960113ee5b98fd23fb4fca2c95d1c1380ebfaa3");
            var walletService = contractService.CreateWalletService(mycontracts, creator);
            var ledger1 = walletService.CreateLedger(OperationCode.Issue);
            await walletService.BroadcastLedger(ledger1);
            Console.Write(ledger1.TxId);


            //var a = new DigibyteAPI(new APIOptions() { BaseURL = "https://explorer.thaismartcontract.com" });
            //var b = await a.GetBlockHeight("8836df00d125574303418e44ce0fe6316e54cfbf15799fc79a68c47171188ffd");
            //var c = await a.GetSync();

            //Console.WriteLine(b);
            //Console.WriteLine(c.blockChainHeight);






            //try
            //{
            //    System.Net.WebClient client = new System.Net.WebClient();
            //    string result = client.DownloadString("http://172.16.195.8:3000/api/sync");
            //    Console.WriteLine("Connect");
            //}
            //catch (System.Net.WebException ex)
            //{
            //    //do something here to make the site unusable, e.g:
            //    Console.WriteLine("Disconnect");

            //}
            //try
            //{
            //    using (var client = new TcpClient("172.16.195.8", 3000))
            //        Console.WriteLine("Connected");
            //}
            //catch (SocketException ex)
            //{
            //    Console.WriteLine("DisConnected");
            //}


            //mycontracts = contractService.FindLocalContract();



            //if (mywalletServices != null && mywalletServices.Count() > 0)
            //{
            //    foreach (var service in mywalletServices)
            //    {
            //        if (await service.IsNewTransactionAvailable())
            //            isUpdated = true;
            //    }
            //}
            //else
            //{
            //    isUpdated = true;
            //}

            //if (isUpdated)
            //{
            //    mywalletServices.Clear();
            //    myaccountServices.Clear();
            //    myAccounts.Clear();
            //    mycontracts = contractService.FindLocalContract();
            //    foreach (var con in mycontracts)
            //    {
            //        mywalletServices.Add(contractService.CreateWalletService(con, privateKey));
            //    }

            //    foreach (var service in mywalletServices)
            //    {
            //        await service.Rescan(resetBit);
            //        myaccountServices.Add(service.GetAccountService());
            //    }
            //}

            //    foreach (var service in myaccountServices)
            //    {

            //        myAccounts.Add(service.GetAccount(privateKey.GetAddress()));
            //    }

            //    try
            //    {

            //        foreach (var account in myAccounts)
            //        {
            //            var currentContract = mycontracts.First(c => c.NameString == account.TokenName);
            //            string balance = account.Balance.ToString("N" + currentContract.NoOfDecimal.ToString());
            //            Console.WriteLine(balance);


            //        }
            //    }
            //    catch
            //    {

            //    }

            //}
            //    var allLedgers = mywalletServices.SelectMany(ws => ws.GetMyLedgers(limit: 5)).ToList();

            //    if (shownLedger != null)
            //    {
            //        if (shownLedger.TxId != allLedgers[0].TxId)
            //        {

            //        }
            //    }

            //    shownLedger = allLedgers[0];
            //    foreach (var ledger in allLedgers)
            //    {
            //        if (!ledger.TokenSenderHash.QuickCompare(PubkeyHash) && !ledger.TokenReceiverHash.QuickCompare(PubkeyHash))
            //        {
            //            continue;
            //        }

            //        if (ledger.Status != ProcessStatus.Processed)
            //        {
            //            continue;
            //        }

            //        var currentContract = mycontracts.First(c => c.TokenName.QuickCompare(ledger.TokenName));

            //    }

            //    resetBit = false;

            //    isUpdated = false;
            //}
            //catch (Exception e)
            //{
            //    //if (Program.CheckForInternetConnection())
            //    //{
            //    //    resetBit = true;
            //    //}
            //    //else
            //    //{
            //    //    Thread.Sleep(15000);
            //    //}
            //    //goto restart;



            //IsDone = true;




            //foreach (var service in myaccountServices)
            //{

            //    myAccounts.Add(service.GetAccount(privateKey.GetAddress()));
            //}
            //foreach (var account in myAccounts)
            //{
            //    var currentContract = mycontracts.First(c => c.NameString == account.TokenName);
            //    Console.WriteLine(account.Balance.ToString("N" + currentContract.NoOfDecimal.ToString()));
            //    Console.WriteLine(account.Balance);
            //}
            //var allLedgers = mywalletServices.SelectMany(ws => ws.GetMyLedgers(limit: 5)).ToList();
            //foreach (var ledger in allLedgers)
            //{
            //    Console.WriteLine(contactService.GetContact(ledger.TokenSenderHashHex));
            //}

            //var savedKey = keyService.GetKey();
            ////Console.WriteLine(savedKey.Seed);
            //privateKey = savedKey.SecretKey;

            //contractService = new ContractService(@"tsc-wallet.db", new DigibyteAPI(new APIOptions() { BaseURL = "https://digibyteblockexplorer.com" }));   //รุบุที่อยู่รหัสสัญญา
            //var walletContract = await contractService.FindContract("165dc1f670fb2c89046f47de718c4818d6c47fefa3ba152ed914fd82ae7b29f1"); //เตรียมรหัสสัญญา
            //var walletService = contractService.CreateWalletService(walletContract, privateKey);    //การเตรียมการใช้งาน
            //var pb = privateKey.GetAddress();
            // var skip = pageNo * pageSize;

            //mycontracts = contractService.FindLocalContract();
            //foreach (var con in mycontracts)
            //{
            //    mywalletServices.Add(contractService.CreateWalletService(con, privateKey));
            //}
            //contactService = new ContactService();
            //var allLedgers = mywalletServices.SelectMany(ws => ws.GetMyLedgers(limit: 5)).ToList();

            //var ledger2 = walletService.CreateLedger(OperationCode.Transfer, "D5AFMqnpcmFh9GyC8UcPCMupStiAN3jMwX", 500m);
            //await walletService.BroadcastLedger(ledger2, false);
            //Console.WriteLine(ledger2.TxId);
            //Console.WriteLine("Press enter to continue...");
            //Console.ReadLine();
            //await ownerWalletService.BroadcastLedger(ledger2, false);
            //allLedgers.ForEach(i => Console.Write("{0}\t", i));

            //var ledgers = walletService.GetLedgers(pb.Hash.ToString(), skip, pageSize);
            //ledgers.ForEach(i => Console.Write("{0}\t", i));
            //ledgers.ForEach(l => displayLedgers.Add(l.AsVieweable(contactService)));

            //Console.WriteLine(contactService.GetContact(allLedgers[0].TokenSenderHashHex));





            //WalletContract mycontract = null;
            //var contract = Encoding.UTF8.GetBytes("PIGER");
            //    var token = Encoding.UTF8.GetBytes("PIG");
            //mycontract = await contractService.CreateContract(creator, contract, token, 100000m, 2);
            //    Console.WriteLine(mycontract.ID);
            //Console.WriteLine("Contract is issued: " + mycontract.ID);






            //{
            //    var contract = Encoding.UTF8.GetBytes("SCIENCE");
            //    var token = Encoding.UTF8.GetBytes("SCI");
            //    mycontract = await contractService.CreateContract(creator, contract, token, 1000000m, 2);
            //    Console.WriteLine(mycontract.ID);
            //    Console.ReadLine();
            //}
            //{
            //    var contract = Encoding.UTF8.GetBytes("RMUTSB");
            //    var token = Encoding.UTF8.GetBytes("ITS");
            //    mycontract = await contractService.CreateContract(creator, contract, token, 1000m, 2);
            //    Console.WriteLine(mycontract.ID);
            //    Console.ReadLine();
            //}


            //WriteData<WalletContract>(mycontract);
            ////Console.WriteLine("Press enter to continue...");
            //Console.ReadLine();
            //var firstUser = creator;
            //var ownerWalletService = contractService.CreateWalletService(mycontract, firstUser);
            //await ownerWalletService.Rescan(false);
            //var secondUser = user;
            //var userWalletService = contractService.CreateWalletService(mycontract, secondUser);
            //await userWalletService.Rescan(false);

            /// validate if future fee is transfered to client or not


            //var ledger1 = ownerWalletService.CreateLedger(OperationCode.Issue);
            //await ownerWalletService.BroadcastLedger(ledger1, false);
            //Console.WriteLine(ledger1.TxId);
            //Console.WriteLine("Press enter to continue...");
            //Console.ReadLine();
            //var ledger2 = ownerWalletService.CreateLedger(OperationCode.Transfer, secondUser.GetAddress(), 1000m);
            //await ownerWalletService.BroadcastLedger(ledger2, false);
            //Console.WriteLine(ledger2.TxId);
            //Console.WriteLine("Press enter to continue...");
            //Console.ReadLine();

            //var ledger3 = userWalletService.CreateLedger(OperationCode.Transfer, firstUser.GetAddress(), 500m);
            //await userWalletService.BroadcastLedger(ledger3, false);
            //Console.WriteLine(ledger3.TxId);
            //Console.WriteLine("Press enter to continue...");
            //Console.ReadLine();


            //await userWalletService.Rescan(false);
            //Console.WriteLine("User Wallet");
            //var accountManager = userWalletService.GetAccountService();
            //var firstAccount = accountManager.GetAccount(firstUser);
            //var secondAccount = accountManager.GetAccount(secondUser);
            //Console.WriteLine(firstAccount);
            //Console.WriteLine(secondAccount);
            //Console.WriteLine("Owner Wallet");
            //await ownerWalletService.Rescan(false);
            //accountManager = ownerWalletService.GetAccountService();
            //firstAccount = accountManager.GetAccount(firstUser);
            //secondAccount = accountManager.GetAccount(secondUser);
            //Console.WriteLine(firstAccount);
            //Console.WriteLine(secondAccount);
            //Console.ReadLine();


            //testLedger = await walletService.WriteLedger(creator, creator.GetAddress(), testLedger);

            //var mycontract = contractService.CreateContract(creator, contract, token, 100000000m, 8);


            //var transferLedger = new Ledger()
            //{
            //    Operation = OperationCode.Transfer,
            //    Amount = 1000m
            //};

            //transferLedger = await walletService.WriteLedger(creator, user.GetAddress(), transferLedger);

            //var userWalletService = contractService.CreateWalletService(user);
            //await userWalletService.Rescan(true);
            //Console.WriteLine(mycontract);
            //var mycontract = contractService.FindContract("59b900e905aa60130eca5a4c6a49fc30d3767b90014acecebdb521e773e1b350");

        }


        private static void WriteData<T>(T obj)
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "backupdata.txt"), JsonConvert.SerializeObject(obj));
        }
        //static async Task Test1()
        //{
        //    httpClient.BaseAddress = new Uri("http://192.168.2.150:3001");
        //    //httpClient.BaseAddress = new Uri("https://digiexplorer.info");
        //    Transaction tx;
        //    //Console.WriteLine(tsc.PubKey.GetAddress(network).ToString());

        //    // KxTMz3WW2RZR31dXmRcBZ8sHpbqikTAbBdSD1ecr98ZcxACvtuuX
        //    // DBBbNQPf6fewuZj4vpYgaG16HauxNhrNsi

        //    // KzvsQ3QPeoBmi1DquLdVnQFfhu55HZj3XxDLSHtBdoBbWtsaXkJt
        //    // D865wpB8q8yb8xNXSxrgbtdr4sgV8aQ1FL
        //    Network network = NBitcoin.Altcoins.Digibyte.Instance.Mainnet;
        //    BitcoinSecret sender = new BitcoinSecret("KxTMz3WW2RZR31dXmRcBZ8sHpbqikTAbBdSD1ecr98ZcxACvtuuX", network);
        //    var oohCoinomi = new BitcoinPubKeyAddress("D865wpB8q8yb8xNXSxrgbtdr4sgV8aQ1FL ", network);

        //    var aa = oohCoinomi.Hash.ToBytes();
        //    var bb = oohCoinomi.Hash.ToString();
        //    //var a = sender.PubKey.GetScriptAddress(network);
        //    //var aa = sender.PubKey.ToBytes();
        //    //SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
        //    //byte[] shaofcompressedpubkey = sha256.ComputeHash(aa);
        //    //var witnessProgram = new byte[20];
        //    //RipeMD160Digest rip = new RipeMD160Digest();
        //    //rip.BlockUpdate(shaofcompressedpubkey, 0, shaofcompressedpubkey.Length);
        //    //rip.DoFinal(witnessProgram, 0);
        //    //var ws = BitConverter.ToString(witnessProgram).Replace("-", "");
        //    //var b = sender.PubKeyHash.ToString();
        //    //var c = sender.ScriptPubKey;
        //    //var d = sender.PubKey.ToHex();
        //    //var e = oohCoinomi.Hash.ToString();

        //    var timeLock = new Script(
        //        Op.GetPushOp(Utils.DateTimeToUnixTime(DateTimeOffset.UtcNow.AddMinutes(15))),
        //        OpcodeType.OP_CHECKLOCKTIMEVERIFY,
        //        OpcodeType.OP_DROP,
        //        OpcodeType.OP_DUP,
        //        OpcodeType.OP_HASH160,
        //        Op.GetPushOp(oohCoinomi.Hash.ToBytes()),
        //        OpcodeType.OP_EQUALVERIFY,
        //        OpcodeType.OP_CHECKSIG
        //    );
        //    Console.WriteLine(timeLock.ToString());
        //    var myTransaction = Transaction.Create(network);

        //    var fee = new Money(1000);
        //    var allUtxo = await GetReceivedCoinFromDigibyte(sender.GetAddress().ToString());
        //    var utxo = allUtxo.OrderByDescending(u => u.amount).First().ToCoin();
        //    //var utxo = allUtxo[0].ToCoin();

        //    var txBuilder = network.CreateTransactionBuilder();
        //    for (int i = 0; i < allUtxo.Count(); i++)
        //    {
        //        txBuilder = txBuilder.AddCoins(allUtxo[i].ToCoin());
        //    }



        //    tx = txBuilder
        //        .AddKeys(sender)
        //        .Send(oohCoinomi, new Money(1000))
        //        .Send(timeLock, new Money(1234))
        //        //.Send(new Script("OP_RETURN 84727383327383327079823284698384737871"), Money.Zero)
        //        .SetChange(sender.GetAddress())
        //        .SendFees(fee)
        //        .BuildTransaction(true);

        //    Console.WriteLine(tx.ToString());

        //    var verified = txBuilder.Verify(tx);
        //    //var txid = await BroadcastTransaction(tx.ToHex());
        //    Console.WriteLine(tx.ToString());
        //    Console.ReadLine();
        //}


        //static async Task<List<UTXO>> GetReceivedCoinFromDigibyte(string address)
        //{
        //    var result = await httpClient.GetStringAsync($"/api/addr/{address}/utxo");
        //    return JsonConvert.DeserializeObject<List<UTXO>>(result);
        //}

        //static async Task<TxID> BroadcastTransaction(string hex)
        //{
        //    var data = new RawTX()
        //    {
        //        rawtx = hex
        //    };
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    var result = await httpClient.PostAsync($"/api/tx/send", content);
        //    result.EnsureSuccessStatusCode();
        //    var output = await result.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<TxID>(output);
        //}

    }
}
