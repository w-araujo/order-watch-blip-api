using Microsoft.AspNetCore.Mvc;
using OrderWatch.Database;
using OrderWatch.Models;

namespace OrderWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       private readonly DbConnection _dbConnection;

        public UserController(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet(Name  = "User")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _dbConnection.Set<User>().ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao listar os usuários: {ex.Message}");
            } 
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _dbConnection.Set<User>().Find(id);

                if (user == null)
                {
                    return NotFound($"Usuário com ID {id} não encontrado.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter o usuário: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            try
            {
                var emailExist = _dbConnection.Set<User>().FirstOrDefault(u => u.Email == newUser.Email);

                if (emailExist != null)
                {
                    return BadRequest($"Erro o cadastrar");
                }

                _dbConnection.Users.Add(newUser);
                _dbConnection.SaveChanges();
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar o usuário: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdatedUser")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                var existingUser = _dbConnection.Set<User>().Find(id);

                if (existingUser == null)
                {
                    return NotFound($"Usuário com ID {id} não encontrado.");
                }

                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                existingUser.Password = updatedUser.Password;
                existingUser.Birthdate = updatedUser.Birthdate;
                existingUser.UpdatedAt = DateTime.UtcNow;

                _dbConnection.SaveChanges();

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar o usuário: {ex.Message}");

            }

        }

        [HttpDelete("{id}", Name = "DeletedUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var userToDelete = _dbConnection.Set<User>().Find(id);

                if (userToDelete == null)
                {
                    return NotFound($"Usuário com ID {id} não encontrado.");
                }

                _dbConnection.Set<User>().Remove(userToDelete);
                _dbConnection.SaveChanges();

                return Ok($"Usuário com ID {id} removido com sucesso.");
            }

            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir o usuário: {ex.Message}");
            }

        }
    }
}
