using API.Catalog.Application.DataAccess;
using API.Catalog.Application.Dto;
using Dapper;
using Infrastructure.Base.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace API.Catalog.Infrastructure.DataAccess
{
    public class ProductQueries : IProductQueries
    {
        private readonly QueryConnectionModel _connectionModel;
        private readonly ILogger<ProductQueries> _logger;

        private const string ProductTable = "ms_catalog.Products";

        public ProductQueries(QueryConnectionModel connectionModel, ILogger<ProductQueries> logger)
        {
            _connectionModel = connectionModel;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetLatestProductAsync(int limit)
        {
            IEnumerable<ProductDto> products = null;

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionModel.ConnectionString);

                await connection.OpenAsync();

                var query = string.Format("SELECT TOP(@limit) Id, Name, Description, Price FROM {0}", ProductTable);

                products = await connection.QueryAsync<ProductDto>(query, new { Limit = limit });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return products;
        }
    }
}
