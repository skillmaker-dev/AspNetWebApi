using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSWebApi.Models;

namespace PSWebApi.Controllers
{
    [ApiController]
    [Route("api/camps/{moniker}/[Controller]")]
    public class TalksController : ControllerBase
    {
        
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public TalksController(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<TalkModel []>> GetTalks(string moniker,bool includeSpeakers = true)
        {
            try
            {
                var talks = await repository.GetTalksByMonikerAsync(moniker,includeSpeakers);


                return Ok(mapper.Map<TalkModel []>(talks));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
            }

            
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<TalkModel>> GetTalkById(string moniker,int id,bool includeSpeakers = true)
        {
            try
            {
                var talk = await repository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);

                if(talk == null)
                    return NotFound();

                return Ok(mapper.Map<TalkModel>(talk));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
            }

        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> AddTalkToCamp(TalkModel model,string moniker)
        {
            try
            {
                var camp = await repository.GetCampAsync(moniker);

                if (camp == null) return BadRequest("camp not found");

                var newTalk = mapper.Map<Talk>(model);
                newTalk.Camp = camp;
                //if (model.Speaker == null) return BadRequest("SpeakerId not specified");

                //var speaker = await repository.GetSpeakerAsync(model.Speaker.SpeakerId);

                //if (speaker == null) return BadRequest("speaker not found");

                //newTalk.Speaker = speaker;

                repository.Add(newTalk);

                if (!await repository.SaveChangesAsync())
                    return BadRequest("could not add new talk please try again");

                return CreatedAtAction(nameof(GetTalkById),new {moniker , id = newTalk.TalkId}, mapper.Map<TalkModel>(newTalk));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
            }
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult<TalkModel>> UpdateTalkInCamp(TalkModel model, string moniker,int id)
        {
            try
            {
                var talk = await repository.GetTalkByMonikerAsync(moniker,id,true);

                if (talk == null) return BadRequest("talk not found");

                mapper.Map(model, talk);

                if(model.Speaker != null)
                {
                    talk.Speaker = model.Speaker; 
                }

                if (!await repository.SaveChangesAsync())
                    return BadRequest("could not add new talk please try again");

                return mapper.Map<TalkModel>(talk);

            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTalk(int id,string moniker)
        {
            try
            {
                var talk = await repository.GetTalkByMonikerAsync(moniker,id);
                if (talk == null) return NotFound();

                repository.Delete(talk);

                if(await repository.SaveChangesAsync())
                    return Ok();

                return BadRequest("Could not delete talk, please try again");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
            }
        }

    }
}
