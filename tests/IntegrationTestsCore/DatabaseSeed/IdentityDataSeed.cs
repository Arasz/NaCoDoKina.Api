using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace IntegrationTestsCore.DatabaseSeed
{
    public class IdentityDataSeed : IDatabaseSeed
    {
        private readonly IUserRepository _userRepository;
        private readonly IntegrationTestsSettings _apiSettings;

        private readonly ILogger<IdentityDataSeed> _logger;
        public ApplicationIdentityContext DbContext { get; }

        public IdentityDataSeed(ApplicationIdentityContext identityContext, IUserRepository userRepository, ILogger<IdentityDataSeed> logger, IntegrationTestsSettings apiSettings)
        {
            _userRepository = userRepository;
            _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DbContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
        }

        public void Seed()
        {
            using (_logger.BeginScope(nameof(IdentityDataSeed)))
            {
                _logger.LogInformation("Ensure database is created");
                DbContext.Database.Migrate();
                _logger.LogInformation("Migration completed");

                _logger.LogInformation("Create users");

                var applicationUserWithPassword = Enumerable.Range(1, 15)
                    .Select(i =>
                    {
                        var user = new ApplicationUser
                        {
                            Email = $"{_apiSettings.DefaultUserName}{i}",
                            UserName = $"{_apiSettings.DefaultUserName}{i}"
                        };
                        _logger.LogInformation("Create user {@user} task", user);
                        return (User: user, Password: _apiSettings.DefaultUserPassword);
                    });

                foreach (var userWithPassword in applicationUserWithPassword)
                {
                    _userRepository.CreateUserWithPasswordAsync(userWithPassword.User, userWithPassword.Password).GetAwaiter().GetResult();
                }

                _logger.LogInformation("All users created");
            }
        }

        public void Dispose()
        {
            if (DbContext.Database.EnsureDeleted())
                _logger.LogInformation("Database for context {contextName} was deleted", DbContext.GetType().Name);
            DbContext?.Dispose();
        }
    }
}