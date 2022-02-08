namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ApplicationState
    {
        public DateTime? RunIdentifier { get; set; }

        public Dictionary<string, int> UriRequests { get; set; }

        public string ForceResponseStatusCode { get; set; }

        public int ForceResponseStatusCodeCount { get; set; }
    }
}
