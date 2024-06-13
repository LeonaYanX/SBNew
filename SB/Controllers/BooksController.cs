using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SB.ViewModels;

namespace SB.Controllers
{
    public class BooksController : BaseController
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
            using (var dbContext = new SwapBookDbContext())
            {
                var categories = dbContext.Catalogs.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Value }).ToList();
                categories.First().Selected = true;

                ViewBag.Categories = categories;
            }                

            return View("CreateBook");
        }

        public IActionResult MyBooks()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(BookVM newBook, IFormFile[] files)
        {
            using (var dbContext = new SwapBookDbContext())
            {
                Book book = new Book
                {
                    Author = newBook.Author,
                    Title = newBook.Title,
                    Info = newBook.Info,
                    Price = newBook.Price,
                    IdCatalog = Convert.ToInt32(newBook.Category)
                };

                book.Swap = newBook.Swap == true ? 1 : 0;


                foreach (var f in files)
                {
                    using (var st = f.OpenReadStream())
                    {
                        byte[] images = new byte[f.Length];
                        st.Read(images, 0, images.Length);

                        Galary galary = new Galary() { Photo = images };
                        book.Galaries.Add(galary);
                    }
                }

                book.IdUser = GetUserId();

                dbContext.Books.Add(book);
                dbContext.SaveChanges();

                return RedirectToAction("Details", "Books", new { idBook = book.Id });
            }
        }

        private static string GetBase64Image(byte[] bytes)
		{
			if (bytes == null)
				return String.Empty;

			return "data:image/png;base64, " + Convert.ToBase64String(bytes);
		}
	}
}
