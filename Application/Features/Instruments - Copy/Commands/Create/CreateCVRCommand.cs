using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Features.CVRs.Commands.Create;
using Application.Features.Projects.Commands.Create;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities.CVRs;
using MediatR;

namespace Application.Features.CVRs.Commands.Create
{
    public partial class CreateCVRCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public CreateCVRContractInfo ContractInfo { get; set; }
        public CreateCVRInput Input { get; set; }
        public string PreparedBy { get; set; }
        public DateTime? PreparedDate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }        
    }
    public class CreateCVRContractInfo
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
    public class CreateCVRInput
    {
        public int Id { get; set; }
        public CreateCVRInputItem LastMonthFigures { get; set; }
        public int LastMonthFiguresId { get; set; }
        public CreateCVRInputItem ThisMonthFigures { get; set; }
        public int ThisMonthFiguresId { get; set; }
        public CreateCVRCashflow Cashflow { get; set; }
        public List<CreateCVRForecast> Forecasts { get; set; }
    }
    public class CreateCVRForecast : BaseEntity
    {
        public int Id { get; set; }
        public DateTime ForecastDate { get; set; }
        public float ForecastValue { get; set; }
        public float ForecastMargin { get; set; }
        public int CVRInputId { get; set; }
    }
    public class CreateCVRInputItem
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
    public class CreateCVRCashflow 
    {
        public int Id { get; set; }
        public float CurentMonthCertificate { get; set; }
        public DateTime? CertificateDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public float TotalPaidToDate { get; set; }
        public float AmountOutstanding { get; set; }
    }

    public class CreateCVRCommandHandler : IRequestHandler<CreateProjectCommand, Result<int>>
    {
        private readonly ICVRRepository _cvrRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateCVRCommandHandler(ICVRRepository cvrRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cvrRepository = cvrRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var cvr = _mapper.Map<CVR>(request);
            await _cvrRepository.InsertAsync(cvr);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(cvr.Id);
        }
    }
}
