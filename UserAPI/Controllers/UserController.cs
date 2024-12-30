using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UserAPI.DB;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly AppDBContext _dbContext;

        public UserController(IMemoryCache cache, AppDBContext dbContext)
        {
            _cache = cache;
            _dbContext = dbContext;
            //_dbContext = new AppDBContext();
        }

        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var entity = await _dbContext.Users.AsNoTracking().ToListAsync() ;

                if (entity is null || entity.Count <= 0)
                {
                    throw new Exception("Nenhum usuário encontrado");
                }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseMessage(ex.Message));
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetById([FromRoute] int id)
        {
            try
            {
                if (!_cache.TryGetValue(id, out User? entity))
                {
                    entity = await _dbContext.Users.FindAsync(id);

                    if (entity is null)
                    {
                        throw new Exception("Usuário não encontrado");
                    }

                    _cache.Set(id, entity, TimeSpan.FromMinutes(5));
                }

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseMessage(ex.Message));
            }
        }

        [HttpPost]
        [Route("novo")]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseMessage>> Post([FromBody] RequestNewUser usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.FirstName) || string.IsNullOrWhiteSpace(usuario.Email))
                    throw new Exception("Nome e email são obrigatórios");

                bool userExists = await _dbContext.Users.AnyAsync(user => user.FirstName.Equals(usuario.FirstName) || user.Email.Equals(usuario.Email));

                if (userExists)
                    throw new Exception("Usuário já existe");

                var novoUsuario = new User()
                {
                    FirstName = usuario.FirstName,
                    Email = usuario.Email
                };

                _dbContext.Add(novoUsuario);
                _dbContext.SaveChanges();

                return Created(string.Empty, new ResponseMessage("Usuário criado com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage(ex.Message));
            }
        }

        [HttpPut]
        [Route("atualizar/{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseMessage>> Put([FromRoute] int id, [FromBody] RequestUpdateUser requestUsuario)
        {
            try
            {
                var entity = await _dbContext.Users.FindAsync(id);

                if (entity is null)
                    throw new Exception("Usuário com este id não existe");

                entity.FirstName = string.IsNullOrEmpty(requestUsuario.FirstName) ? entity.FirstName : requestUsuario.FirstName;
                entity.Email = string.IsNullOrEmpty(requestUsuario.Email) ? entity.FirstName : requestUsuario.FirstName;
                entity.PhoneNumber = string.IsNullOrEmpty(requestUsuario.PhoneNumber) ? entity.PhoneNumber : requestUsuario.PhoneNumber;
                entity.Active = requestUsuario.Active;
                entity.UpdatedAt = DateTime.Now;

                _dbContext.SaveChanges();
                _cache.Remove(id);

                return Ok(new ResponseMessage("Usuário atualizado com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage(ex.Message));
            }
        }

        [HttpDelete]
        [Route("deletar/{id}")]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseMessage>> Delete([FromRoute] int id)
        {
            try
            {
                var entity = await _dbContext.Users.FindAsync(id);

                if (entity is null)
                    throw new Exception("Usuário com este id não existe");

                _dbContext.Remove(entity);
                _dbContext.SaveChanges();

                return Ok(new ResponseMessage($"Usuário '{entity.FirstName}' removido com sucesso"));
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseMessage(ex.Message));
            }
        }
    }
}
