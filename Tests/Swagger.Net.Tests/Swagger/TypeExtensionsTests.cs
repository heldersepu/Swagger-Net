using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Swagger.Net.Swagger
{
    public class TypeExtensionsTests
    {
        [TestCase(typeof(DateTime), "DateTime")]
        [TestCase(typeof(IEnumerable<string>), "IEnumerableOfString")]
        [TestCase(typeof(IDictionary<string, decimal>), "IDictionaryOfStringAndDecimal")]
        public void FriendlyId_ReturnsNonQualifiedFriendlyId_IfFullyQualifiedFlagIsUnset(
            Type systemType,
            string expectedReturnValue)
        {
            Assert.AreEqual(expectedReturnValue, systemType.FriendlyId());
        }

        [TestCase(typeof(DateTime), "System.DateTime")]
        [TestCase(typeof(IEnumerable<string>), "System.Collections.Generic.IEnumerableOfSystem.String")]
        [TestCase(typeof(IDictionary<string, decimal>), "System.Collections.Generic.IDictionaryOfSystem.StringAndSystem.Decimal")]
        [TestCase(typeof(TypeExtensionsTests.InnerType), "Swagger.Net.Swagger.TypeExtensionsTests.InnerType")]
        public void FriendlyId_ReturnsFullQualifiedFriendlyId_IfFullyQualifiedFlagIsSet(
            Type systemType,
            string expectedReturnValue)
        {
            Assert.AreEqual(expectedReturnValue, systemType.FriendlyId(true));
        }

        [Test]
        public void GetEnumNamesForSerialization_HonorsEnumMemberAttributes()
        {
            var enumNames = typeof(EnumWithMemberAttributes).GetEnumNamesForSerialization();
            CollectionAssert.AreEqual(new[] { "value-1", "value-2" }, enumNames);
        }

        [Test]
        public void GetEnumNamesForSerialization_ExcludesObsoleteAttributes()
        {
            var enumNames = typeof(EnumWithObsoleteAttributes).GetEnumNamesForSerialization(excludeObsolete: true);
            CollectionAssert.AreEqual(new[] { "Value3" }, enumNames);
        }

        [Test]
        public void GetEnumValuesForSerialization_ExcludesObsoleteAttributes()
        {
            var enumValues = typeof(EnumWithObsoleteAttributes).GetEnumValuesForSerialization(excludeObsolete: true);
            CollectionAssert.AreEqual(new[] { 2 }, enumValues);
        }

        [Test]
        public void FullNameSansTypeParameters_Test()
        {
            var mock = new Mock<Type>();
            mock.Setup(x => x.Name).Returns("");
            Assert.AreEqual("", mock.Object.FullNameSansTypeParameters());
        }

        private class InnerType
        {
            public string Property1 { get; set; }
        }

        public enum EnumWithMemberAttributes
        {
            [EnumMember(Value = "value-1")]
            Value1 = 0,

            [EnumMember(Value = "value-2")]
            Value2 = 1
        }

        public enum EnumWithObsoleteAttributes
        {
            [Obsolete]
            Value1 = 0,

            [Obsolete]
            Value2 = 1,

            Value3 = 2
        }
    }
}