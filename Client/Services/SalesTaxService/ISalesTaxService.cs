namespace LouiseTieDyeStore.Client.Services.SalesTaxService
{
    public interface ISalesTaxService
    {
        Task<List<TaxRate>> GetTaxRates(); 
        Task<List<TaxRate>> UpdateRates(List<TaxRate> taxRates);
    }
}
