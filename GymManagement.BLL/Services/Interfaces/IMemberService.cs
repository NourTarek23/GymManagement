using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces;

public interface IMemberService
{
    //get all members 
     Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct);

    //get member by id
    Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct);

    //get member's health record
    Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);

    //add new member
    Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct);

    //get member to update
    Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct);

    //update member
    Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct);

    //delete member
    Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct);


}
