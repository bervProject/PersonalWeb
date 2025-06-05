using System.Collections.Generic;

namespace PersonalWeb.Api.Integration.Test.Models
{
    public class ODataListResponse<T>
    {
        public List<T> Value { get; set; }
    }
}
