using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.User.Dto
{
    public class UserReadDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool? IsActive { get; set; }
    }
}
