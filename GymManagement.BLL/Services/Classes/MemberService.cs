using AutoMapper;
using GymManagement.BLL.Services.Attachment;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Classes;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public MemberService(IUnitOfWork unitOfWork,
                         IMapper mapper,
                         IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
    {
        // Mapping from Member to MemberViewModel
        var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

        //var membersViewModel = members.Select(M => new MemberViewModel
        //{
        //    Id = M.Id,
        //    Name = M.Name,
        //    Photo = M.Photo,
        //    Email = M.Email,
        //    Phone = M.Phone,
        //    Gender = M.Gender.ToString()
        //});


        var membersViewModel = _mapper.Map<IEnumerable<MemberViewModel>>(members);

        return membersViewModel;
    }

    public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
    {
        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

        if (member is null) return null;

        //Mapping Member to MemberViewModel
        //var model = new MemberViewModel()
        //{
        //    Photo = member.Photo,
        //    Name = member.Name,
        //    Email = member.Email,
        //    Phone = member.Phone,
        //    Gender = member.Gender.ToString(),
        //    DateOfBirth = member.DateOfBirth.ToString(),
        //    Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
        //};

        //Mapping
        var model = _mapper.Map<MemberViewModel>(member);

        var activeMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(M => M.MemberId == memberId && M.EndDate > DateTime.UtcNow, ct);

        if (activeMembership is not null)
        {
            model.PlanName = activeMembership.Plan.Name;
            model.MembershipStartDate = activeMembership.CreatedAt.ToString();
            model.MembershipEndDate = activeMembership.EndDate.ToString();
        }

        return model;
    }

    public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
    {
        var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(H => H.MemberId == memberId, ct);
        if (healthRecord is null) return null;


        //mapping healthRecord to HealthRecordViewModel
        //var model = new HealthRecordViewModel()
        //{
        //    Height = healthRecord.Height,
        //    Weight = healthRecord.Weight,
        //    BloodType = healthRecord.BloodType,
        //    Note = healthRecord.Note
        //};

        var model = _mapper.Map<HealthRecordViewModel>(healthRecord);


        return model;
    }
    
    //You Need To Check This Mapping *********************************************************************
    public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
    {
        //Check Email
        var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email, ct);
        //Check Phone
        var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone, ct);

        if (emailExists || phoneExists) return false;

        //ToDo : Upload File 
        var fileName = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), "MembersPicture", model.PhotoFile.FileName, ct);
        if (string.IsNullOrWhiteSpace(fileName)) return false;

        //Auto Mapping
        var member = _mapper.Map<Member>(model);
        member.Photo = fileName;

        _unitOfWork.GetRepository<Member>().Add(member);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        if(count > 0)
        {
            return true;
        }
        else
        {
            //Delete Photo 
            _attachmentService.Delete("MembersPicture", fileName);
            return false;
        }
        
    }

    public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
    {
        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
        if (member is null) return null;

        //var model = new MemberToUpdateViewModel()
        //{
        //    Name = member.Name,
        //    Photo = member.Photo,
        //    Phone = member.Phone,
        //    Email = member.Email,
        //    BuildingNumber = member.Address.BuildingNumber,
        //    Street = member.Address.Street,
        //    City = member.Address.City
        //};

        var model = _mapper.Map<MemberToUpdateViewModel>(member);

        return model;
    }

    public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
    {
        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
        if (member is null) return false;

        //Check Email
        var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email && M.Id != memberId, ct);
        //Check Phone
        var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone && M.Id != memberId, ct);

        if (emailExists || phoneExists) return false;

        member.Email = model.Email;
        member.Phone = model.Phone;
        member.Address.BuildingNumber = model.BuildingNumber;
        member.Address.City = model.City;
        member.Address.Street = model.Street;

        _unitOfWork.GetRepository<Member>().Update(member);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0;
    }

    public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
    {
        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
        if (member is null) return false;

        var hasFutureSessions = await _unitOfWork.GetRepository<Booking>().AnyAsync(B => B.MemberId == memberId && B.Session.StartDate > DateTime.UtcNow, ct);
        if (hasFutureSessions) return false;

        if (member.Photo is not null) _attachmentService.Delete("MembersPicture", member.Photo);

        _unitOfWork.GetRepository<Member>().Delete(member);

        var count = await _unitOfWork.SaveChangesAsync(ct);

        return count > 0;
    }

}