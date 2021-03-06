﻿namespace OmniXaml.Tests
{
    using Builder;
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Typing;

    [TestClass]
    public class XamlTypeRepositoryTests
    {
        private readonly Mock<IXamlNamespaceRegistry> nsRegistryMock;

        public XamlTypeRepositoryTests()
        {
            nsRegistryMock = new Mock<IXamlNamespaceRegistry>();

            var type = typeof(DummyClass);

            var fullyConfiguredMapping = XamlNamespace
                .Map("root")
                .With(new[] {Route.Assembly(type.Assembly).WithNamespaces(new[] {type.Namespace})});

            nsRegistryMock.Setup(registry => registry.GetNamespace("root"))
                .Returns(fullyConfiguredMapping);
            nsRegistryMock.Setup(registry => registry.GetNamespace("clr-namespace:DummyNamespace;Assembly=DummyAssembly"))
                .Returns(new ClrNamespace(type.Assembly, type.Namespace));
        }

        [TestMethod]
        public void GetWithFullAddressReturnsCorrectType()
        {          
            var sut = new XamlTypeRepository(nsRegistryMock.Object, new TypeFactoryDummy());

            var xamlType = sut.GetWithFullAddress(new XamlTypeName("root", "DummyClass"));

            Assert.AreEqual(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [TestMethod]
        public void GetWithFullAddressOfClrNamespaceReturnsTheCorrectType()
        {
            var sut = new XamlTypeRepository(nsRegistryMock.Object, new TypeFactoryDummy());

            var xamlType = sut.GetWithFullAddress(new XamlTypeName("clr-namespace:DummyNamespace;Assembly=DummyAssembly", "DummyClass"));

            Assert.AreEqual(xamlType.UnderlyingType, typeof(DummyClass));
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotFoundException))]
        public void FullAddressOfUnknownThrowNotFound()
        {
            var sut = new XamlTypeRepository(nsRegistryMock.Object, new TypeFactoryDummy());

            const string unreachableTypeName = "UnreachableType";
            sut.GetWithFullAddress(new XamlTypeName("root", unreachableTypeName));
       }
    }
}
