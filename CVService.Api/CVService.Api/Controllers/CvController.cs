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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CvController : Controller
    {
        private readonly ICvBusinessLogic _cvCrudBusinessLogic;

        public CvController(ICvBusinessLogic cvCrudBusinessLogic)
        {
            Guard.Against.Null(cvCrudBusinessLogic, nameof(cvCrudBusinessLogic));
            _cvCrudBusinessLogic = cvCrudBusinessLogic;
        }

        /// <summary>
        /// Returns a list of all "CV"s currently in the DB
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CvViewModel>>> Get()
        { 
            //TODO: Tech Test - If there was more time I would have added paging to these get methods, I understand that most APIs have methods that take numberperpage and pagenumer to get paged results.
           var cvDataModels = await _cvCrudBusinessLogic.ReadAllAsync();
           var cvViewModels = cvDataModels.Select(c => c.MapToWebViewModel());

           return Ok(cvViewModels);
        }

        /// <summary>
        /// Gets a "CV" based on Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CvViewModel>> Get(int id)
        {
            var cvDataModel = await _cvCrudBusinessLogic.GetByIdAsync(id);

            if (cvDataModel == null)
            {
                return NotFound("CV does not exist");
            }

            return Ok(cvDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Updates a "CV" based on Id
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CvViewModel>> Put(int id, CvViewModel cvViewModel)
        {
            if (id != cvViewModel.Id)
            {
                return BadRequest("Ids cannot be different");
            }

            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(id);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            var cvDataModel =  await _cvCrudBusinessLogic.UpdateAsync(cvViewModel.MapToDataViewModel());

            return Ok(cvDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Creates a new "CV"
        /// </summary>
        /// <remarks>PLEASE DO NOT PASS AN ID AS PART OF THE MODEL - For POST requests, supplying an ID will result in an error</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CvViewModel>> Post(CvViewModel cvViewModel)
        {
            if (cvViewModel == null)
            {
                return BadRequest("Cv cannot be null");
            }

            var cvDataModel =  await _cvCrudBusinessLogic.AddAsync(cvViewModel.MapToDataViewModel());

            return CreatedAtAction("Get", new { id = cvDataModel.Id }, cvDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Deletes a "CV" based on Id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CvViewModel>> Delete(int id)
        {
            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(id);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            var cvToDelete = await _cvCrudBusinessLogic.GetByIdAsync(id);
            var cvDataModel =  await _cvCrudBusinessLogic.RemoveAsync(cvToDelete);

            return Ok(cvDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Returns a list of all "Skill"s currently in the DB for a specific CV
        /// </summary>
        [HttpGet("{cvId}/skills")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SkillViewModel>>> GetSkills(int cvId)
        {
            var skillDataModels = await _cvCrudBusinessLogic.GetSkills(cvId);

            if (!skillDataModels.Any())
            {
                return NotFound("Skills do not exist");
            }

            var skillViewModels = skillDataModels.Select(s => s.MapToWebViewModel());
            return Ok(skillViewModels);
        }

        /// <summary>
        /// Returns a list of all "Company Histories"s currently in the DB for a specific CV
        /// </summary>
        [HttpGet("{cvId}/company-histories")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompanyHistoryViewModel>>> GetCompanyHistories(int cvId)
        {
            var companyHistoryDataModels = await _cvCrudBusinessLogic.GetCompanyHistories(cvId);

            if (!companyHistoryDataModels.Any())
            {
                return NotFound("Company History does not exist");
            }

            var companyHistoryViewModels = companyHistoryDataModels.Select(x => x.MapToWebViewModel());
            return Ok(companyHistoryViewModels);
        }

        /// <summary>
        /// Returns a "Company History" currently in the DB for a specific CV
        /// </summary>
        [HttpGet("{cvId}/company-histories/{companyHistoryId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyHistoryViewModel>> GetCompanyHistory(int cvId, int companyHistoryId)
        {
            var companyHistoryDataModel = await _cvCrudBusinessLogic.GetCompanyHistory(cvId, companyHistoryId);

            if (companyHistoryDataModel == null)
            {
                return NotFound("Company History does not exist");
            }

            return Ok(companyHistoryDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Returns a "Skill" currently in the DB for a specific CV
        /// </summary>
        [HttpGet("{cvId}/skills/{skillId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SkillViewModel>> GetSkill(int cvId, int skillId)
        {
            var skillDataModel = await _cvCrudBusinessLogic.GetSkill(cvId, skillId);

            if (skillDataModel == null)
            {
                return NotFound("Skill does not exist");
            }

            return Ok(skillDataModel.MapToWebViewModel());
        }


        /// <summary>
        /// Creates a new "Skill" for a "CV"
        /// </summary>
        /// <remarks>PLEASE DO NOT PASS AN ID AS PART OF THE MODEL - For POST requests, supplying an ID will result in an error</remarks>
        [HttpPost("{cvId}/skills")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SkillViewModel>> Post(int cvId, SkillViewModel skillViewModel)
        {
            if (skillViewModel == null)
            {
                return BadRequest("Skill cannot be null");
            }

            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(cvId);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            var skillDataModel = await _cvCrudBusinessLogic.AddSkill(cvId, skillViewModel.MapToDataViewModel());

            return CreatedAtAction("GetSkill", new { cvId = cvId , skillId = skillDataModel.Id }, skillDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Creates a new "Company History" for a "CV"
        /// </summary>
        /// <remarks>PLEASE DO NOT PASS AN ID AS PART OF THE MODEL - For POST requests, supplying an ID will result in an error</remarks>
        [HttpPost("{cvId}/company-histories")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CompanyHistoryViewModel>> Post(int cvId, CompanyHistoryViewModel companyHistoryViewModel)
        {
            if (companyHistoryViewModel == null)
            {
                return BadRequest("Company History cannot be null");
            }

            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(cvId);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            var companyHistoryDataModel = await _cvCrudBusinessLogic.AddCompanyHistory(cvId, companyHistoryViewModel.MapToDataViewModel());

            return CreatedAtAction("GetCompanyHistory", new {cvId = cvId ,companyHistoryId = companyHistoryDataModel.Id }, companyHistoryDataModel.MapToWebViewModel());
        }


        /// <summary>
        /// Updates a "Skill" for a "CV"
        /// </summary>
        [HttpPut("{cvId}/skills/{skillId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SkillViewModel>> Put(int cvId, int skillId, SkillViewModel skillViewModel)
        {
            if (skillId != skillViewModel.Id)
            {
                return BadRequest("Ids cannot be different");
            }

            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(cvId);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            bool doesSkillExist =
                await _cvCrudBusinessLogic.DoesEntityExistInCvAsync<DataLayer.Models.Skill>(cvId,skillId);
            if (!doesSkillExist)
            {
                return NotFound("Skill does not exist in CV");
            }

            var skillDataModel = await _cvCrudBusinessLogic.EditSkill(cvId,skillViewModel.MapToDataViewModel());

            return Ok(skillDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Updates a "Company History" for a "CV"
        /// </summary>
        [HttpPut("{cvId}/company-histories/{companyHistoryId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyHistoryViewModel>> Put(int cvId, int companyHistoryId, CompanyHistoryViewModel companyHistoryViewModel)
        {
            if (companyHistoryId != companyHistoryViewModel.Id)
            {
                return BadRequest("Ids cannot be different");
            }

            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(cvId);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            bool doesCompanyHistoryExist =
                await _cvCrudBusinessLogic.DoesEntityExistInCvAsync<DataLayer.Models.CompanyHistory>(cvId,companyHistoryId);
            if (!doesCompanyHistoryExist)
            {
                return NotFound("Company History does not exist in CV");
            }

            var companyHistoryDataModel = await _cvCrudBusinessLogic.EditCompanyHistory(cvId, companyHistoryViewModel.MapToDataViewModel());

            return Ok(companyHistoryDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Deletes a "Skill" for a "CV"
        /// </summary>
        [HttpDelete("{cvId}/skills/{skillId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SkillViewModel>> DeleteSkill(int cvId, int skillId)
        {
            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(cvId);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            bool doesCompanyHistoryExist =
                await _cvCrudBusinessLogic.DoesEntityExistInCvAsync<DataLayer.Models.Skill>(cvId,skillId);
            if (!doesCompanyHistoryExist)
            {
                return NotFound("Skill does not exist in CV");
            }

            var skillDataModel = await _cvCrudBusinessLogic.RemoveSkill(cvId, skillId);

            return Ok(skillDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Deletes a "Company History" for a "CV"
        /// </summary>
        [HttpDelete("{cvId}/company-histories/{companyHistoryId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyHistoryViewModel>> DeleteCompanyHistory(int cvId, int companyHistoryId)
        {
            bool doesExist = await _cvCrudBusinessLogic.DoesExistAsync(cvId);
            if (!doesExist)
            {
                return NotFound("CV does not exist");
            }

            bool doesCompanyHistoryExist =
                await _cvCrudBusinessLogic.DoesEntityExistInCvAsync<DataLayer.Models.CompanyHistory>(cvId,companyHistoryId);
            if (!doesCompanyHistoryExist)
            {
                return NotFound("Company History does not exist in CV");
            }

            var companyHistoryDataModel = await _cvCrudBusinessLogic.RemoveCompanyHistory(cvId, companyHistoryId);
            return Ok(companyHistoryDataModel.MapToWebViewModel());
        }



    }
}