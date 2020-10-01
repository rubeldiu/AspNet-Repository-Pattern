using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dto;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;

        }  
        [HttpGet]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            foreach (var item in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(item));
            }

            return Ok(objDto);
        }
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);


        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailCreateDto)
        {
            if (trailCreateDto==null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailCreateDto.Name))
            {
                ModelState.AddModelError("", "Trail Already Exists !!!");
                return StatusCode(404, ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailCreateDto);
            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", "Something went to wrong");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]

        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailUpdateDto)
        {
            if (trailUpdateDto == null)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailUpdateDto);
            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", "Something goes wrong !!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{trailId:int}",Name ="DeleteTrail")]

        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", "Something goes wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
