namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideShowingInfo : IProvideCommonShowingInfo
    {
        string OccupantPhone { get; set; }
        string ContactPhone { get; set; }
        LockBoxType? LockBoxType { get; set; }
        ICollection<ShowingRequirements> ShowingRequirements { get; set; }
        public ICollection<string> RealtorContactEmail { get; set; }
    }
}
