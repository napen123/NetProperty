using System;
using System.IO;
using System.Collections.Generic;

using NUnit.Framework;

using NetProperty;

namespace NetPropertyTest
{
    [SetUpFixture]
    public class TestSetup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var dir = Path.GetDirectoryName(typeof(TestSetup).Assembly.Location);

            if (dir == null)
                return;
            
            Environment.CurrentDirectory = dir;
        }
    }
    
    [TestFixture]
    public class Test
    {
        [Test]
        public void SimpleTest()
        {
            var property = new PropertyFile("tests/simple.property");
            
            Assert.AreEqual("Hello, World!", property.GetProperty("message"));
        }

        [Test]
        public void SaveAndLoadTest()
        {
            const string file = "tests/saveload.property";

            new PropertyFile(new Dictionary<string, string>(3)
            {
                ["property.first"] = "First Property",
                ["property.second"] = "Second Property",
                ["property.third"] = "Third Property"
            }).Save(file);

            var stringProperty = new PropertyFile(file);

            Assert.AreEqual("First Property", stringProperty.GetProperty("property.first"));
            Assert.AreEqual("Second Property", stringProperty.GetProperty("property.second"));
            Assert.AreEqual("Third Property", stringProperty["property.third"]);
        }

        [Test]
        public void CommentTest()
        {
            var commentProperty = new PropertyFile("tests/comment.property");

            Assert.AreEqual("Hello, World!", commentProperty.GetProperty("message"));
        }

        [Test]
        public void SpacingTest()
        {
            var spacingProperty = new PropertyFile("tests/spacing.property");

            Assert.AreEqual("    Space!", spacingProperty["space"]);
            Assert.AreEqual("No Space!", spacingProperty["nospace"]);
        }

        [Test]
        public void SpacingSaveTest()
        {
            const string file = "tests/spacingsave.property";

            new PropertyFile(new Dictionary<string, string>(2)
            {
                ["nospace"] = "No spaces",
                ["space"] = "    Four spaces"
            }).Save(file);

            var variedProperty = new PropertyFile(file);

            Assert.AreEqual("No spaces", variedProperty.GetProperty("nospace"));
            Assert.AreEqual("    Four spaces", variedProperty.GetProperty("space"));
        }
        
        [Test]
        public void ErrorTest()
        {
            Assert.Throws<InvalidPropertyException>(() => new PropertyFile("tests/error.property"));
        }
    }
}
