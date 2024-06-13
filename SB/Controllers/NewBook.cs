using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SB.ViewModels;

namespace SB.Controllers
{
    public class NewBook : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult MyBooks() 
        {
            return View();
        }

        [HttpPost]

       public IActionResult Index(BookVM newBook , IFormFile[] files ) 
       {
         Book book1 = new Book {Author = newBook.Author , Title = newBook.Title ,
             Info=newBook.Info , Price = newBook.Price };

         book1.Swap= newBook.Swap== true ? 1 : 0;
            
            SwapBookDbContext db = new SwapBookDbContext();

            int[] keys = new int[db.Catalogs.Count()];

            string[] values = new string[db.Catalogs.Count()];

           
            int i = 0;
            while(i < keys.Length) 
            {
                foreach (var k in db.Catalogs)
                {
                    keys[i] = k.Id;
                    i++;
                }
                i= 0;
                foreach (var v in db.Catalogs) 
                {
                    values[i] = v.Value; i++;
                }
            }

            for (int k=0; k< values.Length; k++) 
            {
                if (values[k]== newBook.Category)
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

                    Galary galary = new Galary() {Photo = images };
                    book1.Galaries.Add(galary);
                }
            }

            
            book1.IdUser = HttpContext.Session.GetInt32("user");

            db.Books.Add(book1);
            db.SaveChanges();
            newBook.Id = book1.Id;
            return RedirectToAction("MyBooks","NewBook");
       }

    }
}
