using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class PublicationRepositories : DefaultModelRepositories<Publication, PersonalWebApiContext>
    {
        public PublicationRepositories(PersonalWebApiContext context) : base(context)
        {
        }
    }
}
