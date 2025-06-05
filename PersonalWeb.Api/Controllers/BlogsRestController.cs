using Microsoft.AspNetCore.Mvc;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class BlogsRestController : DefaultRestController<Blog, BlogRepositories>
    {
        public BlogsRestController(BlogRepositories repo) : base(repo)
        {
        }
    }
}