using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopsApi.Data;
using ShopsApi.Models;

namespace ShopsApi.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context
                .Products
                //Include faz um join, vai executar uma carga maior.
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}", Name = "ProductById")]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(products);
        }

        [HttpGet]
        [Route("category/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "manager, employee")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}