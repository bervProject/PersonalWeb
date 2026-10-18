using Microsoft.AspNetCore.Mvc;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class EngagementsRestController : DefaultRestController<Engagement, EngagementRepositories>
    {
        public EngagementsRestController(EngagementRepositories repo) : base(repo)
        {
        }
    }
}
