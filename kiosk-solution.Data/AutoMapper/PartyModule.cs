using AutoMapper;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.AutoMapper
{
    public static class PartyModule
    {
        public static void ConfigPartyModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Party, PartyViewModel>();
            mc.CreateMap<PartyViewModel, Party>();

            mc.CreateMap<Party, CreateAccountViewModel>();
            mc.CreateMap<CreateAccountViewModel, Party>();
        }
        
    }
}
