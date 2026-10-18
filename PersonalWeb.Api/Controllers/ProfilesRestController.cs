using Microsoft.AspNetCore.Mvc;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class ProfilesRestController : DefaultRestController<Profile, ProfileRepositories>
    {
        public ProfilesRestController(ProfileRepositories repo) : base(repo)
        {
        }
    }
}
