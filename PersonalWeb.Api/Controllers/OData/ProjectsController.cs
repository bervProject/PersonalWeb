using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class ProjectsController : DefaultController<Project, ProjectRepositories>
    {
        public ProjectsController(ProjectRepositories repo) : base(repo)
        {
        }
    }
}
