using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObaCore.Domain;

namespace ObaCore.Tests
{
    [TestClass]
    public class LawProposalTests
    {
        private ObaInstance _oba;

        [TestInitialize]
        public void Setup()
        {
            _oba = new ObaInstance();
            _oba.Members.Add(new Member(){ Hash = "123"});
        }

        [TestMethod]
        public void HashLenghtShouldBeEqual()
        {
            string lawText = "Every one is equal";
            string hash = Helper.GetHashString(lawText);

            string law2Text = "Every Oba's law is only for themselves.";
            string hash2 = Helper.GetHashString(law2Text);

            Assert.AreEqual(hash.Length, hash2.Length);
        }

        [TestMethod]
        public void ShouldReturnHashAfterPropose()
        {
            _oba.HashOfInvoker = "123";
            string lawText = "Every one is equal";
            string hash = Helper.GetHashString(lawText);

            string returnedHash = _oba.ProposeLaw(lawText);

            Assert.AreEqual(hash, returnedHash);
        }

        [TestMethod]
        public void ShouldAddToPorposals()
        {
            _oba.HashOfInvoker = "123";
            _oba.ProposeLaw("Every one is equal");

            Assert.AreEqual(1, _oba.ObaLawProposals.Count);
            Assert.AreEqual(0, _oba.ObaLaws.Count);
        }

        [TestMethod]
        public void ShouldOneVoteBeEnough()
        {
            _oba.HashOfInvoker = "123";
            string returnedHash = _oba.ProposeLaw("Every one is equal");

            _oba.VoteForLawProposal(returnedHash);

            Assert.AreEqual(1, _oba.ObaLaws.Count);
            Assert.IsFalse(_oba.ObaLawProposals.Any(x => x.IsProposalActive == true));
        }

        [TestMethod]
        public void ShouldRequireTwoVote()
        {
            _oba.Members.Add(new Member() {Hash = "987"});

            _oba.HashOfInvoker = "123";
            string returnedHash = _oba.ProposeLaw("Every one is equal");

            _oba.VoteForLawProposal(returnedHash);

            Assert.AreEqual(0, _oba.ObaLaws.Count);
            Assert.IsTrue(_oba.ObaLawProposals.Any(x => x.IsProposalActive == true));
        }

        [TestMethod]
        public void ShouldTwoVoteBeEnough()
        {
            _oba.Members.Add(new Member() { Hash = "987" });

            _oba.HashOfInvoker = "123";
            string returnedHash = _oba.ProposeLaw("Every one is equal");

            _oba.VoteForLawProposal(returnedHash);

            _oba.HashOfInvoker = "987";
            _oba.VoteForLawProposal(returnedHash);

            Assert.AreEqual(1, _oba.ObaLaws.Count);
            Assert.IsFalse(_oba.ObaLawProposals.Any(x => x.IsProposalActive == true));
        }
    }
}
