using System;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace kiosk_solution.Data.Repositories.impl
{
    public class UnitOfWork : IUnitOfWork
    {
        public Kiosk_PlatformContext _context { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IKioskRepository KioskRepository { get; set; }
        public IPartyRepository PartyRepository { get; set; }
        public IKioskLocationRepository KioskLocationRepository { get; set; }
        public IScheduleRepository ScheduleRepository { get; set; }
        public ITemplateRepository TemplateRepository { get; set; }
        public IScheduleTemplateRepository ScheduleTemplateRepository { get; set; }
        public IServiceApplicationRepository ServiceApplicationRepository { get; set; }
        public IServiceApplicationPublishRequestRepository ServiceApplicationPublishRequestRepository { get; set; }

        public UnitOfWork(Kiosk_PlatformContext context, IRoleRepository roleRepository, IKioskRepository kioskRepository, 
            IPartyRepository partyRepository, IKioskLocationRepository kioskLocationRepository, IScheduleRepository scheduleRepository,
            ITemplateRepository templateRepository, IScheduleTemplateRepository scheduleTemplateRepository, IServiceApplicationRepository serviceApplicationRepository,
            IServiceApplicationPublishRequestRepository serviceApplicationPublishRequestRepository)
        {
            _context = context;
            RoleRepository = roleRepository;
            KioskRepository = kioskRepository;
            PartyRepository = partyRepository;
            KioskLocationRepository = kioskLocationRepository;
            ScheduleRepository = scheduleRepository;
            TemplateRepository = templateRepository;
            ScheduleTemplateRepository = scheduleTemplateRepository;
            ServiceApplicationRepository = serviceApplicationRepository;
            ServiceApplicationPublishRequestRepository = serviceApplicationPublishRequestRepository;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}