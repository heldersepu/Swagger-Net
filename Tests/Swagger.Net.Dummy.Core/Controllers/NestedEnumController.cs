using System.Web.Http;

namespace Swagger.Net.Dummy.Controllers
{
    public abstract class NestedEnum<T> : ApiController
    {
        public enum Giorno { Lunedi, Martedi, Venerdi }


        /// <summary>NestedEnum test</summary>
        public string Put(Giorno value)
        {
            return "Nullable Enum parameter";
        }
    }

    public class NestedEnumController : NestedEnum<int> { }
}
