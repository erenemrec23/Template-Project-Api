using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Feedbacks.DTOs;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Feedbacks.Queries.GetList
{
    internal class GetListFeedBackQueryHandler : IRequestHandler<GetListFeedBackQuery, Result<Paginate<FeedBackListItemDto>>>
    {
        private readonly IFeedBackRepository _feedbackRepository;
        private readonly IFileStorageService _fileStorageService;
        public GetListFeedBackQueryHandler(IFeedBackRepository feedbackRepository, IFileStorageService fileStorageService)
        {
            _feedbackRepository = feedbackRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Paginate<FeedBackListItemDto>>> Handle(GetListFeedBackQuery request, CancellationToken cancellationToken)
        {
            var result = await _feedbackRepository.GetDtoListAsync(request, cancellationToken);


            foreach (var item in result.Items)
                item.ScreenshotPath = _fileStorageService.ResolveUrl(item.ScreenshotPath);

            return Result.Success(result);
        } 
    }
}
