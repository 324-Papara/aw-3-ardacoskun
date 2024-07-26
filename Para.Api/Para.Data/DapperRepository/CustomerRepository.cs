using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Para.Data.Domain;

namespace Para.Data.DapperRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<T?> QueryFirstOrDefaultAsync<T>(SqlConnection connection, string sql, object? param = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        private async Task<IEnumerable<T>> QueryAsync<T>(SqlConnection connection, string sql, object? param = null)
        {
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<IEnumerable<Customer>> GetCustomerDetails(long customerId)
        {
            const string customerQuery = "SELECT * FROM dbo.Customer WHERE Id = @CustomerId";
            const string detailQuery = "SELECT * FROM dbo.CustomerDetail WHERE CustomerId = @CustomerId";
            const string phoneQuery = "SELECT * FROM dbo.CustomerPhone WHERE CustomerId = @CustomerId";
            const string addressQuery = "SELECT * FROM dbo.CustomerAddress WHERE CustomerId = @CustomerId";

            using var connection = new SqlConnection(_configuration.GetConnectionString("MsSqlConnection"));

            var customer = await QueryFirstOrDefaultAsync<Customer>(connection, customerQuery, new { CustomerId = customerId });
            if (customer is null)
            {
                return Enumerable.Empty<Customer>();
            }

            customer.CustomerDetail = await QueryFirstOrDefaultAsync<CustomerDetail>(connection, detailQuery, new { CustomerId = customerId });
            customer.CustomerPhones = (await QueryAsync<CustomerPhone>(connection, phoneQuery, new { CustomerId = customerId })).ToList();
            customer.CustomerAddresses = (await QueryAsync<CustomerAddress>(connection, addressQuery, new { CustomerId = customerId })).ToList();

            return new List<Customer> { customer };
        }
    }
}
