using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObaCore.Domain
{
    public class ObaInstance
    {
        public string ObaHash { get; set; }
        public List<ObaLaw> ObaLaws;
        public List<ObaLaw> ObaLawProposals;
        public List<LawAcceptanceRatioProposal> LawAcceptanceRatioProposals;
        public List<AcceptanceProposal> AcceptanceProposals;
        public List<Member> Members;

        public int ContributedValue { get; set; }

        public double LawAcceptanceRatio { get; private set; }
        public double MemberAcceptanceRatio { get; private set; }


        private int NewMemberContributionValue { get; set; }

        public ObaInstance()
        {
            ObaLaws = new List<ObaLaw>();

            ObaLawProposals = new List<ObaLaw>();
            LawAcceptanceRatioProposals = new List<LawAcceptanceRatioProposal>();

            AcceptanceProposals = new List<AcceptanceProposal>();
            Members = new List<Member>();

            LawAcceptanceRatio = 1;
            MemberAcceptanceRatio = 1;
            NewMemberContributionValue = 1;
        }

        public int MemberCount => Members.Count;

        #region Law Proposal
        public string ProposeLaw(ObaLawProposal obaLaw)
        {
            throw new NotImplementedException();
        }

        public void VoteForLawProposal(string obaLawHash)
        {

        }
        #endregion

        #region LawAcceptanceRatio Proposal
        private void ChangeLawAcceptanceRatio(double newRatio)
        {
            this.LawAcceptanceRatio = newRatio;
        }

        public string ProposeLawAcceptanceRatio(double newRatio)
        {
            var existing = LawAcceptanceRatioProposals.FirstOrDefault(x => x.NewRatio == newRatio);
            if (existing != null) return existing.Hash;

            string newHash = Helper.GetHashString(newRatio.ToString(CultureInfo.InvariantCulture));

            LawAcceptanceRatioProposals.Add(new LawAcceptanceRatioProposal() { Hash = newHash, NewRatio = newRatio });

            return newHash;
        }

        public void VoteLawProposalAcceptanceRatio(string hash, string voterId)
        {
            //Check if there is an active law proposal
            var proposal = LawAcceptanceRatioProposals.FirstOrDefault(x => x.Hash == hash);
            if (proposal == null || !proposal.IsProposalActive) return;

            //Can not vote two times
            if (proposal.VoterIds.Any(x => x == voterId)) return;

            proposal.VoterIds.Add(voterId);

            //Law accepted
            if (proposal.VoterIds.Count >= this.MemberCount * LawAcceptanceRatio)
            {
                proposal.IsProposalActive = false;
                ChangeLawAcceptanceRatio(proposal.NewRatio);
            }
        }
        #endregion

        #region New Member Voting

        public void ProposeForAccentance(string newMemberHash)
        {
            if(this.ContributedValue != NewMemberContributionValue) return;

            //Check if already proposed 
            var existing = AcceptanceProposals.FirstOrDefault(x => x.Hash == newMemberHash);
            if (existing != null) return;

            AcceptanceProposals.Add(new AcceptanceProposal() { Hash = newMemberHash });
        }

        public void VoteForNewMember(string newMemberHash, string voterHash)
        {
            //Check member
            if (Members.Count > 0)
            {
                //Only exsiting members can vote
                if (!Members.Select(x => x.Hash).Contains(voterHash)) return;
            }
            else
            {
                //If there isn't any member, only self voting is accepted
                if (newMemberHash != voterHash) return;
            }


            //Check if there is an active member proposal
            var proposal = AcceptanceProposals.FirstOrDefault(x => x.Hash == newMemberHash);
            if (proposal == null || !proposal.IsProposalActive) return;

            //Can not vote two times
            if (proposal.VoterIds.Any(x => x == voterHash)) return;

            //Update voters 
            proposal.VoterIds.Add(voterHash);

            //Check if member accepted
            if (proposal.VoterIds.Count >= this.MemberCount * MemberAcceptanceRatio)
            {
                proposal.IsProposalActive = false;
                Members.Add(new Member() { Hash = newMemberHash });
            }
        }
        #endregion
    }

}
