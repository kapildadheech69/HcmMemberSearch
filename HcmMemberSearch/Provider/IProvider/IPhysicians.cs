using HcmMemberSearch.Modals;

namespace HcmMemberSearch.Provider.IProvider
{
    public interface IPhysicians
    {
        Task<IEnumerable<Physician>> GetPhysicians();
    }
}
