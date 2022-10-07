using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HcmMemberSearch.Modals
{
    public class Claim
    {
        [Required]
        public int ClaimId { get; set; }
        [Required]
        public string ClaimType { get; set; }
        [Required]
        public int ClaimAmount { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Remarks { get; set; }
        [Required]
        public DateTime ClaimDate { get; set; }
        [Required]
        public int MemberId { get; set; }
    }
}
