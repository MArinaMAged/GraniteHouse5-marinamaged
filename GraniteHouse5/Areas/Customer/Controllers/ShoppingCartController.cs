using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse5.Data;
using GraniteHouse5.Extensions;
using GraniteHouse5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace GraniteHouse5.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {


        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        


        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()
            };
        }

        //Get Index Shopping Cart
        public async Task< IActionResult>  Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart.Count>0)
            {
                foreach(int carItem in lstShoppingCart)
                {
                    Products prod = _db.Products.Include(p=>p.SpecialTags).Include(p=>p.ProductTypes).Where(p => p.Id == carItem).FirstOrDefault();
                    ShoppingCartVM.Products.Add(prod);


                }
            }

            return View(ShoppingCartVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            ShoppingCartVM.Appointments.AppointmentDate = ShoppingCartVM.Appointments.AppointmentDate
                .AddHours(ShoppingCartVM.Appointments.AppointmentTime.Hour)
                .AddMinutes(ShoppingCartVM.Appointments.AppointmentTime.Minute);

            Appointments appointments = ShoppingCartVM.Appointments;
            _db.Appointments.Add(appointments);
           await _db.SaveChangesAsync();


            int appointmentId = appointments.Id;

            /* foreach(int productId in lstCartItems)
             {
                 ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                 {
                     AppointmentId = appointmentId,
                     ProductId = productId
                 };

                 _db.ProductsSelectedForAppointment.Add(productsSelectedForAppointment);
             }

             _db.SaveChanges();
             lstCartItems = new List<int>();
             HttpContext.Session.Set("ssShoppingCart", lstCartItems);

             return RedirectToAction(nameof(Index));
         }*/


            foreach (int productId in lstCartItems)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = productId
                };

                _db.ProductsSelectedForAppointment.Add(productsSelectedForAppointment);
            }

            await _db.SaveChangesAsync();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            // return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(AppointmentConfirmation), "ShoppingCart", new { Id = appointmentId });
        }


        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            { 

            if(lstCartItems.Contains(id))
            {
                lstCartItems.Remove(id);
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
        }



            return RedirectToAction(nameof(Index));


            // return RedirectToAction(nameof(AppointmentConfirmation), "ssShoppingCart", new { Id = appointmentId });
        }


        //Get :
        public IActionResult AppointmentConfirmation(int id)
        {
            ShoppingCartVM.Appointments = _db.Appointments.Where(a => a.Id == id).FirstOrDefault();
            List<ProductsSelectedForAppointment> objProdList = _db.ProductsSelectedForAppointment.Where(p => p.AppointmentId == id).ToList();

            foreach(ProductsSelectedForAppointment prodAptObj in objProdList)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.ProductTypes).Include(p => p.SpecialTags).Where(p => p.Id == prodAptObj.ProductId).FirstOrDefault());
                 
            }

            return View(ShoppingCartVM);
        }

    }
}