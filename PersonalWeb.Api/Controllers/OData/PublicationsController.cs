using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public class PublicationsController : DefaultController<Publication, PublicationRepositories>
    {
        public PublicationsController(PublicationRepositories repo) : base(repo)
        {
        }
    }
}
