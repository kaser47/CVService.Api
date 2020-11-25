using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CVService.Api.BusinessLogicLayer.Abstracts;
using CVService.Api.WebLayer;
using CVService.Api.WebLayer.Abstracts;
using CVService.Api.WebLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVService.Api.Controllers
{
    [Route("api/v1/skills")]
    [ApiController]
    public class SkillController : Controller
    {
        private readonly ICrudBusinessLogic<DataLayer.Models.Skill> _skillCrudBusinessLogic;

        public SkillController(ICrudBusinessLogic<DataLayer.Models.Skill> skillCrudBusinessLogic)
        {
            Guard.Against.Null(skillCrudBusinessLogic, nameof(skillCrudBusinessLogic));
            _skillCrudBusinessLogic = skillCrudBusinessLogic;
        }

        /// <summary>
        /// Returns a list of all "Skill"s currently in the DB
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SkillViewModel>>> Get()
        {
            var skillDataModels = await _skillCrudBusinessLogic.ReadAllAsync();
            var skillViewModels = skillDataModels.Select(x => x.MapToWebViewModel());
            return Ok(skillViewModels);
        }

        /// <summary>
        /// Gets a "Skill" based on Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SkillViewModel>> Get(int id)
        {
            var skillDataModel = await _skillCrudBusinessLogic.GetByIdAsync(id);

            if (skillDataModel == null)
            {
                return NotFound("Skill does not exist");
            }

            return Ok(skillDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Updates a "Skill" based on Id
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SkillViewModel>> Put(int id, SkillViewModel skillViewModel)
        {
            if (id != skillViewModel.Id)
            {
                return BadRequest("Ids cannot be different");
            }

            bool doesExist = await _skillCrudBusinessLogic.DoesExistAsync(id);
            if (!doesExist)
            {
                return NotFound("Skill does not exist");
            }

            var skillDataModel = await _skillCrudBusinessLogic.UpdateAsync(skillViewModel.MapToDataViewModel());

            return Ok(skillDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Creates a new "Skill"
        /// </summary>
        /// <remarks>PLEASE DO NOT PASS AN ID AS PART OF THE MODEL - For POST requests, supplying an ID will result in an error</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SkillViewModel>> Post(SkillViewModel skillViewModel)
        {
            if (skillViewModel == null)
            {
                return BadRequest("Skill cannot be null");
            }

            var skillDataModel = await _skillCrudBusinessLogic.AddAsync(skillViewModel.MapToDataViewModel());

            return CreatedAtAction("Get", new { id = skillDataModel.Id }, skillDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Deletes a "Skill" based on Id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SkillViewModel>> Delete(int id)
        {
            bool doesExist = await _skillCrudBusinessLogic.DoesExistAsync(id);
            if (!doesExist)
            {
                return NotFound("Skill does not exist");
            }

            var cvToDelete = await _skillCrudBusinessLogic.GetByIdAsync(id);
            var skillDataModel = await _skillCrudBusinessLogic.RemoveAsync(cvToDelete);

            return Ok(skillDataModel.MapToWebViewModel());
        }
    }
}