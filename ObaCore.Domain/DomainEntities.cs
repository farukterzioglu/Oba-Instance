using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ObaCore.Domain
{
    public class Helper
    {
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }


    public class ObaLaw
    {
        public string LawText { get; set; }
        public string Hash { get; set; }
        public string Proposer { get; set; }
        public bool IsActive { get; set; }
    }

    public class ObaLawProposal : ObaLaw
    {
        public bool IsProposalActive { get; set; }
        public List<string> VoterIds { get; set; }
    }

    public class LawAcceptanceRatioProposal
    {
        public LawAcceptanceRatioProposal()
        {
            VoterIds = new List<string>();
            IsProposalActive = true;
        }
        public double NewRatio { get; set; }
        public string Hash { get; set; }
        public bool IsProposalActive { get; set; }
        public List<string> VoterIds { get; set; }
    }
    public class AcceptanceProposal
    {
        public AcceptanceProposal()
        {
            VoterIds = new List<string>();
            IsProposalActive = true;
        }
        public string Hash { get; set; }
        public bool IsProposalActive { get; set; }
        public List<string> VoterIds { get; set; }
    }

    public class Member
    {
        public string IdNumber { get; set; }
        public string Hash { get; set; }
    }

    public class ObaInstance
    {
        public string ObaHash { get; set; }
        public List<ObaLaw> ObaLaws;
        public List<ObaLaw> ObaLawProposals;
        public List<LawAcceptanceRatioProposal> LawAcceptanceRatioProposals;
        public List<AcceptanceProposal> AcceptanceProposals;
        public List<Member> Members;

        public double LawAcceptanceRatio { get; private set; }
        public double MemberAcceptanceRatio { get; private set; }

        public ObaInstance()
        {
            ObaLaws = new List<ObaLaw>();
            ObaLawProposals = new List<ObaLaw>();
            LawAcceptanceRatioProposals = new List<LawAcceptanceRatioProposal>();
            AcceptanceProposals = new List<AcceptanceProposal>();
            Members = new List<Member>();

            LawAcceptanceRatio = 1;
            MemberAcceptanceRatio = 1;
        }

        public int MemberCount => Members.Count;

        #region Law Proposal
        public string ProposeLaw(ObaLaw obaLaw)
        {
            throw new NotImplementedException();
        }

        public void VoteForLawProposal(string obaLawHash)
        {

        }
        #endregion

        #region LawAcceptanceRatio
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
            var existing = AcceptanceProposals.FirstOrDefault(x => x.Hash == newMemberHash);
            if (existing != null) return;

            AcceptanceProposals.Add(new AcceptanceProposal() { Hash = newMemberHash });
        }

        public void VoteForNewMember(string newMemberHash, string voterHash)
        {
            //Check member
            if (Members.Count > 0)
                if (!Members.Select(x => x.Hash).Contains(voterHash)) return;

            //Check if there is an active law proposal
            var proposal = AcceptanceProposals.FirstOrDefault(x => x.Hash == newMemberHash);
            if (proposal == null || !proposal.IsProposalActive) return;

            //Can not vote two times
            if (proposal.VoterIds.Any(x => x == voterHash)) return;

            proposal.VoterIds.Add(voterHash);

            //Member accepted
            if (proposal.VoterIds.Count >= this.MemberCount * LawAcceptanceRatio)
            {
                proposal.IsProposalActive = false;
                Members.Add(new Member() { Hash = newMemberHash });
            }
        }
        #endregion
    }

}
