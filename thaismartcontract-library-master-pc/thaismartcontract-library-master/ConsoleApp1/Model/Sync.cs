namespace ConsoleApp1.Model
{

    public class Sync
    {
        public string status { get; set; }
        public int blockChainHeight { get; set; }
        public int syncPercentage { get; set; }
        public int height { get; set; }
        public object error { get; set; }
        public string type { get; set; }
    }
}
