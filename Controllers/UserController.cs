using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Services;
using ShopsApi.Data;
using ShopsApi.Models;

namespace ShopsApi.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetUser([FromServices] DataContext context)
        {
            var user = await context.Users
                .AsNoTracking()
                .ToListAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("{id:int}", Name = "UserById")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetUserById([FromServices] DataContext context, int id)
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(user);

        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<User>> CreateUser([FromServices] DataContext context, [FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                //Esconde a senha
                model.Password = "";
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel criar o usuario." });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> LoginUser([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(u => u.Username == model.Username)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuario ou senha invalidos" });

            var token = TokenService.GenerateToken(user);
            //Esconde a senha, ainda vou usar Hash
            user.Password = "";
            return new {
                user,
                token
            };
        }

        [HttpPut()]
        [Route("recovery/{id:int}")]
        [Authorize(Roles = "manager, employee")]
        public async Task<ActionResult<dynamic>> ChangePassOrUsername([FromServices] DataContext context, [FromBody] User model, int id)
        {
            //validacao id
            if (id != model.Id)
                return NotFound(new { message = "Nao foi possivel encontrar o user" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "Atualizado com sucesso!" });

            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel trocar a senha ou username" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<dynamic>> DeleteUser([FromServices] DataContext context, [FromRoute] int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound(new { message = "User Null" });

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new { message = "Deletado com sucesso!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel excluir o usuario" });
            }

        }


    }
}