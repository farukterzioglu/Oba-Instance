using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObaCore.Domain;

namespace ObaCore.Tests
{
    public class LawProposalTests
    {
        private ObaInstance _oba;

        public void Setup()
        {
            _oba = new ObaInstance();
            _oba.Members.Add(new Member(){ Hash = "123"});
        }

        public void HashLenghtShouldBeEqual()
        {
            string lawText = "Every one is equal";
            string hash = Helper.GetHashString(lawText);

            string law2Text = "Every Oba's law is only for themselves.";
            string hash2 = Helper.GetHashString(lawText);

            Assert.AreEqual(hash.Length, hash2.Length);
        }

        public void ShouldReturnHashAfterPropose()
        {
            string lawText = "Every one is equal";
            string hash = Helper.GetHashString(lawText);

            string returnedHash = _oba.ProposeLaw(new ObaLawProposal(){  LawText = lawText});

            Assert.AreEqual(hash, returnedHash);
        }
    }
}
