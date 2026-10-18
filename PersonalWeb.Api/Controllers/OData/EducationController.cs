using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class EducationController : DefaultController<Education, EducationRepositories>
    {
        public EducationController(EducationRepositories repo) : base(repo)
        {
        }
    }
}
