namespace SB.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Author { get; set; }

        public int? Price { get; set; }

        public bool Swap { get; set; }
      
        public string[] Src { get; set; }
        public string Category { get; set; }

        public string? Info { get; set; }

    }
}
