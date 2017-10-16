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
            
            Assert.AreEqual("Hello, World!", property.GetGlobalProperty("message"));
        }

        [Test]
        public void GroupSaveTest()
        {
            const string file = "tests/group_save.property";
            
            new PropertyFile(new Dictionary<string, Dictionary<string, string>>
            {
                ["Group 1"] = new Dictionary<string, string> { ["one"] = "1" },
                ["Group 2"] = new Dictionary<string, string> { ["two"] = "2" }
            }, null).Save(file);
            
            var property = new PropertyFile(file);
            Assert.AreEqual(2, property.Groups.Count);

            // Group 1
            {
                var group = property["Group 1"];
                Assert.IsNotNull(group);

                var one = group["one"];
                Assert.IsNotNull(one);
                Assert.AreEqual("1", one);
            }
            
            // Group 2
            {
                var group = property["Group 2"];
                Assert.IsNotNull(group);

                var two = group["two"];
                Assert.IsNotNull(two);
                Assert.AreEqual("2", two);
            }
        }

        [Test]
        public void GroupLoadTest()
        {
            var property = new PropertyFile("tests/group_load.property");
            Assert.AreEqual(2, property.Groups.Count);
            Assert.AreEqual(2, property.GlobalProperties.Count);

            // stray_global and another_global
            {
                var stray = property.GetGlobalProperty("stray_global");
                Assert.IsNotNull(stray);
                Assert.AreEqual("Whoops!", stray);

                var another = property.GetGlobalProperty("another_global");
                Assert.IsNotNull(another);
                Assert.AreEqual("Yay!", another);
            }
            
            // Test Group 1
            {
                var group = property["Test Group 1"];
                Assert.IsTrue(group.Count == 1);

                var message = group["message"];
                Assert.IsNotNull(message);
                Assert.AreEqual("Hello, World!", message);
            }
            
            // Test Group 2
            {
                var group = property["Test Group 2"];
                Assert.IsTrue(group.Count == 1);

                var message = group["message"];
                Assert.IsNotNull(message);
                Assert.AreEqual("Goodbye, World!", message);
            }
        }

        [Test]
        public void SaveAndLoadTest()
        {
            const string file = "tests/saveload.property";

            new PropertyFile(null, new Dictionary<string, string>
            {
                ["property.first"] = "First Property",
                ["property.second"] = "Second Property",
                ["property.third"] = "Third Property"
            }).Save(file);

            var stringProperty = new PropertyFile(file);

            Assert.AreEqual("First Property", stringProperty.GetGlobalProperty("property.first"));
            Assert.AreEqual("Second Property", stringProperty.GetGlobalProperty("property.second"));
            Assert.AreEqual("Third Property", stringProperty.GetGlobalProperty("property.third"));
        }

        [Test]
        public void CommentTest()
        {
            var commentProperty = new PropertyFile("tests/comment.property");

            Assert.AreEqual("Hello, World!", commentProperty.GetGlobalProperty("message"));
        }

        [Test]
        public void SpacingTest()
        {
            var spacingProperty = new PropertyFile("tests/spacing.property");

            Assert.AreEqual("    Space!", spacingProperty.GetGlobalProperty("space"));
            Assert.AreEqual("No Space!", spacingProperty.GetGlobalProperty("nospace"));
        }

        [Test]
        public void SpacingSaveTest()
        {
            const string file = "tests/spacingsave.property";

            new PropertyFile(null, new Dictionary<string, string>
            {
                ["nospace"] = "No spaces",
                ["space"] = "    Four spaces"
            }).Save(file);

            var variedProperty = new PropertyFile(file);

            Assert.AreEqual("No spaces", variedProperty.GetGlobalProperty("nospace"));
            Assert.AreEqual("    Four spaces", variedProperty.GetGlobalProperty("space"));
        }
        
        [Test]
        public void ErrorTest()
        {
            Assert.Throws<InvalidPropertyException>(() => new PropertyFile("tests/error.property"));
        }
    }
}
