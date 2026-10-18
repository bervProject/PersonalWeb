using Microsoft.AspNetCore.Mvc;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class PublicationsRestController : DefaultRestController<Publication, PublicationRepositories>
    {
        public PublicationsRestController(PublicationRepositories repo) : base(repo)
        {
        }
    }
}
