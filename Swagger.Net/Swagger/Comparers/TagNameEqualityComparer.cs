using System.Collections.Generic;

namespace Swagger.Net
{
    public class TagNameEqualityComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag x, Tag y)
        {
            return x.name.Equals(y.name);
        }

        public int GetHashCode(Tag obj)
        {
            return obj.name.GetHashCode();
        }
    }
}
