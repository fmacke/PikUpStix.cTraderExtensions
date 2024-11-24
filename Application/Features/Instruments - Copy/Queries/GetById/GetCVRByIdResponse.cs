using Domain.Abstractions;
using Domain.Entities.CVRs;

namespace Application.Features.CVRs.Queries.GetById
{
    public class GetCVRByIdResponse
    {
        public int Id { get; set; }
        public string GetdBy { get; set; }
        public DateTime GetdOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string InstanceName { get; set; }
        public bool IsDeleted { get; set; }
        public GetCVRContractInfo ContractInfo { get; set; }
        public GetCVRInput Input { get; set; }
        public string PreparedBy { get; set; }
        public DateTime? PreparedDate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
    public class GetCVRContractInfo
    {
        public int Id { get; set; }
        public string ContractName { get; set; }
        public string ContractNumber { get; set; }
        public string QuantitySurveyor { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public float TenderValue { get; set; }
        public float TenderGrossMargin { get; set; }
        public int WeeksDelay { get; set; }
        public string AwardedEoT { get; set; }
        public string LADamages { get; set; }
        public DateTime? LastMonthEnd { get; set; }
        public DateTime? ThisMonthEnd { get; set; }
    }
    public class GetCVRInput
    {
        public int Id { get; set; }
        public GetCVRInputItem LastMonthFigures { get; set; }
        public int LastMonthFiguresId { get; set; }
        public GetCVRInputItem ThisMonthFigures { get; set; }
        public int ThisMonthFiguresId { get; set; }
        public GetCVRCashflow Cashflow { get; set; }
        public List<GetCVRForecast> Forecasts { get; set; }
    }
    public class GetCVRForecast : BaseEntity
    {
        public int Id { get; set; }
        public DateTime ForecastDate { get; set; }
        public float ForecastValue { get; set; }
        public float ForecastMargin { get; set; }
        public int CVRInputId { get; set; }
    }
    public class GetCVRInputItem
    {
        public int Id { get; set; }
        public decimal? GrossValuation { get; set; }
        public decimal? UnderOverValuation { get; set; }
        public decimal? InternalValue { get; set; }
        public decimal? ComputerCosts { get; set; }
        public decimal? SubcontractLiability { get; set; }
        public decimal? Maintenance { get; set; }
        public decimal? PlantAdjustment { get; set; }
        public decimal? MaterialsOnSite { get; set; }
        public decimal? SpecificCostReserve { get; set; }
        public decimal? TotalCosts { get; set; }
    }
    public class GetCVRCashflow
    {
        public int Id { get; set; }
        public float CurentMonthCertificate { get; set; }
        public DateTime? CertificateDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public float TotalPaidToDate { get; set; }
        public float AmountOutstanding { get; set; }
    }
}
