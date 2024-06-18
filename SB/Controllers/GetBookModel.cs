using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SB.ViewModels;

namespace SB.Controllers
{
    public class GetBookModel : Controller
    {
        private static string[] GetBase64Images(ICollection<Galary> galaries)
        {
            List<string> images = new List<string>();

            foreach (Galary g in galaries)
            {
                if (g.Photo != null)
                {
                    images.Add("data:image/png;base64, " + Convert.ToBase64String(g.Photo));
                }
            }

            return images.ToArray();

        }
            private static string GetBase64Image(byte[] bytes)
        {
            if (bytes == null)
                return String.Empty;

            return "data:image/png;base64, " + Convert.ToBase64String(bytes);
        }
        public BookVM  GetBookVM(Book book) 
        {
            string[] srcs = GetBase64Images(book.Galaries);

            SwapBookDbContext db = new SwapBookDbContext();

            BookVM bookVM = new BookVM()

            {
                Id = book.Id,
                Author = book?.Author,
                Category = book?.IdCatalogNavigation?.Value.ToString()
                ,
                Info = book?.Info,
                Price = book?.Price,
                Swap = book.Swap,
                Title = book?.Title ,
                Src = srcs
            };

            return bookVM;
        }
    }
}
