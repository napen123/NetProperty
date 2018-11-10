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

            new PropertyFile(3)
            {
                ["property.first"] = "First Property",
                ["property.second"] = "Second Property",
                ["property.third"] = "Third Property"
            }.Save(file);

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

            new PropertyFile(2)
            {
                ["nospace"] = "No spaces",
                ["space"] = "    Four spaces"
            }.Save(file);

            var variedProperty = new PropertyFile(file);

            Assert.AreEqual("No spaces", variedProperty.GetProperty("nospace"));
            Assert.AreEqual("    Four spaces", variedProperty.GetProperty("space"));
        }
        
        [Test]
        public void ErrorTest()
        {
            Assert.Throws<InvalidPropertyException>(() => new PropertyFile("tests/error.property"));
        }

        [Test]
        public void NullValueTest()
        {
            const string file = "tests/null.property";

            new PropertyFile(new Dictionary<string, string>(1)
            {
                ["my property"] = null
            }).Save(file);

            // Treat empty values as null.
            {
                var treatNullProperty = new PropertyFile(file, true);

                Assert.AreEqual(null, treatNullProperty["my property"]);
            }

            // _Don't_ treat empty values as null.
            {
                var treatNullProperty = new PropertyFile(file);

                Assert.AreEqual(string.Empty, treatNullProperty["my property"]);
            }
        }

        [Test]
        public void TryLoadTest()
        {
            const string tryTrue = "tests/try.true.property";
            const string tryFalse = "tests/try.false.property";

            var property = new PropertyFile();

            Assert.IsTrue(property.TryLoad(tryTrue));
            Assert.IsFalse(property.TryLoad(tryFalse));
        }

        [Test]
        public void IterationTest()
        {
            const string file = "tests/iter.property";

            var propertyFile = new PropertyFile(file);

            foreach (var property in propertyFile)
                Console.WriteLine($"{property.Key} : [{property.Value}]");
        }
    }
}
