using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SB.ViewModels;
using System.Buffers.Text;
using System.Net.WebSockets;
using static System.Reflection.Metadata.BlobBuilder;

namespace SB.Controllers
{
    public class BooksController : BaseController
    {
		public IActionResult List(int idCategory)
		{
			using (var db = new SwapBookDbContext())
			{
				var m = db.Books.Where(b => b.IdCatalog == idCategory).Include("IdCatalogNavigation")
                    .Include(c => c.Galaries)
					.ToList();
                List<BookVM> bookVMs = new List<BookVM>();

                for (int i = 0; i < m.Count(); i++)
                {
                    bookVMs.Add(new GetBookModel().GetBookVM(m[i]));
                }

                return View("BooksList", bookVMs);
			}
		}

		public IActionResult Details(int idBook)
        {
            using (var dbContext = new SwapBookDbContext())
            {
                var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation").FirstOrDefault(b => b.Id == idBook);
                var book = new GetBookModel().GetBookVM(e); 

                // добавляем изображения
                //book.Src = 

                return View("BookDetails", book);
            }
        }

        // EDIT 
        [HttpGet]
        public IActionResult Edit(int idBook )
        {
            
                using (SwapBookDbContext dbContext = new SwapBookDbContext())
                {
               

                var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation")
                        .FirstOrDefault(b => b.Id == idBook);

                var categories = dbContext.Catalogs.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Value
                    , Selected= e.IdCatalog==c.Id }).ToList();

                  ViewBag.Categories = categories;  

                var eVM = new GetBookModel().GetBookVM(e);
                     return View("EditBook" , eVM); 
                }
           
            
               
            
            
        }
        [HttpPost]
        public IActionResult Edit(BookVM eVM)
        {
            SwapBookDbContext db = new SwapBookDbContext();


            Book book = new Book() {Author= eVM.Author , Id =eVM.Id,Title=eVM.Title, Info=eVM.Info 
                , Price = eVM.Price , Swap=eVM.Swap ,IdUser =GetUserId()};
           

            book.IdCatalog = Convert.ToInt32(eVM.Category);
            

            Galary galary = new Galary();

             galary.Photo = eVM.EditPhoto;  
            
            book.Galaries.Add(galary);
           

            byte[] photoBytes = eVM.EditPhoto;

            foreach (var item in book.Galaries)
            {
                int i = 0;
                while (i < photoBytes?.Length )
                {
                    item.Photo = photoBytes;
                    i++;
                }
                
            }
                db.Books.Update(book);
            db.SaveChanges();  // 
            return RedirectToAction("Info" , "Account");
        }

       /* public IActionResult Delete(int idBook) 
        {
         SwapBookDbContext db = new SwapBookDbContext();
            db.Books.Where(i=>i.Id==idBook).
        }*/
      

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

                book.Swap = newBook.Swap;


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

            //if (bytes == null)
            //    return String.Empty;

            //return "data:image/png;base64, " + Convert.ToBase64String(bytes);
        }
    }
}
/*
 public IActionResult List(int idCategory)
		{
			using (var db = new SwapBookDbContext())
			{
				var m = db.Books.Where(b => b.IdCatalog == idCategory).Include("IdCatalogNavigation")
                    .Include(c => c.Galaries)
					.Select(e => new BookVM()
					{
						Id = e.Id,
						Title = e.Title,
						Author = e.Author,						
						Price = e.Price,
						Swap = e.Swap,
                        Category = e.IdCatalogNavigation.Value,
						Src = new string[] { GetBase64Image(e.Galaries.FirstOrDefault().Photo) },
                        Info = e.Info
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
                    Id = e.Id,
                    Title = e.Title,
                    Author = e.Author,
                    Price = e.Price,
                    Swap = e.Swap,
                    Src = GetBase64Images(e.Galaries),
                    Info = e.Info,
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

                book.Swap = newBook.Swap;


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

            //if (bytes == null)
            //    return String.Empty;

            //return "data:image/png;base64, " + Convert.ToBase64String(bytes);
        }
 */