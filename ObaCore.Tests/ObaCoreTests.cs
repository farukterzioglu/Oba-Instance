using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObaCore.Domain;

namespace ObaCore.Tests
{
    [TestClass]
    public class ObaCoreTests
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

        [TestMethod]
        public void VoteForAcceptanceFirstPerson()
        {
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");

            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, memberHash);

            Assert.IsTrue(_oba.Members.Count > 0);
            Assert.IsFalse(_oba.AcceptanceProposals.Any( x=> x.IsProposalActive == true));
        }

        [TestMethod]
        public void VoteForAcceptanceCantVoteSelfIfObaHasMembers()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, memberHash);

            //Propose another person
            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");
            _oba.ProposeForAccentance(member2Hash);

            //Vote for self
            _oba.VoteForNewMember(member2Hash, member2Hash);

            //Can not vote for self 
            Assert.IsTrue(_oba.Members.Count == 1);
            Assert.IsTrue(_oba.AcceptanceProposals.First(x => x.IsProposalActive == true) != null);
        }

        [TestMethod]
        public void VoteForAcceptanceSecondPerson()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, memberHash);

            //Propose another person
            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");
            _oba.ProposeForAccentance(member2Hash);

            //Vote for new person 
            _oba.VoteForNewMember(member2Hash, memberHash);

            //Can not vote for self 
            Assert.IsTrue(_oba.Members.Count == 2);
            Assert.IsFalse(_oba.AcceptanceProposals.Any(x => x.IsProposalActive == true));
        }

        [TestMethod]
        public void VoteForAcceptance_NonMemberCantVote()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");

            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, member2Hash);
            
            Assert.IsTrue(_oba.Members.Count == 0);
            Assert.IsTrue(_oba.AcceptanceProposals.Any(x => x.IsProposalActive == true));
        }

        [TestMethod]
        public void VoteForAcceptance_NonMemberCantVote_WhenBothHasProposal()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            _oba.ProposeForAccentance(memberHash);

            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");
            _oba.ProposeForAccentance(member2Hash);

            _oba.VoteForNewMember(memberHash, member2Hash);
            _oba.VoteForNewMember(member2Hash, memberHash);

            Assert.IsTrue(_oba.Members.Count == 0);
            Assert.IsTrue(_oba.AcceptanceProposals.Any(x => x.IsProposalActive == true));
        }

        [TestMethod]
        public void VoteForAcceptance_NonMemberCantVote_WhenObaHasMember()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, memberHash);

            //Second person proposal
            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");
            _oba.ProposeForAccentance(member2Hash);

            var member3Hash = Helper.GetHashString("bf4f6997dbcb57c8492f2d4f7a39f09f231b4d61ae9372e1ac8dcbc00a763281");

            _oba.VoteForNewMember(member2Hash, member3Hash);

            Assert.IsTrue(_oba.Members.Count == 1);
            Assert.IsTrue(_oba.AcceptanceProposals.Any(x => x.IsProposalActive == true));
        }
        
        [TestMethod]
        public void VoteForAcceptance_WhenObaHasMultipleMembers_NotEnoughVote()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, memberHash);

            //Propose another person
            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");
            _oba.ProposeForAccentance(member2Hash);

            //Vote for new person 
            _oba.VoteForNewMember(member2Hash, memberHash);

            //3. Person proposal
            var member3Hash = Helper.GetHashString("bf4f6997dbcb57c8492f2d4f7a39f09f231b4d61ae9372e1ac8dcbc00a763281");
            _oba.ProposeForAccentance(member3Hash);

            //One member votes
            _oba.VoteForNewMember(member3Hash, memberHash);

            Assert.IsTrue(_oba.Members.Count == 2);
            Assert.IsTrue(_oba.AcceptanceProposals.Any(x => x.IsProposalActive == true));
        }

        [TestMethod]
        public void VoteForAcceptance_WhenObaHasMultipleMembers_EnoughVote()
        {
            //Add him/her self
            var memberHash = Helper.GetHashString("8eb6bfcf8da5c86717c2a3b927c19792b3fc55c85191f298d9ec2e4bea8ce39e");
            _oba.ProposeForAccentance(memberHash);
            _oba.VoteForNewMember(memberHash, memberHash);

            //Propose another person
            var member2Hash = Helper.GetHashString("e6e10d36854ed0ade3c5677d2cde228be5205712e0ea3456474ca38512f1025c");
            _oba.ProposeForAccentance(member2Hash);

            //Vote for new person 
            _oba.VoteForNewMember(member2Hash, memberHash);

            //3. Person proposal
            var member3Hash = Helper.GetHashString("bf4f6997dbcb57c8492f2d4f7a39f09f231b4d61ae9372e1ac8dcbc00a763281");
            _oba.ProposeForAccentance(member3Hash);

            //One member votes
            _oba.VoteForNewMember(member3Hash, memberHash);
            _oba.VoteForNewMember(member3Hash, member2Hash);

            Assert.IsTrue(_oba.Members.Count == 3);
            Assert.IsFalse(_oba.AcceptanceProposals.Any( x=> x.IsProposalActive == true));
        }

    }
}
