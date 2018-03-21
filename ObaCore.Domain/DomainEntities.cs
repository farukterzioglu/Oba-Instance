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

    
}
