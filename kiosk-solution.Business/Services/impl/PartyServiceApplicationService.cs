using AutoMapper;
using AutoMapper.QueryableExtensions;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Repositories;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services.impl
{
    public class PartyServiceApplicationService : IPartyServiceApplicationService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IPartyServiceApplicationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PartyServiceApplicationService(IMapper mapper, ILogger<IPartyServiceApplicationService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<PartyServiceApplicationViewModel> Create(Guid id, PartyServiceApplicationCreateViewModel model)
        {
            var checkExist = await _unitOfWork.PartyServiceApplicationRepository.Get(c => c.PartyId.Equals(id) && c.ServiceApplicationId.Equals(model.ServiceApplicationId)).FirstOrDefaultAsync();
            if (checkExist != null)
            {
                _logger.LogInformation("You have already taken this app.");
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "You have already taken this app.");
            }
            var partyService = _mapper.Map<PartyServiceApplication>(model);

            partyService.PartyId = id;

            try
            {
                await _unitOfWork.PartyServiceApplicationRepository.InsertAsync(partyService);
                await _unitOfWork.SaveAsync();
                var result =  await _unitOfWork.PartyServiceApplicationRepository
                    .Get(a => a.Id.Equals(partyService.Id))
                    .Include(a => a.Party)
                    .Include(a => a.ServiceApplication)
                    .ProjectTo<PartyServiceApplicationViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid Data.");
                throw new ErrorResponse((int)HttpStatusCode.UnprocessableEntity, "Invalid Data.");
            }
        }
    }
}
