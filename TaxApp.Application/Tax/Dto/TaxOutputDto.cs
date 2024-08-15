namespace TaxApp.Application.Tax.Dto
{
    /// <summary>
    /// Model to return tax details
    /// </summary>
    public class TaxOutputDto
    {
        /// <summary>
        /// Gets or sets total
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets cost centre
        /// </summary>
        public string CostCentre { get; set; }

        /// <summary>
        /// Gets or sets payment method
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets sales tax
        /// </summary>
        public decimal SalesTax { get; set; }

        /// <summary>
        /// Gets or sets total excluding tax
        /// </summary>
        public decimal TotalExcludingTax { get; set; }
    }
}
