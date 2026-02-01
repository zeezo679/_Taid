using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models.Entities;
using Web.ViewModel;

namespace Core.Models.Interfaces
{
    public interface IMapper
    {
        ApplicationUser MapToUser(RegisterViewModel newUserVM);
    }
}
