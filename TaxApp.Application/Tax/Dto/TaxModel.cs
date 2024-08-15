namespace TaxApp.Application.Tax.Dto
{
    /// <summary>
    /// Model to extract tax details
    /// </summary>
    public class TaxModel
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
    }
}
