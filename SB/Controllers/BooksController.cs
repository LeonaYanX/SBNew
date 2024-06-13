using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SB.ViewModels;

namespace SB.Controllers
{
    public class BooksController : Controller
    {
		public IActionResult List(int idCategory)
		{
			using (var db = new SwapBookDbContext())
			{
				var m = db.Books.Where(b => b.IdCatalog == idCategory)
					.Include(c => c.Galaries)
					.Select(e => new BookVM()
					{
						Id = e.Id,
						Title = e.Title,
						Author = e.Author,
						Info = e.Info,
						Price = e.Price,
						Swap = (e.Swap == 1 ? true : false),
						Src = new string[] { GetBase64Image(e.Galaries.FirstOrDefault().Photo) }
					}).ToList();


				return View("BooksList", m);
			}
		}

		public IActionResult Details(int idBook)
        {
            using (var dbContext = new SwapBookDbContext())
            {
                var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation").FirstOrDefault(b => b.Id == idBook);
                var book = new BookVM()
                {
                    Author = e.Author,
                    Price = e.Price,
                    Info = e.Info,
                    Swap = (e.Swap == 1 ? true : false),
                    Title = e.Title,
                    Category = (e.IdCatalogNavigation?.Value) ?? "Not Specified"
                };

                // добавляем изображения
                //book.Src = 

                return View("BookDetails", book);
            }
        }

        public IActionResult Edit(int idBook)
        {

            return View("EditBook");
        }

        public IActionResult Create()
        {
            return View("CreateBook");
        }

        public IActionResult MyBooks()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(BookVM newBook, IFormFile[] files)
        {
            Book book1 = new Book
            {
                Author = newBook.Author,
                Title = newBook.Title,
                Info = newBook.Info,
                Price = newBook.Price
            };

            book1.Swap = newBook.Swap == true ? 1 : 0;

            SwapBookDbContext db = new SwapBookDbContext();

            int[] keys = new int[db.Catalogs.Count()];

            string[] values = new string[db.Catalogs.Count()];


            int i = 0;
            while (i < keys.Length)
            {
                foreach (var k in db.Catalogs)
                {
                    keys[i] = k.Id;
                    i++;
                }
                i = 0;
                foreach (var v in db.Catalogs)
                {
                    values[i] = v.Value; i++;
                }
            }

            for (int k = 0; k < values.Length; k++)
            {
                if (values[k] == newBook.Category)
                {
                    book1.IdCatalog = keys[k];
                    break;
                }
            }

            foreach (var f in files)
            {
                using (var st = f.OpenReadStream())
                {
                    byte[] images = new byte[f.Length];
                    st.Read(images, 0, images.Length);

                    Galary galary = new Galary() { Photo = images };
                    book1.Galaries.Add(galary);
                }
            }


            book1.IdUser = HttpContext.Session.GetInt32("user");

            db.Books.Add(book1);
            db.SaveChanges();
            newBook.Id = book1.Id;
            return RedirectToAction("MyBooks", "NewBook");
        }

        private static string GetBase64Image(byte[] bytes)
		{
			if (bytes == null)
				return String.Empty;

			return "data:image/png;base64, " + Convert.ToBase64String(bytes);
		}
	}
}
