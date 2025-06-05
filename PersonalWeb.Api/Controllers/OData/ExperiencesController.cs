using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class ExperiencesController : DefaultController<Experience, ExperienceRepositories>
    {
        public ExperiencesController(ExperienceRepositories repo) : base(repo)
        {
        }
    }
}