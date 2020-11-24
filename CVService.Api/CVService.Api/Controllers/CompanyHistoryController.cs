using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CVService.Api.WebLayer;
using CVService.Api.WebLayer.Abstracts;
using CVService.Api.WebLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CVService.Api.Controllers
{
    [Route("api/v1/company-histories")]
    [ApiController]
    public class CompanyHistoryController : Controller
    {
        private readonly ICrudBusinessLogic<DataLayer.Models.CompanyHistory> _companyHistoryCrudBusinessLogic;

        public CompanyHistoryController(ICrudBusinessLogic<DataLayer.Models.CompanyHistory> companyHistoryCrudBusinessLogic)
        {
            Guard.Against.Null(companyHistoryCrudBusinessLogic, nameof(companyHistoryCrudBusinessLogic));
            _companyHistoryCrudBusinessLogic = companyHistoryCrudBusinessLogic;
        }

        /// <summary>
        /// Returns a list of all "Company Histories" currently in the DB
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompanyHistoryViewModel>>> Get()
        {
            var companyHistoryDataModels = await _companyHistoryCrudBusinessLogic.ReadAllAsync();
            var companyHistoryViewModels = companyHistoryDataModels.Select(x => x.MapToWebViewModel());
            return Ok(companyHistoryViewModels);
        }

        /// <summary>
        /// Gets a "Company History" based on Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyHistoryViewModel>> Get(int id)
        {
            var companyHistoryDataModel = await _companyHistoryCrudBusinessLogic.GetByIdAsync(id);

            if (companyHistoryDataModel == null)
            {
                return NotFound("Company History does not exist");
            }

            return Ok(companyHistoryDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Updates a "Company History" based on Id
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyHistoryViewModel>> Put(int id, CompanyHistoryViewModel companyHistoryViewModel)
        {
            if (id != companyHistoryViewModel.Id)
            {
                return BadRequest("Ids cannot be different");
            }

            bool doesExist = await _companyHistoryCrudBusinessLogic.DoesExistAsync(id);
            if (!doesExist)
            {
                return NotFound("Company History does not exist");
            }

            var companyHistoryDataModel = await _companyHistoryCrudBusinessLogic.UpdateAsync(companyHistoryViewModel.MapToDataViewModel());

            return Ok(companyHistoryDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Creates a new "Company History"
        /// </summary>
        /// <remarks>PLEASE DO NOT PASS AN ID AS PART OF THE MODEL - For POST requests, supplying an ID will result in an error</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CompanyHistoryViewModel>> Post(CompanyHistoryViewModel companyHistoryViewModel)
        {
            if (companyHistoryViewModel == null)
            {
                return BadRequest("Company history cannot be null");
            }

            var companyHistoryDataModel = await _companyHistoryCrudBusinessLogic.AddAsync(companyHistoryViewModel.MapToDataViewModel());

            return CreatedAtAction("Get", new { id = companyHistoryDataModel.Id }, companyHistoryDataModel.MapToWebViewModel());
        }

        /// <summary>
        /// Deletes a "Company History" based on Id
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyHistoryViewModel>> Delete(int id)
        {
            //TODO: Tech Test - See readme notes for explanation
            //if (!_securityModule.DoesUserHaveApiWriteAccess(request.header["token"]))
            //{ throw Forbidden }

            bool doesExist = await _companyHistoryCrudBusinessLogic.DoesExistAsync(id);
            if (!doesExist)
            {
                return NotFound("Company History does not exist");
            }

            var cvToDelete = await _companyHistoryCrudBusinessLogic.GetByIdAsync(id);
            var companyHistoryDataModel = await _companyHistoryCrudBusinessLogic.RemoveAsync(cvToDelete);

            return Ok(companyHistoryDataModel.MapToWebViewModel());
        }
    }
}