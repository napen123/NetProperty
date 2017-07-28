using System;
using System.IO;

using NetProperty.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetPropertyTest
{
    [TestClass]
    public class SerializationTest
    {
        public class TestConverter : PropertyConverter
        {
            public override string Serialize(object value)
            {
                return "-->" + (value as TestClass.InnerTestClass).InnerName;
            }

            public override object Deserialize(string value)
            {
                return new TestClass.InnerTestClass {InnerName = value.Substring(3)};
            }
        }

        public class TestClass
        {
            public class InnerTestClass
            {
                public string InnerName;
            }

            [Property("test.integer")]
            public int TestInteger;
            
            public bool TestBoolean;
            
            [Property("TEST STRING")]
            public string TestString;
            
            [Property("whitespace")]
            public string TestWhitespace;

            [NonSerialized]
            public string TestDontSerialize;

            [Property("test.inner.class", typeof(TestConverter))]
            public InnerTestClass TestInnerClass;

            [Property(typeof(TestConverter))]
            public InnerTestClass TestNamelessClass;

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
                    TestFloat = 5.5f,

                    TestInnerClass = new TestClass.InnerTestClass
                    {
                        InnerName = "Inner-class test"
                    },

                    TestNamelessClass = new TestClass.InnerTestClass
                    {
                        InnerName = "Only type test"
                    }
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

                Assert.IsNotNull(test.TestInnerClass);
                Assert.AreEqual("Inner-class test", test.TestInnerClass.InnerName);

                Assert.IsNotNull(test.TestNamelessClass);
                Assert.AreEqual("Only type test", test.TestNamelessClass.InnerName);
            }
        }
    }
}
