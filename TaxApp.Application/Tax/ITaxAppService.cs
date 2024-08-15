using TaxApp.Application.Tax.Dto;

namespace TaxApp.Application.Tax
{
    public interface ITaxAppService
    {
        TaxOutputDto CalculateTax(TaxModel taxModel);

        TaxModel ExtractDataFromString(string inputText);
    }
}
