using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class ProfileRepositories : DefaultModelRepositories<Profile, PersonalWebApiContext>
    {
        public ProfileRepositories(PersonalWebApiContext context) : base(context)
        {
        }
    }
}
