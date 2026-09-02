using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Tenants.Commands.Update
{
    public class UpdateTenantResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
