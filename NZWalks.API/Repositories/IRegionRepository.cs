using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsyncRegions();

        Task<Region?> GetRegionById(Guid id);

        Task<Region> CreateRegion(Region region);

        Task<Region?> UpdateRegion(Guid id, Region region);

        Task<Region?> DeleteRegion(Guid id);
    }
}
