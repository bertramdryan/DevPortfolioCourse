using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;

        public CategoriesController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Category> categories = await _appDbContext.Categories.ToListAsync();

            return Ok(categories);
        }

        // website.com/api/categories/withposts
        [HttpGet("withposts")]
        public async Task<IActionResult> GetWithPosts()
        {
            List<Category> categories = await _appDbContext.Categories
                .Include(category=>category.Posts)
                .ToListAsync();
            return Ok(categories);
        }

        // website.com/api/categories/2
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await GetCategoyById(id, false);

            return Ok(category);
        }

        // website.com/api/categories/withposts/2
        [HttpGet("withposts/{id}")]
        public async Task<IActionResult> GetWithPosts(int id)
        {
            Category category = await GetCategoyById(id, true);

            return Ok(category);
        }



        #endregion

        #region Utility Methods
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> PersistChangesToDatabase()
        {
            int amountOfChanges = await _appDbContext.SaveChangesAsync();

            return amountOfChanges > 0;
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<Category> GetCategoyById(int categoyId, bool withPosts)
        {
            Category categoryToGet;

            if (withPosts)
            {
                categoryToGet = await _appDbContext.Categories
                                        .Include(category => category.Posts)
                                        .FirstAsync(category => category.CategoryId == categoyId);
            }
            else
            {
                categoryToGet = await _appDbContext.Categories
                                        .FirstAsync(category=>category.CategoryId == categoyId);
            }

            return categoryToGet;
        }

        #endregion
    }
}
