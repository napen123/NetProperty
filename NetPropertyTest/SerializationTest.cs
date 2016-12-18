using System.IO;

using NetProperty.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetPropertyTest
{
    [TestClass]
    public class SerializationTest
    {
        public class TestClass
        {
            [Property("test.integer")]
            public int TestInteger;
            
            public bool TestBoolean;

            [Property("TEST STRING")]
            public string TestString;

            [Property("whitespace")]
            public string TestWhitespace;

            [Property("testFloat")]
            public float TestFloat { get; private set; } = 5.5f;
        }

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
