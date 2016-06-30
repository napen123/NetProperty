using NetProperty;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetPropertyTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void SimpleTest()
        {
            var property = new PropertyFile("tests/simple.property");
            
            Assert.AreEqual("Hello, World!", property["message"]);
        }

        [TestMethod]
        public void ValueTest()
        {
            var property = new PropertyFile<int>("tests/value.property");

            Assert.AreEqual(100, property["int"]);
        }

        [TestMethod]
        public void SaveTest()
        {
            // Simple test
            {
                new PropertyFile("tests/save.simple.property", false)
                {
                    ["property.first"] = "First Property",
                    ["property.second"] = "Second Property",
                    ["property.third"] = "Third Property"
                }.Save();

                var stringProperty = new PropertyFile("tests/save.simple.property");

                Assert.AreEqual("First Property", stringProperty["property.first"]);
                Assert.AreEqual("Second Property", stringProperty["property.second"]);
                Assert.AreEqual("Third Property", stringProperty["property.third"]);
            }

            // Varied test
            {
                new PropertyFile("tests/save.varied.property", false)
                {
                    ["nospace"] = "No spaces",
                    ["space"] = "    Four spaces"
                }.Save();

                var variedProperty = new PropertyFile("tests/save.varied.property");

                Assert.AreEqual("No spaces", variedProperty["nospace"]);
                Assert.AreEqual("    Four spaces", variedProperty["space"]);
            }

            // Typed test
            {
                new PropertyFile<int>("tests/save.typed.int.property", false) {["int"] = 100}.Save();
                var integerProperty = new PropertyFile<int>("tests/save.typed.int.property");
                Assert.AreEqual(100, integerProperty["int"]);

                new PropertyFile<float>("tests/save.typed.float.property", false) {["float"] = 0.1f}.Save();
                var floatProperty = new PropertyFile<float>("tests/save.typed.float.property");
                Assert.AreEqual(0.1f, floatProperty["float"]);
                
                new PropertyFile<bool>("tests/save.typed.bool.property", false) {["true"] = true, ["false"] = false}.Save();
                var boolProperty = new PropertyFile<bool>("tests/save.typed.bool.property");
                Assert.AreEqual(true, boolProperty["true"]);
                Assert.AreEqual(false, boolProperty["false"]);
            }
        }
    }
}
