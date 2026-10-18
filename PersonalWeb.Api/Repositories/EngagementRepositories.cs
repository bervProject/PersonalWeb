using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class EngagementRepositories : DefaultModelRepositories<Engagement, PersonalWebApiContext>
    {
        public EngagementRepositories(PersonalWebApiContext context) : base(context)
        {
        }
    }
}
