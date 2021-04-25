namespace TestTask.Domain
{
    public class Crawled
    {
        public string Url { get; set; }
        public int Result { get; set; }
        public ProcessStatus Status { get; set; }
    }
}
