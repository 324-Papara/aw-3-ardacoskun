using Para.Data.Domain;

namespace Para.Data.DapperRepository;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomerDetails(long customerId);
}
