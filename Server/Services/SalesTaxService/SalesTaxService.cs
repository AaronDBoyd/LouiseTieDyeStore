using LouiseTieDyeStore.Client.Pages.Admin;

namespace LouiseTieDyeStore.Server.Services.SalesTaxService
{
    public class SalesTaxService : ISalesTaxService
    {
        private readonly DataContext _context;

        public SalesTaxService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<TaxRate>>> GetRates()
        {
            var rates = await _context.TaxRates.ToListAsync();

            return new ServiceResponse<List<TaxRate>>
            {
                Data = rates
            };
        }

        public async Task<ServiceResponse<decimal>> GetTaxRate(string state)
        {
            var dbRate = await _context.TaxRates.FirstOrDefaultAsync(x => x.Abbreviation == state);
          
            return new ServiceResponse<decimal>
            {
                Data = dbRate.Rate
            };
        }

        public async Task<ServiceResponse<List<TaxRate>>> UpdateRates(List<TaxRate> newTaxRates)
        {
            var dbRates = await _context.TaxRates.ToListAsync();

            foreach (var rate in dbRates)
            {               
                rate.Rate = newTaxRates.FirstOrDefault(x => x.Id == rate.Id).Rate;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<List<TaxRate>>
            {
                Data = dbRates
            };
        }
    }
}
