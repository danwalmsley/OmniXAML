﻿namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Classes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OmniXaml.Reader.Tests.Wpf;
    using Xaml.Tests.Resources;

    [TestClass]
    public class PropertyReadingTests : GivenAXamlXmlLoader
    {
        [TestMethod]
        public void StringProperty()
        {
            var actualInstance = BootstrappableXamlStreamLoader.Load(Dummy.StringProperty);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void ExpandedStringProperty()
        {
            var actualInstance = BootstrappableXamlStreamLoader.Load(Dummy.InnerContent);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }

        [TestMethod]
        public void NonStringProperty()
        {
            var actualInstance = BootstrappableXamlStreamLoader.Load(Dummy.NonStringProperty);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass, "dummyClass != null");
            Assert.AreEqual(1234, dummyClass.Number);
        }

        [TestMethod]
        public void ChildCollection()
        {       
            var actualInstance = BootstrappableXamlStreamLoader.Load(Dummy.ChildCollection);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.IsNotNull(dummyClass.Items);
            Assert.AreEqual(3, dummyClass.Items.Count);
        }

        [TestMethod]
        public void AttachedProperty()
        {
            var actualInstance = BootstrappableXamlStreamLoader.Load(Dummy.AttachedProperty);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual(Container.GetProperty(dummyClass), "Value");
        }

        [TestMethod]
        public void Ignorable()
        {
            var actualInstance = BootstrappableXamlStreamLoader.Load(Dummy.Ignorable);

            Assert.IsInstanceOfType(actualInstance, typeof(DummyClass), "The retrieved instance should be of type DummyClass");
            var dummyClass = actualInstance as DummyClass;
            Assert.IsNotNull(dummyClass);
            Assert.AreEqual("Property!", dummyClass.SampleProperty);
        }
    }
}
