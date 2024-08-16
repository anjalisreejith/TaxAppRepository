using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TaxApp.Application;
using TaxApp.Application.Tax;
using TaxApp.Application.Tax.Dto;

namespace TaxApp.Web.Host.Controllers
{
    /// <summary>
    /// Controller class for application settings related mehtods.
    /// </summary>
    [Route("tax")]
    [ApiController]
    public class TaxController : Controller
    {
        private readonly ITaxAppService _taxAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxAppController"/> class.
        /// </summary>
        /// <param name="taxAppService">Instance of tax application service.</param>
        public TaxController(
            ITaxAppService taxAppService
            )
            : base()
        {
            this._taxAppService = taxAppService;
        }

        [HttpGet]
        [Route("calculatetax")]
        public IActionResult CalculateTax(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return BadRequest(new { message = "Invalid input" });
            }

            TaxModel taxModel = _taxAppService.ExtractDataFromString(inputText);

            if (taxModel == null)
            {
                return BadRequest(new { message = "Input string is not in the valid format" });
            }

            if (string.IsNullOrEmpty(taxModel.Total))
            {
                var errorResponse = new ErrorModel
                {
                    StatusCode = 422,
                    Message = "The request could not be processed.",
                    Detailed = "The total amount field is required and cannot be empty."
                };

                return StatusCode(422, errorResponse);
            }

            if (!decimal.TryParse(taxModel.Total, out decimal total))
            {
                var errorResponse = new ErrorModel
                {
                    StatusCode = 422,
                    Message = "The request could not be processed.",
                    Detailed = "The total amount field is required and is not in valid format."
                };

                return StatusCode(422, errorResponse);
            }

            return Ok(_taxAppService.CalculateTax(taxModel));
        }
    }
}
