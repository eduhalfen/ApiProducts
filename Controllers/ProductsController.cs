using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module2Edu.Data;
using Module2Edu.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Module2Edu.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {

        ProductsDbContext productsDbContext;

        public ProductsController(ProductsDbContext _productsDbContext) {
            productsDbContext = _productsDbContext;
        }


        // GET: api/products
        [HttpGet]
        public IEnumerable<Product> Get(string sortName, int? pageNumber, int? pageSize, string searchProduct)
        {
            int currentPage = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            IQueryable<Product> products;

            switch (sortName){
                case "desc":
                    products = productsDbContext.Products.OrderByDescending(p => p.ProductName);
                    break;
                case "asc":
                    products = productsDbContext.Products.OrderBy(p => p.ProductName);
                    break;
                default:
                    products = productsDbContext.Products;
                    break;                    
            }

            var productsSearch = products;
            if (searchProduct != null)
            {
                productsSearch = products.Where(p => p.ProductName.StartsWith(searchProduct));
            }
                
            var items = productsSearch.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize).ToList();
            return items;
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var product = productsDbContext.Products.SingleOrDefault(m=>m.ProductId == id);
            if (product == null){
                return NotFound("No Record found...");
            }
            return Ok(product);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            productsDbContext.Products.Add(product);
            productsDbContext.SaveChanges(true);
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            if (id!=product.ProductId){
                return BadRequest();
            }
            try {
                productsDbContext.Products.Update(product);
                productsDbContext.SaveChanges(true);    
            } catch (Exception e){
                Console.WriteLine(e);
                return NotFound("No Record Found against this Id...");
            }

            return Ok("Product Updated...");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = productsDbContext.Products.SingleOrDefault(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound("No Record found...");
            }
            productsDbContext.Products.Remove(product);
            productsDbContext.SaveChanges(true);
            return Ok("Product Deleted...");
        }
    }
}
