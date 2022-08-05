using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public LabelController(ILabelBL labelBL, FundooContext fundooContext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.labelBL = labelBL;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddLabel(LabelModel labelModel)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var labelNote = fundooContext.NotesTable.Where(x => x.NoteId == labelModel.NoteId).FirstOrDefault();
                if (labelNote.UserId == userid)
                {
                    var result = labelBL.AddLabel(labelModel);
                    if (result != null)
                    {
                        return Ok(new { Success = true, Message = "Label created successfully", data = result });
                    }
                    else
                    {
                        return BadRequest(new { Success = false, Message = "Label is not created" });
                    }
                }
                return this.Unauthorized(new { Success = false, Message = "Unauthorized Access" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllLabel(long userId)
        {
            try
            {
                var label = labelBL.GetAllLabel(userId);
                if (label != null)
                {
                    return Ok(new { Success = true, Message = " Displaying Label Successfully", data = label });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = "No label found" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Getlabel(long NoteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var label = labelBL.Getlabel(NoteId, userId);
                if (label != null)
                {
                    return Ok(new { Success = true, message = "Label found Successfully", data = label });
                }
                else
                    return BadRequest(new { Success = false, message = "Label not Found" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var result = labelBL.UpdateLabel(labelModel, labelID);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Label Updated Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Label Not Updated" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteLabel(long labelID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var delete = labelBL.DeleteLabel(labelID, userId);
                if (delete != null)
                {
                    return Ok(new { Success = true, message = "Label Deleted Successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Label not Deleted" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "LabelsList";
            string serializedLabelsList;
            var labelsList = new List<LabelEntity>();
            var redisLabelsList = await this.distributedCache.GetAsync(cacheKey);
            if (redisLabelsList != null)
            {
                serializedLabelsList = Encoding.UTF8.GetString(redisLabelsList);
                labelsList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelsList);
            }
            else
            {
                labelsList = await this.fundooContext.LabelTable.ToListAsync();
                serializedLabelsList = JsonConvert.SerializeObject(labelsList);
                redisLabelsList = Encoding.UTF8.GetBytes(serializedLabelsList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await this.distributedCache.SetAsync(cacheKey, redisLabelsList, options);
            }

            return this.Ok(labelsList);
        }
    }
}
