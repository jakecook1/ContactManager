namespace ContactManagerWeb.Models
{
    public class PageNavigation
    {
        public int Index { get; set; }

        public int Pages { get; set; }

        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }
    }
}