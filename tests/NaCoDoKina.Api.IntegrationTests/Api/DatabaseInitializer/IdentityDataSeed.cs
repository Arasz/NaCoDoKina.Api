using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer
{
    public class IdentityDataSeed : IDatabaseSeed<ApplicationIdentityContext>
    {
        public string UniversalPassword => "Aw23_ffdD3df_ddw!efefdewww";

        public string UniversalEmail => "a@bmail.com";

        private int _userCounter;

        private readonly IUserManager _identityService;
        private readonly IFixture _fixture;
        private readonly ILogger<IdentityDataSeed> _logger;
        public ApplicationIdentityContext DbContext { get; }

        public IdentityDataSeed(ApplicationIdentityContext identityContext, IUserManager userManager, ILogger<IdentityDataSeed> logger, IFixture fixture)
        {
            _identityService = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DbContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
        }

        public async Task SeedAsync()
        {
            using (_logger.BeginScope(nameof(IdentityDataSeed)))
            {
                if (DbContext.Database.EnsureDeleted())
                    _logger.LogInformation("Database for context {contextName} was deleted", DbContext.GetType().Name);

                _logger.LogInformation("Ensure database is created");
                await DbContext.Database.MigrateAsync();
                _logger.LogInformation("Migration completed");

                _logger.LogInformation("Create users data");
                _fixture.Register(() => new RegisterUser
                {
                    Email = $"{UniversalEmail}{_userCounter++}",
                    Password = UniversalPassword
                });

                _logger.LogInformation("Create users");

                var applicationUserWithPassword = _fixture.CreateMany<RegisterUser>(15)
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
                    await _identityService.CreateAsync(userWithPassword.User, userWithPassword.Password);
                }

                _logger.LogInformation("All users created");
            }
        }
    }
}