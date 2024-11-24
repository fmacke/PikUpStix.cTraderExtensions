using MediatR;
using Application.Common.Interfaces;
using Application.Interfaces.Repositories;
using Application.Common.Results;
using Domain.Entities.CVRs;
using AutoMapper;

namespace Application.Features.CVRs.Commands.Update
{
    public class UpdateCVRCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public UpdateCVRContractInfo ContractInfo { get; set; }
        public UpdateCVRInput Input { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string PreparedBy { get; set; }
        public DateTime? PreparedDate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class UpdateCVRContractInfo
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
    public class UpdateCVRInput
    {
        public int Id { get; set; }
        public UpdateCVRInputItem LastMonthFigures { get; set; }
        public int LastMonthFiguresId { get; set; }
        public UpdateCVRInputItem ThisMonthFigures { get; set; }
        public int ThisMonthFiguresId { get; set; }
        public UpdateCVRCashflow Cashflow { get; set; }
        public List<UpdateCVRForecast> Forecasts { get; set; }
    }    
    public class UpdateCVRInputItem
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
    public class UpdateCVRCashflow
    {
        public int Id { get; set; }
        public float CurentMonthCertificate { get; set; }
        public DateTime? CertificateDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public float TotalPaidToDate { get; set; }
        public float AmountOutstanding { get; set; }
    }
    public class UpdateCVRForecast
    {
        public int Id { get; set; }
        public DateTime ForecastDate { get; set; }
        public float ForecastValue { get; set; }
        public float ForecastMargin { get; set; }
        public int CVRInputId { get; set; }
    }
    public class UpdateCVRCommandHandler : IRequestHandler<UpdateCVRCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICVRRepository _cvrRepository;
        private readonly IMapper _mapper;

        public UpdateCVRCommandHandler(ICVRRepository cvrRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cvrRepository = cvrRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateCVRCommand command, CancellationToken cancellationToken)
        {
            var cvr = await _cvrRepository.GetByIdAsync(command.Id);

            if (cvr == null)
            {
                return Result<int>.Fail($"CVR Not Found.");
            }
            else
            {
                UpdateModifiedCVR(ref cvr, command);
                await _cvrRepository.UpdateAsync(cvr);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(cvr.Id);
            }
        }

        private void UpdateModifiedCVR(ref CVR cvr, UpdateCVRCommand command)
        {
            cvr.PreparedBy = command.PreparedBy ?? cvr.PreparedBy;
            if (!command.PreparedDate.Equals(cvr.PreparedDate))
                cvr.PreparedDate = command.PreparedDate;

            cvr.ReviewedBy = command.ReviewedBy ?? cvr.ReviewedBy;
            if (!command.ReviewedDate.Equals(cvr.ReviewedDate))
                cvr.ReviewedDate = command.ReviewedDate;
            UpdateModifiedContractInfo(ref cvr, command.ContractInfo);
            UpdateModifiedCVRInputs(ref cvr, command.Input);

        }

        private void UpdateModifiedCVRInputs(ref CVR cvr, UpdateCVRInput updatedInput)
        {
            // Update ThisMonthFigures
            cvr.Input.ThisMonthFigures.GrossValuation = updatedInput.ThisMonthFigures.GrossValuation ?? cvr.Input.ThisMonthFigures.GrossValuation;
            cvr.Input.ThisMonthFigures.UnderOverValuation = updatedInput.ThisMonthFigures.UnderOverValuation ?? cvr.Input.ThisMonthFigures.UnderOverValuation;
            cvr.Input.ThisMonthFigures.InternalValue = updatedInput.ThisMonthFigures.InternalValue ?? cvr.Input.ThisMonthFigures.InternalValue;
            cvr.Input.ThisMonthFigures.ComputerCosts = updatedInput.ThisMonthFigures.ComputerCosts ?? cvr.Input.ThisMonthFigures.ComputerCosts;
            cvr.Input.ThisMonthFigures.SubcontractLiability = updatedInput.ThisMonthFigures.SubcontractLiability ?? cvr.Input.ThisMonthFigures.SubcontractLiability;
            cvr.Input.ThisMonthFigures.Maintenance = updatedInput.ThisMonthFigures.Maintenance ?? cvr.Input.ThisMonthFigures.Maintenance;
            cvr.Input.ThisMonthFigures.PlantAdjustment = updatedInput.ThisMonthFigures.PlantAdjustment ?? cvr.Input.ThisMonthFigures.PlantAdjustment;
            cvr.Input.ThisMonthFigures.MaterialsOnSite = updatedInput.ThisMonthFigures.MaterialsOnSite ?? cvr.Input.ThisMonthFigures.MaterialsOnSite;
            cvr.Input.ThisMonthFigures.SpecificCostReserve = updatedInput.ThisMonthFigures.SpecificCostReserve ?? cvr.Input.ThisMonthFigures.SpecificCostReserve;
            cvr.Input.ThisMonthFigures.TotalCosts = updatedInput.ThisMonthFigures.TotalCosts ?? cvr.Input.ThisMonthFigures.TotalCosts;
            //Update LastMonthFigures
            cvr.Input.LastMonthFigures.GrossValuation = updatedInput.LastMonthFigures.GrossValuation ?? cvr.Input.LastMonthFigures.GrossValuation;
            cvr.Input.LastMonthFigures.UnderOverValuation = updatedInput.LastMonthFigures.UnderOverValuation ?? cvr.Input.LastMonthFigures.UnderOverValuation;
            cvr.Input.LastMonthFigures.InternalValue = updatedInput.LastMonthFigures.InternalValue ?? cvr.Input.LastMonthFigures.InternalValue;
            cvr.Input.LastMonthFigures.ComputerCosts = updatedInput.LastMonthFigures.ComputerCosts ?? cvr.Input.LastMonthFigures.ComputerCosts;
            cvr.Input.LastMonthFigures.SubcontractLiability = updatedInput.LastMonthFigures.SubcontractLiability ?? cvr.Input.LastMonthFigures.SubcontractLiability;
            cvr.Input.LastMonthFigures.Maintenance = updatedInput.LastMonthFigures.Maintenance ?? cvr.Input.LastMonthFigures.Maintenance;
            cvr.Input.LastMonthFigures.PlantAdjustment = updatedInput.LastMonthFigures.PlantAdjustment ?? cvr.Input.LastMonthFigures.PlantAdjustment;
            cvr.Input.LastMonthFigures.MaterialsOnSite = updatedInput.LastMonthFigures.MaterialsOnSite ?? cvr.Input.LastMonthFigures.MaterialsOnSite;
            cvr.Input.LastMonthFigures.SpecificCostReserve = updatedInput.LastMonthFigures.SpecificCostReserve ?? cvr.Input.LastMonthFigures.SpecificCostReserve;
            cvr.Input.LastMonthFigures.TotalCosts = updatedInput.LastMonthFigures.TotalCosts ?? cvr.Input.LastMonthFigures.TotalCosts;

            UpdateModifiedCashflow(ref cvr, updatedInput.Cashflow);
            UpdateModifiedForecasts(ref cvr, updatedInput.Forecasts);
        }

        private void UpdateModifiedForecasts(ref CVR cvr, List<UpdateCVRForecast> forecasts)
        {
            foreach(var item in forecasts)
            {
                if (cvr.Input.Forecasts.Any(x => x.Id == item.Id))
                {
                    if (!item.ForecastDate.Equals(cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastDate))
                        cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastDate = item.ForecastDate;
                    if (!item.ForecastValue.Equals(cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastValue))
                        cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastValue = item.ForecastValue;
                    if (!item.ForecastMargin.Equals(cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastMargin))
                        cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastMargin = item.ForecastMargin;
                    if (!item.ForecastDate.Equals(cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastDate))
                        cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastDate = item.ForecastDate;
                    if (!item.ForecastDate.Equals(cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastDate))
                        cvr.Input.Forecasts.First(x => x.Id == item.Id).ForecastDate = item.ForecastDate;
                }
                else
                {
                    var newForecast = _mapper.Map<CVRForecast>(item);
                    cvr.Input.Forecasts.Add(newForecast);
                }
            }
        }

        private void UpdateModifiedCashflow(ref CVR cvr, UpdateCVRCashflow cashflow)
        {
            if (!cashflow.CurentMonthCertificate.Equals(cvr.Input.Cashflow.CurentMonthCertificate))
                cvr.Input.Cashflow.CurentMonthCertificate = cashflow.CurentMonthCertificate;
            if (!cashflow.CertificateDate.Equals(cvr.Input.Cashflow.CertificateDate))
                cvr.Input.Cashflow.CertificateDate = cashflow.CertificateDate;
            if (!cashflow.PaymentDate.Equals(cvr.Input.Cashflow.PaymentDate))
                cvr.Input.Cashflow.PaymentDate = cashflow.PaymentDate;
            if (!cashflow.TotalPaidToDate.Equals(cvr.Input.Cashflow.TotalPaidToDate))
                cvr.Input.Cashflow.TotalPaidToDate = cashflow.TotalPaidToDate;
            if (!cashflow.AmountOutstanding.Equals(cvr.Input.Cashflow.AmountOutstanding))
                cvr.Input.Cashflow.AmountOutstanding = cashflow.AmountOutstanding;
        }

        private void UpdateModifiedContractInfo(ref CVR cvr, UpdateCVRContractInfo updatedContractInfo)
        {
            cvr.ContractInfo.ContractName = updatedContractInfo.ContractName ?? cvr.ContractInfo.ContractName;
            cvr.ContractInfo.ContractNumber = updatedContractInfo.ContractNumber ?? cvr.ContractInfo.ContractNumber;
            cvr.ContractInfo.QuantitySurveyor = updatedContractInfo.QuantitySurveyor ?? cvr.ContractInfo.QuantitySurveyor;

            if (!updatedContractInfo.ContractStartDate.Equals(cvr.ContractInfo.ContractStartDate))
                cvr.ContractInfo.ContractStartDate = updatedContractInfo.ContractStartDate;
            if (!updatedContractInfo.ContractEndDate.Equals(cvr.ContractInfo.ContractEndDate))
                cvr.ContractInfo.ContractEndDate = updatedContractInfo.ContractEndDate;

            if (!updatedContractInfo.TenderValue.Equals(cvr.ContractInfo.TenderValue))
                cvr.ContractInfo.TenderValue = updatedContractInfo.TenderValue;
            if (!updatedContractInfo.TenderGrossMargin.Equals(cvr.ContractInfo.TenderGrossMargin))
                cvr.ContractInfo.TenderGrossMargin = updatedContractInfo.TenderGrossMargin;
            if (!updatedContractInfo.WeeksDelay.Equals(cvr.ContractInfo.WeeksDelay))
                cvr.ContractInfo.WeeksDelay = updatedContractInfo.WeeksDelay;

            cvr.ContractInfo.AwardedEoT = updatedContractInfo.AwardedEoT ?? cvr.ContractInfo.AwardedEoT;
            cvr.ContractInfo.LADamages = updatedContractInfo.LADamages ?? cvr.ContractInfo.LADamages;

            if (!updatedContractInfo.LastMonthEnd.Equals(cvr.ContractInfo.LastMonthEnd))
                cvr.ContractInfo.LastMonthEnd = updatedContractInfo.LastMonthEnd;
            if (!updatedContractInfo.ThisMonthEnd.Equals(cvr.ContractInfo.ThisMonthEnd))
                cvr.ContractInfo.ThisMonthEnd = updatedContractInfo.ThisMonthEnd;
        }
    }
}