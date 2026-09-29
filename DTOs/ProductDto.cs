using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.DTOs
{            // use  class ProductDto when create new product 
             //                              //   & Update product
    public class ProductDto
    {

        [Required,MaxLength(100)]
        public string Name { get; set; } = "";
         
        [Required, MaxLength(100)]
        public string Brand { get; set; } = "";

        [Required, MaxLength(100)]
        public string Category { get; set; } = "";

        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = "";
       
        public IFormFile? ImageFile { get; set; }
        // When  create new product ImageFile is Requried ( must)
        // but when Update Producr ImageFile is Optional >( ? ) 

        /* X   public string ImageFileName { get; set; } = ""; X */

        /*       [Precision(16, 2)]  no need its to DataBase  DElete >  using Microsoft.EntityFrameworkCore;*/

    }








}
