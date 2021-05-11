using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rainbow.MultiTenancy.AspNetCore.Text;

namespace Rainbow.MultiTenancy.AspNetCore.Test.Text
{
    [TestClass]
    public class FormattedStringValueExtracterTest
    {
        [TestMethod]
        public void ExtractText()
        {
            var result = FormattedStringValueExtracter.Extract("liuhao.test.com", "{tenant}.test.com");

            Assert.IsTrue(result.IsMatch);
            Assert.AreEqual(1, result.Matches.Count);
            Assert.AreEqual("tenant", result.Matches[0].Key);
            Assert.AreEqual("liuhao", result.Matches[0].Value);
        }
    }
}
