using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed
{
    public class IdentityDataSeed : IDatabaseSeed
    {
        private readonly IntegrationTestApiSettings _apiSettings;

        private int _userCounter = 1;

        private readonly IUserManager _identityService;
        private readonly IFixture _fixture;
        private readonly ILogger<IdentityDataSeed> _logger;
        public ApplicationIdentityContext DbContext { get; }

        public IdentityDataSeed(ApplicationIdentityContext identityContext, IUserManager userManager, ILogger<IdentityDataSeed> logger, IFixture fixture, IntegrationTestApiSettings apiSettings)
        {
            _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
            _identityService = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
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

                _logger.LogInformation("Create users data");
                _fixture.Register(() => new CreateUser
                {
                    Email = $"{_apiSettings.DefaultUserName}{_userCounter++}",
                    Password = _apiSettings.DefaultUserPassword,
                });

                _logger.LogInformation("Create users");

                var applicationUserWithPassword = _fixture.CreateMany<CreateUser>(15)
                    .Select(user =>
                    {
                        _logger.LogInformation("Create user {@user} task", user);
                        return (User: new ApplicationUser
                        {
                            Email = user.Email,
                            UserName = user.Email
                        }, Password: user.Password);
                    });

                foreach (var userWithPassword in applicationUserWithPassword)
                {
                    _identityService.CreateAsync(userWithPassword.User, userWithPassword.Password).GetAwaiter().GetResult();
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