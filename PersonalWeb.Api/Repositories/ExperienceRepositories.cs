using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class ExperienceRepositories : DefaultModelRepositories<Experience, PersonalWebApiContext>
    {
        public ExperienceRepositories(PersonalWebApiContext context) : base(context)
        {

        }
    }
}
