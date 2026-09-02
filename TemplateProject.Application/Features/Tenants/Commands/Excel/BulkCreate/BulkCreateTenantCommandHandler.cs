using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate
{
    public class BulkCreateTenantCommandHandler : IRequestHandler<BulkCreateTenantCommand, Result<List<Guid>>>
    {
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;

        public BulkCreateTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<List<Guid>>> Handle(BulkCreateTenantCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return Result.Failure<List<Guid>>(new Error("Yüklenecek geçerli bir veri bulunamadı.", "TENANT_BULK_CREATE_NO_DATA"));
            }
            var resultIdList = new List<Guid>();
            var codeIsNullList = request.Items.Where(w => !w.Code.HasValue);
            if (codeIsNullList.Any())
            {
                var tenantList = _mapper.Map<List<Tenant>>(codeIsNullList);

                await _tenantRepository.AddRangeAsync(tenantList, cancellationToken);
                resultIdList.AddRange(tenantList.Select(t => t.Id).ToList());
            }
            var codeIsNotNullList = request.Items.Where(w => w.Code.HasValue).Select(s=>s.Code.Value).ToList();

            var dataListForUpdate = _tenantRepository.GetByRevNumsAsync(codeIsNotNullList, cancellationToken);

            var resultHasNoUpdateData = new List<long>();
            foreach (var code in codeIsNotNullList)
            {
                var dataForUpdate = dataListForUpdate.Result.SingleOrDefault(s => s.RevNum == code);
                var requestDto = request.Items.Single(w => w.Code == code);
                if (dataForUpdate != null)
                { 
                    var result = _mapper.Map(requestDto, dataForUpdate);
                    _tenantRepository.Update(result);
                    resultIdList.Add(result.Id);
                }
                else
                {
                    resultHasNoUpdateData.Add(code);
                } 
            }
            if (resultHasNoUpdateData.Any())
            {
                Result.Failure(new Error("HasNoUpdateData", string.Format("Girmiş Olduğunuz Kod(lar)a Ship Bir Data Bulunamadı ({0})",string.Join(", ", resultHasNoUpdateData))));
            }

            return Result.Success(resultIdList);
        }
    }
}