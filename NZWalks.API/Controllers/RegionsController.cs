using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllRegions()
        {
            // get data from DB - domain models

            var regionsDomain = dbContext.Regions.ToList();

            // Map domain models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            // return DTOs
            return Ok(regionsDto);
        }

        // GET Single Region
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(r => r.Id == id);
            

            if (regionDomain == null)
            {
                return NotFound();
            }

            // map region domain to region DTO
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // return dto
            return Ok(regionDto);
        }

        // POST
        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map / convert AddRegionRequestDto to Region domain model
            var regionDomainModel = new Region
            {
               Code = addRegionRequestDto.Code,
               Name = addRegionRequestDto.Name,
               RegionImageUrl = addRegionRequestDto.RegionImageUrl 
            };

            // User domain model to create region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // Map region domain model to region DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel =  dbContext.Regions.FirstOrDefault(r => r.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map dto to doamin
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            // convert domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // DELETE a region
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(r => r.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // delete region
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            // return deleted region
            // map domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        
    }
}
