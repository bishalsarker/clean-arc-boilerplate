﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMess.Infrastructure.Persistence
{
    internal class ApplicationDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationDbSeeder _dbSeeder;
        private readonly ILogger<ApplicationDbInitializer> _logger;

        public ApplicationDbInitializer(
            ApplicationDbContext dbContext,
            ILogger<ApplicationDbInitializer> logger,
            ApplicationDbSeeder dbSeeder)
        {
            _dbContext = dbContext;
            _logger = logger;
            _dbSeeder = dbSeeder;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (_dbContext.Database.GetMigrations().Any())
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    _logger.LogInformation("Applying Migrations");

                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    _logger.LogInformation("Connection to database succeeded.");

                    await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
                }
            }
        }
    }
}
