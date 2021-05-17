namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Models;

    public interface ILoadControlProcessor
    {
        Task<LoadControl> CreateLoadControlAsync(DateTime value, CancellationToken cancellationToken);
    }
}
