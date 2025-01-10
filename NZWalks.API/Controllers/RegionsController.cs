using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [EnableRateLimiting("FixedPolicy")]
        public async Task<IActionResult> GetAllRegions()
        {
            // get data from DB - domain models
            var regionsDomain = await regionRepository.GetAllAsyncRegions();

            // return DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // GET Single Region
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetRegionById(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // return dto
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // POST
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map / convert AddRegionRequestDto to Region domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // User domain model to create region
            regionDomainModel = await regionRepository.CreateRegion(regionDomainModel);

            // Map region domain model to region DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);


        }

        // Update region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            regionDomainModel = await regionRepository.UpdateRegion(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // convert domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);


        }

        // DELETE a region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteRegion(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted region
            // map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }


    }
}
