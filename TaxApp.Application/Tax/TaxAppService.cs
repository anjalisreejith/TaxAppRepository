using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TaxApp.Application.Tax.Dto;

namespace TaxApp.Application.Tax
{
    /// <summary>
    /// Application service class
    /// </summary>
    public class TaxAppService : ITaxAppService
    {
        public readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public TaxAppService(ILogger<TaxAppService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Method to calculate tax
        /// </summary>
        /// <param name="taxModel">input model for tax calculation</param>
        /// <returns>file details</returns>
        public TaxOutputDto CalculateTax(TaxModel taxModel)
        {
            try
            {
                decimal total = decimal.Parse(taxModel.Total.Replace(",", ""));
                Dictionary<string, CostCenter> costCenterModel = _configuration.GetSection("CostCenters").Get<Dictionary<string, CostCenter>>();

                if (!costCenterModel.TryGetValue(taxModel.CostCentre, out var costCenter))
                {
                    taxModel.CostCentre = "Unknown";
                    costCenter = costCenterModel.GetValueOrDefault("Unknown");
                }

                decimal salesTaxRate = costCenter.SalesTaxRate;
                decimal salesTax = total * salesTaxRate / (1 + salesTaxRate);
                decimal totalExcludingTax = total - salesTax;

                TaxOutputDto taxOutputDto = new TaxOutputDto()
                {
                    Total = taxModel.Total,
                    CostCentre = taxModel.CostCentre,
                    PaymentMethod = taxModel.PaymentMethod,
                    SalesTax = salesTax,
                    TotalExcludingTax = totalExcludingTax,
                };

                return taxOutputDto;
            }
            catch (Exception ex)
            {
                this._logger.LogError("Calculate tax error", ex);
                return null;
            }
        }

        /// <summary>
        /// Method to extract relevant data from input
        /// </summary>
        /// <param name="inputText">input text</param>
        /// <returns>file details</returns>
        public TaxModel ExtractDataFromString(string inputText)
        {
            try
            {
                Regex tagPattern = new Regex(@"<\/?(\w+)>");
                MatchCollection matches = tagPattern.Matches(inputText);

                List<Match> openingTags = matches.Cast<Match>().Where(x => x.Value.StartsWith("<") && !x.Value.StartsWith("</")).ToList();
                List<Match> closingTags = matches.Cast<Match>().Where(x => x.Value.StartsWith("</")).ToList();

                List<string> openingTagNames = openingTags.Select(i => i.Groups[1].Value).ToList();
                List<string> closingTagNames = closingTags.Select(i => i.Groups[1].Value).ToList();

                if (openingTags.Count != closingTags.Count)
                {
                    return null;
                }

                if (openingTagNames.Except(closingTagNames, StringComparer.OrdinalIgnoreCase).Any())
                {
                    return null;
                }

                TaxModel taxModel = new TaxModel();
                taxModel.CostCentre = Regex.Match(inputText, @"<cost_centre>(.*?)</cost_centre>").Groups[1].Value;
                taxModel.Total = Regex.Match(inputText, @"<total>(.*?)</total>").Groups[1].Value;
                taxModel.PaymentMethod = Regex.Match(inputText, @"<payment_method>(.*?)</payment_method>").Groups[1].Value;

                return taxModel;
            }
            catch (Exception ex)
            {
                this._logger.LogError("Error while extracting data from input", ex);
                return null;
            }
        }
    }
}
