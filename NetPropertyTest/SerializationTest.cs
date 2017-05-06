using System;
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

            [NonSerialized]
            public string TestDontSerialize;

            [Property("testFloat")]
            public float TestFloat { get; set; }
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
                    TestWhitespace = "    ",
                    TestDontSerialize = "Don't serialize me!",
                    TestFloat = 5.5f
                };

                PropertySerializer.Serialize(File.Open(file, FileMode.Create), test);
            }

            // Deserialize
            {
                var test = PropertySerializer.Deserialize<TestClass>(File.Open(file, FileMode.Open));
                
                Assert.AreEqual(true, test.TestBoolean);
                Assert.AreEqual(123456, test.TestInteger);
                Assert.AreEqual("This is a test string!", test.TestString);
                Assert.AreEqual("    ", test.TestWhitespace);
                Assert.AreEqual(5.5f, test.TestFloat);

                Assert.IsNull(test.TestDontSerialize);
            }
        }
    }
}
