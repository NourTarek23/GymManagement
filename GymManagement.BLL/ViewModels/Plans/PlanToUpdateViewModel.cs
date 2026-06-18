using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.ViewModels.Plans;

public class PlanToUpdateViewModel
{
    public string? Name { get; set; }

    [Required(ErrorMessage = "Duration Is Required")]
    [Range(1, 365, ErrorMessage = "Duration Days must be Between 1 and 365")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Price Is Required")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Description Is Required")]
    public string Description { get; set; } = default!;
}
