using AutoMapper;

namespace NaCoDoKina.Api.Infrastructure.Profiles
{
    public class ProfileBaseTest<TProfile> where TProfile : Profile, new()
    {
        protected TProfile ProfileUnderTest { get; }

        protected IMapper Mapper { get; }

        public ProfileBaseTest()
        {
            ProfileUnderTest = new TProfile();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(ProfileUnderTest));
            Mapper = new Mapper(configuration);
        }
    }
}