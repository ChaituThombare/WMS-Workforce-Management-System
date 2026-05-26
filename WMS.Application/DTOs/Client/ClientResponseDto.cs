using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Application.DTOs.Client
{
    public class ClientResponseDto
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string? ClientAddress { get; set; }

        public string? ClientPhoneNumber { get; set; }

        public string? ClientLocation { get; set; }

        public bool Status { get; set; }
    }
}
