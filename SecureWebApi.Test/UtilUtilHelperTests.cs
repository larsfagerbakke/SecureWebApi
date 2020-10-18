using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureWebApi.Tests
{
    [TestClass]
    public class UtilUtilHelperTests
    {
        [TestMethod]
        public void GenerateRandomString_Test()
        {
            var ret = SecureWebApi.Shared.Helpers.Util.Util.GenerateRandomString(10);

            Assert.AreEqual(ret.Length, 10);
        }
    }
}
