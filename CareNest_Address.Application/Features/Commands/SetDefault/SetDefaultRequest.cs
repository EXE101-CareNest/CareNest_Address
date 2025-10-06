using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareNest_Address.Application.Features.Commands.SetDefault
{
    public class SetDefaultRequest
    {
        public bool IsDefault { get; set; }
        public string? AccountId { get; set; }
    }
}
