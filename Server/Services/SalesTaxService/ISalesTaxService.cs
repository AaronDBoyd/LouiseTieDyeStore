using LouiseTieDyeStore.Client.Pages.Admin;

namespace LouiseTieDyeStore.Server.Services.SalesTaxService
{
    public interface ISalesTaxService
    {
        Task<ServiceResponse<List<TaxRate>>> GetRates();
        Task<ServiceResponse<List<TaxRate>>> UpdateRates(List<TaxRate> newTaxRates);
        Task<ServiceResponse<decimal>> GetTaxRate(string state);
    }
}
