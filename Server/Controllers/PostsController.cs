using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public PostsController(AppDBContext appDbContext, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Post> posts = await _appDbContext.Posts
                .Include(post => post.Category)
                .ToListAsync();

            return Ok(posts);
        }

        // website.com/api/posts/2
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Post post = await GetPostById(id);

            return Ok(post);
        }

       

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostDto postToCreateDto)
        {
            try
            {
                if (postToCreateDto == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Post postToCreate = _mapper.Map<Post>(postToCreateDto);

                if (postToCreateDto.Published)
                {
                    postToCreate.PublishDate = DateTime.UtcNow;
                }
              

                await _appDbContext.Posts.AddAsync(postToCreate);

                bool changePersistedToDatabase = await PersistChangesToDatabase();

                if (!changePersistedToDatabase)
                {
                    return StatusCode(500, "Something went wrong on our side. Please contact administrator");
                }
                else
                {
                    return Created("Create", postToCreateDto);
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
        public async Task<IActionResult> Update([FromBody] PostDto updatedPostDto, int id)
        {
            try
            {
                if (id < 1 || updatedPostDto == null || id != updatedPostDto.PostId)
                {
                    return BadRequest(ModelState);
                }

                Post oldPost = await _appDbContext.Posts.FindAsync(id);

                if (oldPost == null)
                {
                    return NotFound();
                }

                Post postToUpdate = _mapper.Map<Post>(updatedPostDto);

                if (!oldPost.Published && updatedPostDto.Published)
                {
                    postToUpdate.PublishDate = DateTime.UtcNow;
                }

                // Debatch oldpost from EF, else it can't be updated
                _appDbContext.Entry(oldPost).State = EntityState.Detached;

                _appDbContext.Posts.Update(postToUpdate);

                bool changePersistedToDatabase = await PersistChangesToDatabase();

                if (!changePersistedToDatabase)
                {
                    return StatusCode(500, "Something went wrong on our side. Please contact administrator");
                }
                else
                {
                    return Created("Create", postToUpdate);
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

                bool exists = await _appDbContext.Posts.AnyAsync(post => post.PostId == id);

                if (!exists)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Post postToDelete = await GetPostById(id);

                if (postToDelete.ThumbnailImagePath != "uploads/placeholder.jpg")
                {
                    string fileName = postToDelete.ThumbnailImagePath.Split('/').Last();

                    System.IO.File.Delete($"{_webHostEnvironment.ContentRootPath}\\wwwroot\\uploads\\{fileName}");
                }

                _appDbContext.Posts.Remove(postToDelete);

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
        private async Task<Post> GetPostById(int postId)
        {
            Post postToGet = await _appDbContext.Posts
                .Include(post => post.Category)
                .FirstAsync(post => post.PostId == postId);

            return postToGet;
        }

        #endregion
    }
}
