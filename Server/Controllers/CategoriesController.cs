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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(AppDBContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
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
                .Include(category => category.Posts)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category categoryToCreate)
        {
            try
            {
                if (categoryToCreate == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _appDbContext.Categories.AddAsync(categoryToCreate);

                bool changePersistedToDatabase = await PersistChangesToDatabase();

                if (!changePersistedToDatabase)
                {
                    return StatusCode(500, "Something went wrong on our side. Please contact administrator");
                } else
                {
                    return Created("Create", categoryToCreate);
                }
            }
            catch (Exception e)
            {

                return StatusCode(
                    500, 
                    $"Something went wrong on our side, please contact the administrator on error: {e.Message}"
                    );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Category categoryToUpdate, int id)
        {
            Console.WriteLine(categoryToUpdate.Name);
            
            try
            {
                if (id < 1 || categoryToUpdate == null || id != categoryToUpdate.CategoryId)
                {
                    return BadRequest(ModelState);
                }

                bool exists = await _appDbContext.Categories.AnyAsync(category => category.CategoryId == id);

                if (!exists)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _appDbContext.Categories.Update(categoryToUpdate);

                bool changePersistedToDatabase = await PersistChangesToDatabase();

                if (!changePersistedToDatabase)
                {
                    return StatusCode(500, "Something went wrong on our side. Please contact administrator");
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {

                return StatusCode(
                    500,
                    $"Something went wrong on our side, please contact the administrator on error: {e.Message}"
                    );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest(ModelState);
                }

                bool exists = await _appDbContext.Categories.AnyAsync(category => category.CategoryId == id);

                if (!exists)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Category categoryToDelete = await GetCategoyById(id, false);

                if (categoryToDelete.ThumbnailImagePath != "uploads/placeholder.jpg")
                {
                    string fileName = categoryToDelete.ThumbnailImagePath.Split('/').Last();

                    System.IO.File.Delete($"{_webHostEnvironment.ContentRootPath}\\wwwroot\\uploads\\{fileName}");
                }

                _appDbContext.Categories.Remove(categoryToDelete);

                bool changePersistedToDatabase = await PersistChangesToDatabase();

                if (!changePersistedToDatabase)
                {
                    return StatusCode(500, "Something went wrong on our side. Please contact administrator");
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {

                return StatusCode(
                   500,
                   $"Something went wrong on our side, please contact the administrator on error: {e.Message}"
                   );
            }
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
