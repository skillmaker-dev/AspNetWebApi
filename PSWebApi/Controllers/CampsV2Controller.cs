
using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using PSWebApi.Models;

namespace PSWebApi.Controllers
{
    [Route("api/camps")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CampsV2Controller : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public CampsV2Controller(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetCamps(bool includeTalks = false)
        {
            try
            {
                var results = await repository.GetAllCampsAsync(includeTalks);

                //return results.Select(camp => MapToModel(camp)).ToArray();
                //return results;
                var response = new
                {
                    Count = results.Count(),
                    Results = mapper.Map<CampModel[]>(results)
                };
                return Ok(response);
            }
            catch(Exception)
            {
              return  this.StatusCode(StatusCodes.Status500InternalServerError,"Database Error");
            }
             
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="includeTalks"></param>
        /// <returns></returns>
        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> GetCamps(string moniker,bool includeTalks = false)
        {
            try
            {
                var result = await repository.GetCampAsync(moniker,includeTalks);
                if (result == null)
                    return NotFound();

                 //return MapToModel(result);

               return mapper.Map<CampModel>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }

        

        [HttpGet("search")]

        public async Task<ActionResult<CampModel[]>> GetCampsByDate(DateTime date,bool includeTalks = false)
        {
            try
            {
                var result = await repository.GetAllCampsByEventDate(date, includeTalks);

                if (!result.Any())
                    return NotFound();


                return mapper.Map<CampModel[]>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampModel>> CreateCamp(CampModel model)
        {
            var exists = await repository.GetCampAsync(model.Moniker);

            if (exists != null)
                return BadRequest("Moniker Name already exists");
            

            try
            {
                var camp = mapper.Map<Camp>(model);
                repository.Add(camp);
                if(await repository.SaveChangesAsync())
                    return CreatedAtAction(nameof(GetCamps),new { moniker = model.Moniker}, mapper.Map<CampModel>(camp));
            }
            catch (Exception )
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }

            return BadRequest();
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> UpdateCamp( string moniker, CampModel model )
        {
            try
            {
                var oldCamp = await repository.GetCampAsync(moniker);
                if (oldCamp == null)
                    return NotFound($"Could not found camp of moniker: {moniker}");

                mapper.Map(model, oldCamp);

                if (await repository.SaveChangesAsync())
                    return mapper.Map<CampModel>(oldCamp);
            }
            catch(Exception )
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }

                return BadRequest();
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> DeleteCamp(string moniker)
        {
            try
            {
                var camp = await repository.GetCampAsync(moniker);

                if (camp == null)
                    return NotFound();

                 repository.Delete(camp);

                if(await repository.SaveChangesAsync())
                    return Ok();
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }

            return BadRequest();
        }
       
    }
}
