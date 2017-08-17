using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Swagger.Net.Tests.CoreUnitTests
{
    [TestFixture]
    class JsonPropertyExtensionsTests
    {
        [Test]
        public void FieldInfo_Null()
        {
            var x = new JsonProperty();
            x.UnderlyingName = null;
            Assert.IsNull(x.FieldInfo());
        }

        [Test]
        public void FieldInfo_Test()
        {
            var x = new JsonProperty();
            x.UnderlyingName = "test";
            x.DeclaringType = typeof(Test);
            Assert.IsNull(x.FieldInfo());
        }
    }

    [MetadataType(typeof(int))]
    public class Test
    {
        [MetadataType(typeof(int))]
        public class Meta { }

        public int id { get; set; }
        public MetadataTypeAttribute attrib1 { get; set; }
        public Meta attrib2 { get; set; }
    }
}
