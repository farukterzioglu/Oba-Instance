using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObaCore.Domain;

namespace ObaCore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private ObaInstance _oba;

        [TestInitialize]
        public void Setup()
        {
            //8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e
            //e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c
            //bf4f6997dbcb57c8492f2d4f7a39f09f231b4d61ae9372e1ac8dcbc00a763281

            _oba = new ObaInstance();
        }

        [TestMethod]
        public void AddtoAcceptanceProposals()
        {
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");

            _oba.ProposeForAccentance(memberHash);

            var first = _oba.AcceptanceProposals.First( x=> x.Hash == memberHash);

            Assert.IsTrue(first.IsProposalActive);
        }

        [TestMethod]
        public void AddstoAcceptanceProposals()
        {
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");

            _oba.ProposeForAccentance(memberHash);
            _oba.ProposeForAccentance(memberHash);

            var first = _oba.AcceptanceProposals.First(x => x.Hash == memberHash);

            Assert.IsTrue(first.IsProposalActive);
        }
    }
}
