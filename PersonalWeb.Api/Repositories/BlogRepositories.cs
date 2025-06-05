using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.Repositories
{
    public class BlogRepositories : DefaultModelRepositories<Blog, PersonalWebApiContext>
    {
        public BlogRepositories(PersonalWebApiContext context) : base(context)
        {

        }
    }
}
