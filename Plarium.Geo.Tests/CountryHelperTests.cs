using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plarium.Geo.Helpers;
using Plarium.Geo.Services;

namespace Plarium.Geo.Tests
{
    [TestClass]
    public class CountryHelperTests
    {
        ICountryService _helper;
        string _country;
        string _code;
        string _codeDial;
        byte _byte;

        [TestInitialize]
        public void Init()
        {
            this._helper = CountryHelper.Default;
            this._country = "Ukraine";
            this._code = "UA";
            _codeDial = "380";
            _byte = 222;
        }

        [TestMethod()]
        public void GetCountryNameByCodeTest()
        {
            var result = this._helper.GetCountryName(this._code);
            Assert.AreEqual(result, this._country);
        }

        [TestMethod()]
        public void GetCountryNameByByteTest()
        {
            var result = this._helper.GetCountryName(this._byte);
            Assert.AreEqual(result, this._country);
        }

        [TestMethod()]
        public void CountryToByteTest()
        {
            var result = this._helper.CountryToByte(this._code);
            Assert.AreEqual(result, this._byte);
        }

        [TestMethod()]
        public void GetCountryCodeTest()
        {
            var result = this._helper.GetCountryCode(this._byte);
            Assert.AreEqual(result, this._code);
        }

        [TestMethod()]
        public void GetDialCodeTest()
        {
            var result = this._helper.GetDialCode(this._code);
            Assert.AreEqual(result, this._codeDial);
        }
    }
}
