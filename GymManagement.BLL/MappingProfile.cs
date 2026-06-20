using AutoMapper;
using GymManagement.DAL.Models;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(D => D.Address, O => O.MapFrom(S => new Address()
            {
                BuildingNumber = S.BuildingNumber,
                Street = S.Street,
                City = S.City
            }))
            .ForMember(D => D.HealthRecord, O => O.MapFrom(S => new HealthRecord()
            {
                Height = S.HealthRecordViewModel.Height,
                Weight = S.HealthRecordViewModel.Weight,
                BloodType = S.HealthRecordViewModel.BloodType,
                Note = S.HealthRecordViewModel.Note
            }));

        CreateMap<Member, MemberViewModel>()
            .ForMember(MV => MV.DateOfBirth, O => O.MapFrom(M => M.DateOfBirth.ToString()))
            .ForMember(MV => MV.Gender, O => O.MapFrom(M => M.Gender.ToString()))
            .ForMember(MV => MV.Address, O => O.MapFrom(M => M.Address.ToString()));

        CreateMap<HealthRecord, HealthRecordViewModel>();

        CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(D => D.BuildingNumber, O => O.MapFrom(S => S.Address.BuildingNumber))
            .ForMember(D => D.Street, O => O.MapFrom(S => S.Address.Street))
            .ForMember(D => D.City, O => O.MapFrom(S => S.Address.City));
    }
}
