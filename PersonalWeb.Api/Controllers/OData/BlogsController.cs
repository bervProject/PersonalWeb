using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class BlogsController : DefaultController<Blog, BlogRepositories>
    {
        public BlogsController(BlogRepositories repo) : base(repo)
        {
        }
    }
}