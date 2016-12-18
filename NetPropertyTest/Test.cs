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
        
        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyException))]
        public void ErrorTest()
        {
            new PropertyFile().Load("tests/error.property");
        }
    }
}
