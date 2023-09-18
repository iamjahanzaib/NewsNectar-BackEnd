using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]

        public async Task<ActionResult<List<Users>>> GetAllUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Users> users = await GetAllUsers(connection);
            return Ok(users);
        }

        [HttpGet("{userId}")]

        public async Task<ActionResult<Users>> GetUser(int userId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QueryFirstAsync<Users>("select * from users where id = @Id",
                new {Id = userId});
            return Ok(user);
        }

        [HttpPost]

        public async Task<ActionResult<List<Users>>> AddUser(Users user)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into users (id, name, lastName, address) values (@Id, @Name, @LastName, @Address)", user);
            return Ok(await GetAllUsers(connection));
        }



        private static async Task<IEnumerable<Users>> GetAllUsers(SqlConnection connection)
        {
            return await connection.QueryAsync<Users>("select * from users");
        }
    }
}
