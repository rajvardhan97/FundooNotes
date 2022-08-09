using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<UserController> logger;
        public CollabController(ICollabBL collabBL, FundooContext fundooContext, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<UserController> logger)
        {
            this.collabBL = collabBL;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult AddCollab(CollabModel collabModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var collab = fundooContext.NotesTable.Where(X => X.NoteId == collabModel.NoteId).FirstOrDefault();
                if (collab.UserId == userId)
                {
                    var result = collabBL.AddCollab(collabModel);
                    if (result != null)
                    {
                        logger.LogInformation("Collaboration successfully");
                        return Ok(new { Success = true, message = "Collaboration successful", data = result });
                    }
                    else
                    {
                        logger.LogError("Collaboration Failed");
                        return BadRequest(new { Sucess = false, message = "Collaboration Failed" });
                    }
                }
                else
                {
                    logger.LogError("Collaboration Failed");
                    return Unauthorized(new { Sucess = false, message = "Collaboration Failed" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemoveCollab(long collabID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var delete = collabBL.RemoveCollab(collabID, userId);
                if (delete != null)
                {
                    logger.LogInformation("Collaboration Removed Successfully");
                    return Ok(new { Success = true, message = "Collaboration Removed"});
                }
                else
                {
                    logger.LogError("Unsuccessful to Remove Collaboration");
                    return BadRequest(new { Success = false, message = "Unsuccessful" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult GetAllCollabs(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var notes = collabBL.GetCollab(noteId, userId);
                if (notes != null)
                {
                    logger.LogInformation("Collaboration Found Successfully");
                    return Ok(new { Success = true, message = "Collaboration Successful", data = notes });

                }
                else
                {
                    logger.LogError("No Collaboration Found");
                    return BadRequest(new { Success = false, message = "No Collaboration Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllCollaboratorUsingRedisCache()
        {
            var cacheKey = "CollabsList";
            string serializedList;
            var CollabsList = new List<CollabEntity>();
            var redisCollabsList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabsList != null)
            {
                serializedList = Encoding.UTF8.GetString(redisCollabsList);
                CollabsList = JsonConvert.DeserializeObject<List<CollabEntity>>(serializedList);
            }
            else
            {
                CollabsList = await fundooContext.CollabTable.ToListAsync();
                serializedList = JsonConvert.SerializeObject(CollabsList);
                redisCollabsList = Encoding.UTF8.GetBytes(serializedList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabsList, options);
            }
            return Ok(CollabsList);
        }
    }
}
