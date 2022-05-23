using System.Threading.Tasks;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Data.Repositories
{
    public interface IUnitOfWork
    {
        IPartyRepository  PartyRepository { get; }
        IRoleRepository RoleRepository { get; }
        IKioskRepository KioskRepository { get; }
        void Save();
        Task SaveAsync();
    }
}