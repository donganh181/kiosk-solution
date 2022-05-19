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
        public IPartyRepository PartyRepository { get; set; }
        
        
        public UnitOfWork(Kiosk_PlatformContext context, IPartyRepository PartyRepository, IRoleRepository RoleRepository)
        {
            _context = context;
            this.PartyRepository = PartyRepository;
            this.RoleRepository = RoleRepository;
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