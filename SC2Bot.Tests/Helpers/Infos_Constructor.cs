using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SC2Bot.Helpers;

namespace SC2Bot.Tests.Helpers
{
    [TestClass]
    public class Infos_Constructor
    {
        [TestMethod]
        public void Should_Create_Infos()
        {
            Infos infos = new Infos();
            Assert.IsNotNull(infos.Aligulac);
            Assert.AreNotEqual(infos.BotAPI, "");
            Assert.AreNotEqual(infos.ServerName, "");
        }
    }
}
