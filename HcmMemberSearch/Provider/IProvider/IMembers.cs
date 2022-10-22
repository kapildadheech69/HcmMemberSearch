using HcmMemberSearch.Modals;

namespace HcmMemberSearch.Provider.IProvider
{
    public interface IMembers
    {
        Task<List<Member>> GetMembersAsync(); 
    }
}
