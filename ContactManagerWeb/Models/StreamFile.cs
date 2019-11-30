namespace ContactManagerWeb.Models
{
    public class StreamFile
    {
        public byte[] Contents { get; set; }

        public string ContentType { get; set; }
        
        public string Name { get; set; }
    }
}