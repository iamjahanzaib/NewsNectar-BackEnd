using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CategoryController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]

        public async Task<ActionResult<List<Category>>> GetAllCategory()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Category> category = await GetAllCategory(connection);
            return Ok(category);
        }

        private async Task<IEnumerable<Category>> GetAllCategory(SqlConnection connection)
        {
            return await connection.QueryAsync<Category>("select * from category");
        }
    }
}
