using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class ProfilesController : DefaultController<Profile, ProfileRepositories>
    {
        public ProfilesController(ProfileRepositories repo) : base(repo)
        {
        }
    }
}
