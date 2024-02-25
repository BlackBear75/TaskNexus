using TaskNexus.Models.ApplicationUser;


namespace TaskNexus.DAL.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<bool> Create(ApplicationUser entity);

        Task<ApplicationUser> Get(string id);

        Task<List<ApplicationUser>> Select();

        Task<bool> Delete(ApplicationUser entity);

        Task<ApplicationUser> Update(ApplicationUser entity);
    }
}
