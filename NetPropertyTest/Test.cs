using System.IO;

using NetProperty;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetPropertyTest
{
    [TestClass]
    public class Test
    {
        public class TestClass
        {
            [Property("test.integer")] public int TestInteger;

            public bool TestBoolean;

            [Property("TEST STRING")] public string TestString;

            [Property("whitespace")] public string TestWhitespace;

            [Property("testFloat")]
            public float TestFloat { get; private set; } = 5.5f;
        }

        [TestMethod]
        public void SimpleTest()
        {
            var property = new PropertyFile("tests/simple.property");

            Assert.AreEqual("Hello, World!", property.GetProperty("message"));
            Assert.AreEqual("Hello, World!", property["message"]);
        }

        [TestMethod]
        public void SaveAndLoadTest()
        {
            const string file = "tests/saveload.property";

            new PropertyFile
            {
                ["property.first"] = "First Property",
                ["property.second"] = "Second Property",
                ["property.third"] = "Third Property"
            }.Save(file);

            var stringProperty = new PropertyFile(file);

            Assert.AreEqual("First Property", stringProperty["property.first"]);
            Assert.AreEqual("Second Property", stringProperty["property.second"]);
            Assert.AreEqual("Third Property", stringProperty["property.third"]);
        }

        [TestMethod]
        public void CommentTest()
        {
            var commentProperty = new PropertyFile("tests/comment.property");

            Assert.AreEqual("Hello, World!", commentProperty["message"]);
        }

        [TestMethod]
        public void SpacingTest()
        {
            var spacingProperty = new PropertyFile("tests/spacing.property");

            Assert.AreEqual("    Space!", spacingProperty["space"]);
            Assert.AreEqual("No Space!", spacingProperty["nospace"]);
        }

        [TestMethod]
        public void SpacingSaveTest()
        {
            const string file = "tests/spacingsave.property";

            new PropertyFile
            {
                ["nospace"] = "No spaces",
                ["space"] = "    Four spaces"
            }.Save(file);

            var variedProperty = new PropertyFile(file);

            Assert.AreEqual("No spaces", variedProperty["nospace"]);
            Assert.AreEqual("    Four spaces", variedProperty["space"]);
        }

        // TODO: Fix me!
        /*
        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyException))]
        public void ErrorTest()
        {
            new PropertyFile().Load("tests/error.property");
        }
        */

        [TestMethod]
        public void SerializeTest()
        {
            const string file = "tests/serialize.property";

            // Serialize
            {
                var test = new TestClass
                {
                    TestBoolean = true,
                    TestInteger = 123456,
                    TestString = "This is a test string!",
                    TestWhitespace = "    "
                };

                PropertySerializer.Serialize(File.Open(file, FileMode.Create), test);
            }

            // Deserialize
            {
                var test = PropertySerializer.Deserialize(File.Open(file, FileMode.Open));

                Assert.AreEqual(true, bool.Parse(test["TestBoolean"]));
                Assert.AreEqual(123456, int.Parse(test["test.integer"]));
                Assert.AreEqual("This is a test string!", test["TEST STRING"]);
                Assert.AreEqual("    ", test["whitespace"]);
                Assert.AreEqual(5.5f, float.Parse(test["testFloat"]));
            }
        }
    }
}
