using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes;

public class PlanRepository : IPlanRepository
{
    private readonly GymDbContext _context;

    public PlanRepository(GymDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        => tracking? await _context.Plans.ToListAsync(ct) : await _context.Plans.AsNoTracking().ToListAsync();

    public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Plans.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
    {
        await _context.Plans.AddAsync(plan, ct);
        return await _context.SaveChangesAsync(ct);
    }
         

    public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
    {
        _context.Plans.Update(plan);
        return await _context.SaveChangesAsync(ct);
    }
    
    public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
    {
        _context.Plans.Remove(plan);
        return await _context.SaveChangesAsync(ct);
    }

}
