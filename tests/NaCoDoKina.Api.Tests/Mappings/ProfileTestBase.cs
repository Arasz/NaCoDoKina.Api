using AutoMapper;

namespace NaCoDoKina.Api.Mappings
{
    public class ProfileTestBase<TProfile> : UnitTestBase
        where TProfile : Profile, new()
    {
        protected TProfile ProfileUnderTest { get; }

        protected IMapper Mapper { get; }

        public ProfileTestBase()
        {
            ProfileUnderTest = new TProfile();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(ProfileUnderTest));
            Mapper = new Mapper(configuration);
        }
    }
}