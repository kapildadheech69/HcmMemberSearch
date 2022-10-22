using HcmMemberSearch.Modals;

namespace HcmMemberSearch.Provider.IProvider
{
    public interface IClaims
    {
        Task<List<Claim>> GetClaims();
    }
}
