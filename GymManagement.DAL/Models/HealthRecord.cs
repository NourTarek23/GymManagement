using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models;

public class HealthRecord : BaseEntity
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public BloodTypes BloodType { get; set; }
    public string? Note { get; set; }

    public Member Member { get; set; }
    public int MemberId { get; set; }
}
