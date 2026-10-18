using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class EducationRepositories : DefaultModelRepositories<Education, PersonalWebApiContext>
    {
        public EducationRepositories(PersonalWebApiContext context) : base(context)
        {
        }
    }
}
