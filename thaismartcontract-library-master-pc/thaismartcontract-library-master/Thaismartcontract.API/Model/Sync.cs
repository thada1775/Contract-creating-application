namespace Thaismartcontract.API.Model
{

    public class Sync
    {
        public string status { get; set; }
        public int blockChainHeight { get; set; }
        public decimal syncPercentage { get; set; }
        public int height { get; set; }
        public object error { get; set; }
        public string type { get; set; }
    }
}
