using AutoMapper;
using GloBus.Data.DTOs;
using GloBus.Data.Models;

namespace GloBus_backend.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<User, CreditDTO>() ;
            CreateMap<CreditDTO,User>() ;

            CreateMap<User, InspectorDTO>();
            CreateMap<InspectorDTO, User>();

            CreateMap<User, UserRegisterDTO>();
            CreateMap<UserRegisterDTO, User>();

            CreateMap<UserLoginDTO, User>();
            CreateMap<User, UserLoginDTO>();

            CreateMap<Ticket, TicketDTO>();
            CreateMap<TicketDTO, Ticket>();

            CreateMap<PenaltyDTO, Penalty>();
            CreateMap<Penalty, PenaltyDTO>();

            CreateMap<ActiveTicketDTO, ActiveTickets>();
            CreateMap<ActiveTickets, ActiveTicketDTO>();

            CreateMap<TicketIdDTO, Ticket>();
            CreateMap<Ticket, TicketIdDTO>();

            CreateMap<AdminDTO, Admin>();
            CreateMap<Admin, AdminDTO>();
        }
    }
}
