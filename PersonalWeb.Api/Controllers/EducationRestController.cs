using Microsoft.AspNetCore.Mvc;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class EducationRestController : DefaultRestController<Education, EducationRepositories>
    {
        public EducationRestController(EducationRepositories repo) : base(repo)
        {
        }
    }
}
