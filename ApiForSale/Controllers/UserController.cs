using Microsoft.AspNetCore.Mvc;
using ApiForSale.Data;
using ApiForSale.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiForSale.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Database context;

        public UserController(Database context)
        {
            this.context = context;
        }





        [HttpGet]
        [Route("showall")]
        public IEnumerable<User> Get()
        {
            try
            {
                return context.User.ToList();
            }
            catch(Exception ex)
            {
                return Enumerable.Empty<User>();
            }
        }




        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {

            User user;

            user = context.User.FirstOrDefault(p => p.Id == id);

            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }



        [HttpPost]
        public IActionResult Add_User([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine(user.Name);
                    Console.WriteLine(user.Password);
                    Console.WriteLine(user.Mail);
                    Console.WriteLine(user.Phone);




                    //checking if the email is repeated
                    User repeated = new User();
                    repeated = context.User.FirstOrDefault( p => p.Mail == user.Mail );

                    if(repeated is null)
                    {
                        context.User.Add(user);
                        context.SaveChanges();
                        return Ok(user.Id);
                    }
                    else
                    {
                        return Ok("The e-mail already exists in our database");
                    }
                }

                return Ok("Invalid user");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok("Invalid user");
            } 
        }


        
        [HttpPost]
        [Route("login")]
        public IActionResult Log_in([FromBody] Login user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User result = context.User.First(m => m.Mail == user.Mail && m.Password == user.Password);

                    if (result != null)
                    {
                        return Ok(result.Id);
                    }
                    else
                    {
                        return Ok(-1);
                    }

                    
                }
                
                return Ok(-1);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(-1);
            }
        }
        






        [HttpPut]
        public IActionResult Editar_Cliente([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }


    }
}
