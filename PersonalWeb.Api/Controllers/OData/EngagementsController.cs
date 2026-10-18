using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class EngagementsController : DefaultController<Engagement, EngagementRepositories>
    {
        public EngagementsController(EngagementRepositories repo) : base(repo)
        {
        }
    }
}
