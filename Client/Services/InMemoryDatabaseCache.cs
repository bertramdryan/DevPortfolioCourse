using System.Globalization;
using Client.Static;
using Shared.Models;
using System.Linq;
using System.Net.Http.Json;

namespace Client.Services
{
    internal sealed class InMemoryDatabaseCache
    {
        private readonly HttpClient _httpClient;

        public InMemoryDatabaseCache(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        #region Categories

        private List<Category> _categories = null;
        internal List<Category> Categories
        {
            get 
            { 
                return _categories; 
            }
            set
            {
                _categories = value;
                NotifyCategoriesDataChanged();
            }
        }

        internal async Task<Category> GetCategoryByCategoryId(int categoryId, bool withPosts)
        { 
            if (_categories == null)
            {
                await GetCategoriesFromDatabaseAndCache(withPosts);
            }

            Category categoryToReturn = _categories.First(category => category.CategoryId == categoryId);

            if (categoryToReturn.Posts == null && withPosts)
            {
                categoryToReturn = await _httpClient.GetFromJsonAsync<Category>($"{ApiEndpoints.s_catetoriesWithPosts}/{categoryToReturn.CategoryId}");
            }

            return categoryToReturn;
        }

        internal async Task<Category> GetCategoryByCategoryName(string categoryName, bool withPosts, bool nameToLowerFromUrl)
        {
            if (_categories == null)
            {
                await GetCategoriesFromDatabaseAndCache(withPosts);
            }

            Category categoryToReturn = null;

            if (nameToLowerFromUrl)
            {
                categoryToReturn = _categories.First(category => category.Name.ToLowerInvariant() == categoryName);
            }
            else
            {
                categoryToReturn = _categories.First(category => category.Name == categoryName);
            }

            if (categoryToReturn.Posts == null && withPosts)
            {
               categoryToReturn = await _httpClient.GetFromJsonAsync<Category>($"{ApiEndpoints.s_catetoriesWithPosts}/{categoryToReturn.CategoryId}");
            }

            return categoryToReturn;
        }

        private bool _gettingCategoriesFromDatabaseAndCaching = false;
        internal async Task GetCategoriesFromDatabaseAndCache(bool withPosts)
        {
            // Only allow 1 get request ot run at a time
            if (_gettingCategoriesFromDatabaseAndCaching == false)
            {
                _gettingCategoriesFromDatabaseAndCaching = true;

                List<Category> categoriesFromDatabase = default;

                if (_categories != null)
                {
                    _categories = null;
                }

                if (withPosts)
                {
                    categoriesFromDatabase = await _httpClient.GetFromJsonAsync<List<Category>>(ApiEndpoints.s_catetoriesWithPosts);
                }
                else
                {
                    categoriesFromDatabase = await _httpClient.GetFromJsonAsync<List<Category>>(ApiEndpoints.s_catetories);
                }

                _categories = categoriesFromDatabase.OrderByDescending(category => category.CategoryId).ToList();

                if (withPosts)
                {
                    List<Post> postsFromCategories = new List<Post>();

                    foreach (var category in categoriesFromDatabase)
                    {
                        if (category.Posts.Count != 0)
                        {
                            foreach (var post in category.Posts)
                            {
                                Category postCategoryWithoutPosts = new Category()
                                {
                                    CategoryId = category.CategoryId,
                                    ThumbnailImagePath = category.ThumbnailImagePath,
                                    Name = category.Name,
                                    Description = category.Description,
                                    Posts = null
                                };
                                post.Category = postCategoryWithoutPosts;
                                postsFromCategories.Add(post);
                            }
                        }
                    }

                    _posts = postsFromCategories.OrderByDescending(post => post.PostId).ToList();
                }
                
                _gettingCategoriesFromDatabaseAndCaching = false;
                
            }
           
        }

        internal event Action OnCategoriesDataChanged;

        private void NotifyCategoriesDataChanged() => OnCategoriesDataChanged?.Invoke();
        #endregion
        
        #region Posts

        private List<Post> _posts = null;

        internal List<Post> Posts
        {
            get
            {
                return _posts;
            }
            set
            {
                _posts = value;
                NotifyPostDataChanged();
            }
        }

        internal async Task<Post> GetPostByPostId(int postId)
        {
            if (_posts == null)
            {
                await GetPostsFromDatabaseAndCache();
            }

            return _posts.First(post => post.PostId == postId);
        }
        
        internal async Task<PostDto> GetPostDtoByPostId(int postId) => await _httpClient.GetFromJsonAsync<PostDto>($"{ApiEndpoints.s_postsDto}/{postId}");

        private bool _gettingPostFromDatabaseAndCaching = false;

        internal async Task GetPostsFromDatabaseAndCache()
        {
            if (_gettingPostFromDatabaseAndCaching == false)
            {
                _gettingPostFromDatabaseAndCaching = true;

                if (_posts != null)
                {
                    _posts = null;
                }

                List<Post> postsFromDatabase = await _httpClient.GetFromJsonAsync<List<Post>>(ApiEndpoints.s_posts);
                _posts = postsFromDatabase.OrderByDescending(post => post.PostId).ToList();

                _gettingPostFromDatabaseAndCaching = false;
            }
        }

        internal event Action OnPostDataChanged;
        private void NotifyPostDataChanged() => OnPostDataChanged?.Invoke();

        #endregion
    }
}
