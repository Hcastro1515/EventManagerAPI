using EventManagerAPI.Data;
using EventManagerAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public EventsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetCurrentEvents()
        {
            return Ok(await _dataContext.Events.ToListAsync()); 
        }

        [HttpPost]
        public async Task<ActionResult<List<Event>>> CreateEvent(Event eventRequest)
        {
            _dataContext.Events.Add(eventRequest);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Events.ToListAsync()); 
        }

        [HttpPut]
        public async Task<ActionResult<List<Event>>> UpdateEvent(Event eventRequest)
        {
            var eventDb = await _dataContext.Events.FindAsync(eventRequest.Id);
            if (eventDb == null)
                return BadRequest("Event not found");

            eventDb.Title = eventRequest.Title;
            eventDb.Description = eventRequest.Description;
            eventDb.Address = eventRequest.Address;
            eventDb.ImageUrl = eventRequest.ImageUrl;
            eventDb.StartTime = eventRequest.StartTime;
            eventDb.EndTime = eventRequest.EndTime; 
            eventDb.UpdateDate = eventRequest.UpdateDate;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Events.ToListAsync());
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult<List<Event>>> DeleteEvent(int id)
        {
            var eventDb = await _dataContext.Events.FindAsync(id);
            if (eventDb == null)
                return BadRequest("Event not found");

            _dataContext.Events.Remove(eventDb);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Events.ToListAsync());
        }
        
    }
}
