using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopsApi.Data;
using ShopsApi.Models;

namespace ShopsApi.Controllers
{
    [Route("category")]

    public class CategoryController : ControllerBase
    {
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(category);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult> GetById([FromServices] DataContext context, int id)
        {
            //AsNoTracking nao vai armazenar nenhuma info.
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Post(
            [FromBody] Category model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //context.DbSet.Add()
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel criar a categoria" });
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager, employee")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
        {
            //verifica se o ID infomardo é o mesmo da Model
            if (id != model.Id)
                return BadRequest(new { message = "Nao foi possivel encontrar a Categoria" });

            //Verifica se os dados sao validos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            //return NotFound(new { message = "Macaco avantajado" });
            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Nao foi possivel atualizar a categoria" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel criar a categoria" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager, employee")]
        public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
        {
            //Se nao encontrar nada ele vai trazer nula.
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria nao encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel remover a categoria" });
            }
        }
    }
}