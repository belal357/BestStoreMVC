using BestStoreMVC.DTOs;
using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
//       //    Create  Edit   Delete  



namespace BestStoreMVC.Controllers
{  // to read from  server contaner Program.cs oready there  call it> need to (ApplicationDbContext)  call it by filed in (constractor ) database
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        // newest first show 
        /* var products = context.Products.OrderByDescending(p => p.Id).ToList();  */

        public IActionResult Index()
        {//context> to read from data base/ products> name of table in data base 
            var products = context.Products.ToList();    // list of product
            // pass  list to View 
            return View(products);
        }



        public IActionResult Create()

        { return View(); }



        [HttpPost]
        public IActionResult Create(ProductDto productDto)

        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            //<<ليه بنعمل اسم للصوره اجديد ؟؟ >>  حتى لا يحدث تعارض إذا المستخدم رفع صورة نفس الاسم القديم :  <<كل صورة اسمًا مختلفًا.
            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            //C:\BestStoreMVC\wwwroot\products\20260928131856123.jpg مثلا 
            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath)) //stream هو الطريق/القناة اللي بننقل من خلالها البيانات من مكان إلى مكان.     >>>  //"أنشئ الملف في هذا المكان، وافتح لي قناة أقدر أكتب البيانات فيها."  <var stream = System.IO.File.Create(imageFullPath);
            {
                productDto.ImageFile.CopyTo(stream);
                //ت تروح إلى ProductDto > وستجد فيه Property  ..>>>public IFormFile? ImageFile { get; set; }   >> // خذ ملف الصورة الموجود داخل ImageFile وانسخه إلى stream.
            }       //productDto.ImageFile.CopyTo(stream);  >>انسخ ملف الصورة إلى المكان الذي جهزناه.





            //نسخنا الصوره  لاكن بس  (بعدها ما نحفضت ) التالي بنحفض با الداتا بيس

            // save the new product in the database    >> take the data from Productdto then >>put the data in Product
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,                                                                                                                                        //20260928131856123.jpg
                ImageFileName = newFileName,      // ImageFileName = newFileName  // نحن لا نحفظ الصورة نفسها داخل Database./  photo in wwwroot/products/20260928131856123.jpg >> وفي Database نحفظ فقط اسم الصورة:
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(product); //   >>ضف الـ product إلى جدول Products >    هنا لم يتم الحفظ النهائي بعده  . 
            context.SaveChanges();
            //نفّذ التغييرات واحفظها فعليًا في قاعدة البيانات                      //Add(Product ) >جهّز المنتج للإضافة // >> SaveChange()  >احفظه فعليًا في Database


            return RedirectToAction("Index", "Products");
      //when Finesh redirect to  index IN Product  ( برجعك لل prodcu قائمة المنتجات.)      // Redirect to list product
        }



        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");   // Redirect to list product
            }

            // create productDto from product
            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");



            return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {// save in obj Product =  if found (id 5 )

            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(productDto);
            }

            // update the image file if we have a new image >  //newFileName = laptop.jpg
            string newFileName = product.ImageFileName;   // (the old photo File >> product.ImageFileName;)
            if (productDto.ImageFile != null)
              {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
               newFileName += Path.GetExtension(productDto.ImageFile.FileName);  // newLaptop.png >> (.png )
                                                   
                                 // wwwRoot is public folder
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                  using (var stream = System.IO.File.Create(imageFullPath))
                           //use Class File ( inside > System.IO ) to create new  file in >  (imageFullPath)
                  {         // (ImageFile >> of the Recevied filed)
                    productDto.ImageFile.CopyTo(stream);
                  }

                  // delete the old image
                  string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                  System.IO.File.Delete(oldImageFullPath);
              }

             // update the product in the database ( Product Dto)
              product.Name = productDto.Name;
                product.Brand = productDto.Brand;
                 product.Category = productDto.Category;
                   product.Price = productDto.Price;
                     product.Description = productDto.Description;
                        product.ImageFileName = newFileName;

               context.SaveChanges();

             return RedirectToAction("Index", "Products");       // Redirect to list product

        }


          public IActionResult Delete(int id)
          {
               var product = context.Products.Find(id);

             if (product == null)
               {
                 return RedirectToAction("Index", "Products");  // Redirect to list product
                }

              string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
              System.IO.File.Delete(imageFullPath);

                  context.Products.Remove(product);  //Delete product from Data Base
                     context.SaveChanges(true); // save modifications

                return RedirectToAction("Index", "Products");  // // Redirect to list product
        }


    }

}
