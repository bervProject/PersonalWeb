using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class ProjectRepositories : DefaultModelRepositories<Project, PersonalWebApiContext>
    {
        public ProjectRepositories(PersonalWebApiContext context) : base(context)
        {

        }
    }
}
