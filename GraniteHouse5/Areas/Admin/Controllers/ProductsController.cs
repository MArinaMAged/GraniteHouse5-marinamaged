using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse5.Data;
using GraniteHouse5.Models;
using GraniteHouse5.Models.ViewModel;
using GraniteHouse5.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse5.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;
          [BindProperty]
         public ProductsViewModel ProductsVM { get; set; }
        public ProductsController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            ProductsVM = new ProductsViewModel()
            {
                ProductTypes = _db.ProductTypes.ToList(),
                SpecialTags = _db.SpecialTags.ToList(),
                Products = new Models.Products()
            };

        }
        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags);

            return View(await products.ToListAsync());
        }

        /* 
         //Get : PRoducts Create
         public IActionResult Create()
         {
             return View(ProductsVM);
         }
         //Post : Products Create
         [HttpPost,ActionName("Create")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreatePost(ProductsViewModel ProductsVM)
         {
         */
        /*  if(!ModelState.IsValid)
          {
              return View(ProductsVM);
          }
          _db.Products.Add(ProductsVM.Products);
          await _db.SaveChangesAsync();

          //Image being Saved


          string webRootPath = _hostingEnvironment.WebRootPath;
          var files = HttpContext.Request.Form.Files;

          var productsFromDb = _db.Products.Find(ProductsVM.Products.Id);

          if(files.Count!=0)
          {
              //Image has been uplaoded
              var uplaods = Path.Combine(webRootPath, SD.ImageFolder);
              var extension = Path.GetExtension(files[0].FileName);

              using (var filestream = new FileStream(Path.Combine(uplaods,ProductsVM.Products.Id+extension),FileMode.Create))
              {
                  files[0].CopyTo(filestream);
              }
              productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;


          }
          else
          {
              //when user does not upload image
              var uplaods = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
              System.IO.File.Copy(uplaods, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg");
              productsFromDb.Image = @"\" +SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg";
          }*/

        /*

            if (!ModelState.IsValid)
            {
                return View(ProductsVM);
            }
            _db.Products.Add(ProductsVM.Products);
            await _db.SaveChangesAsync();

            //Image being saved


            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var productsFromDb = _db.Products.Find(ProductsVM.Products.Id);


            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;

            }
            else
            {
                //when user doesnot uplaod image 
                var uplaods = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uplaods, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg";
            }




            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        } */









        //Get  : Product Create 
        public IActionResult create()
        {
            return View(ProductsVM);
        }

        //Post : Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(ProductsViewModel productsvm)
        {
            if (!ModelState.IsValid)
            {
                return View(productsvm);
            }
            _db.Products.Add(productsvm.Products);
            await _db.SaveChangesAsync();

            //Image being saved


            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var productsFromDb = _db.Products.Find(productsvm.Products.Id);


            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                //  using (var filestream = new FileStream(Path.Combine(uploads, productsvm.Products.Id + extension), FileMode.Create))

                using (var fileStream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))

                {
                    files[0].CopyTo(fileStream);
                }

                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + productsvm.Products.Id + extension;

            }
            else
            {
                //when user doesnot uplaod image 
                var uplaods = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uplaods, webRootPath + @"\" + SD.ImageFolder + @"\" + productsvm.Products.Id + ".jfif");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + productsvm.Products.Id + ".jfif";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }







       /* //Get : Edit
        public async Task<IActionResult> Edit (int? id, ProductsViewModel productsvm)
        {
            if(id==null)
            {
                return NotFound();
            }

            productsvm.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);

            if(productsvm.Products==null)
            {
                return NotFound();
            }
            return View(productsvm);
        }*/

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();

            return View(ProductsVM);
        }



        //Post : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductsViewModel productsvm)
        {
            if(ModelState.IsValid){ 
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var productFromDb = _db.Products.Where(m => m.Id == productsvm.Products.Id).FirstOrDefault();

            if(files.Count>0 && files[0]!=null)
            {
                var uplaods = Path.Combine(webRootPath, SD.ImageFolder);
                var extension_new = Path.GetExtension(files[0].FileName);
                var extension_old = Path.GetExtension(productFromDb.Image);

                if(System.IO.File.Exists(Path.Combine(uplaods,ProductsVM.Products.Id+extension_old)))
                {
                    System.IO.File.Delete(Path.Combine(uplaods, ProductsVM.Products.Id + extension_old));
                }

                using (var fileStream = new FileStream(Path.Combine(uplaods, ProductsVM.Products.Id + extension_new), FileMode.Create))

                {
                    files[0].CopyTo(fileStream);
                }

                ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + productsvm.Products.Id + extension_new;


            }

            if(ProductsVM.Products.Image !=null)
            {
                productFromDb.Image = ProductsVM.Products.Image;
            }

            productFromDb.Name = ProductsVM.Products.Name;
            productFromDb.Price = ProductsVM.Products.Price;
            productFromDb.Available = ProductsVM.Products.Available;
            productFromDb.ProductTypeId = ProductsVM.Products.ProductTypeId;
            productFromDb.SpecialTagsId = ProductsVM.Products.SpecialTagsId;
            productFromDb.ShadeColor = ProductsVM.Products.ShadeColor;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

            return View(ProductsVM);
      }





        //Get : Details 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();

            return View(ProductsVM);
        }


         //Get : Delete 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();

            return View(ProductsVM);
        }

        //Post : Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, ProductsViewModel productsvm)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Products products = await _db.Products.FindAsync(id);


            if (products == null)
            {
                return NotFound();
            }
            else
            {
                var uplaods = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(products.Image);

                if (System.IO.File.Exists(Path.Combine(uplaods, products.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uplaods, products.Id + extension));


                }

                _db.Products.Remove(products);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));


            }


        }
    }

}