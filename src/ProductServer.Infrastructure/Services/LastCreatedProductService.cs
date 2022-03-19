using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductServer.Application.Services.LastCreatedProductService;
using ProductServer.Infrastructure.SeedWork;

namespace ProductServer.Infrastructure.Services
{
    internal class LastCreatedProductService : ILastCreatedProductService
    {
        private const string cacheKey = "last_product";
        private static readonly SemaphoreSlim cacheSemaphore = new(1, 1);
        private static readonly TimeSpan cacheExpirationMinutes = TimeSpan.FromMinutes(1);

        private readonly IMemoryCache cache;
        private readonly IDbContextFactory<DatabaseContext> dbContextFactory;

        public LastCreatedProductService(IMemoryCache cache, IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            this.cache = cache;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<LastCreatedProductDto?> GetAsync(CancellationToken cancellationToken = default)
        {
            if (cache.TryGetValue(cacheKey, out LastCreatedProductDto product))
                return product;

            return await GetAtomicallyAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteToCacheAtomicallyAsync(LastCreatedProductDto product, CancellationToken cancellationToken = default)
        {
            await cacheSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                cache.Set(cacheKey, product, cacheExpirationMinutes);
            }
            finally
            {
                cacheSemaphore.Release();
            }
        }

        private async Task<LastCreatedProductDto?> GetAtomicallyAsync(CancellationToken cancellationToken = default)
        {
            await cacheSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                // Retry getting value from cache in case of race condition
                if (cache.TryGetValue(cacheKey, out LastCreatedProductDto cachedProduct))
                    return cachedProduct;

                LastCreatedProductDto? queriedProduct = await QueryAsync(cancellationToken).ConfigureAwait(false);

                if(queriedProduct is not null)
                    cache.Set(cacheKey, queriedProduct, cacheExpirationMinutes);

                return queriedProduct;
            }
            finally
            {
                cacheSemaphore.Release();
            }
        }

        private async Task<LastCreatedProductDto?> QueryAsync(CancellationToken cancellationToken)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
            return await dbContext.Products
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDateUtc)
                .Select(x => new LastCreatedProductDto(x.Id, x.Denomination))
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
