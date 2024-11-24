using Domain.Entities.CVRs;

namespace Application.Features.CVRs.Queries.GetAllCached
{
    public class GetAllCVRCachedResponse
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string InstanceName { get; set; }
        public bool IsDeleted { get; set; }
        public CVRContractInfo ContractInfo { get; set; }
        public CVRInput Input { get; set; }
        public string PreparedBy { get; set; }
        public DateTime? PreparedDate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
}