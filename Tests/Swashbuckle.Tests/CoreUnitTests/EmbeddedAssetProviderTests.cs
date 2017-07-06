using NUnit.Framework;
using Swashbuckle.SwaggerUi;
using System.Collections.Generic;

namespace Swashbuckle.Tests.CoreUnitTests
{
    [TestFixture]
    class EmbeddedAssetProviderTests
    {
        [Test]
        public void GetAsset_null()
        {
            var prov = new EmbeddedAssetProvider(
                new Dictionary<string, EmbeddedAssetDescriptor>(),
                new Dictionary<string, string>());
            Assert.Throws<AssetNotFound>(() => prov.GetAsset("",""));
        }

        [Test]
        public void GetEmbeddedResourceStreamFor_null()
        {
            var prov = new EmbeddedAssetProvider(
                new Dictionary<string, EmbeddedAssetDescriptor>(),
                new Dictionary<string, string>());
            var ad = new EmbeddedAssetDescriptor(typeof(EmbeddedAssetProviderTests).Assembly, "123", false);
            Assert.Throws<AssetNotFound>(() => prov.GetEmbeddedResourceStreamFor(ad, ""));
        }

        [TestCase("file.css", "text/css")]
        [TestCase("file.js", "text/javascript")]
        [TestCase("file.gif", "image/gif")]
        [TestCase("file.png", "image/png")]
        [TestCase("file.eot", "application/vnd.ms-fontobject")]
        [TestCase("file.woff", "application/font-woff")]
        [TestCase("file.woff2", "application/font-woff2")]
        [TestCase("file.otf", "application/font-sfnt")]
        [TestCase("file.ttf", "application/font-sfnt")]
        [TestCase("file.svg", "image/svg+xml")]
        [TestCase("file.unkown", "text/html")]
        public void InferMediaTypeFrom(string file, string mediaType)
        {
            Assert.AreEqual(mediaType, EmbeddedAssetProvider.InferMediaTypeFrom(file));
        }
    }
}
