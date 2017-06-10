using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC2Bot.Helpers;

namespace SC2Bot.Tests.Helpers
{
    [TestClass]
    public class Parser_Constructor
    {
        [TestMethod]
        public void Should_Create_Parser()
        {
            Parser parser = new Parser();
        }

        [DataTestMethod]
        [DataRow("!foo bar", "foo", new string[] { "bar" })]
        [DataRow("!bar baz quz", "bar", new string[] { "baz", "quz" })]
        [DataRow("!bar", "bar", null)]
        [DataRow("!", null, null)]
        public void Should_Create_Parser_With_Query(string query, string method, string[] parameters)
        {
            Parser parser = new Parser(query);
            Assert.AreEqual(parser.GetQuery(), query);
            Assert.AreEqual(parser.Method, method);
            CollectionAssert.AreEqual(parser.Parameters, parameters);
        }
    }
}
