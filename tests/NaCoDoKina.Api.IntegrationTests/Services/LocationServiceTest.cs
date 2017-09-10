using Autofac;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Location = NaCoDoKina.Api.Models.Location;

namespace NaCoDoKina.Api.IntegrationTests.Services
{
    public class LocationServiceTest
    {
        private IContainer container;
        protected IGoogleDirectionsService DirectionsService { get; }

        protected IGoogleGeocodingService GeocodingService { get; }

        protected ILogger Logger { get; }

        protected IMapper Mapper { get; }

        public LocationServiceTest()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<HttpClient>()
                .AsSelf()
                .SingleInstance();

            containerBuilder.RegisterType<DebugLoggerProvider>()
                .As<ILoggerProvider>();

            containerBuilder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>();

            containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));

            containerBuilder.Register(context => new MapperConfiguration(cfg =>
                {
                    cfg.AddProfiles(typeof(ITravelService).Assembly);
                }))
                .As<IConfigurationProvider>()
                .AsSelf();

            containerBuilder.RegisterType<Mapper>()
                .AsImplementedInterfaces();

            containerBuilder.RegisterType<OnlyRequiredDirectionsRequestParser>()
                .AsImplementedInterfaces();
            containerBuilder.RegisterType<OnlyRequiredGeocodingRequestParser>()
                .AsImplementedInterfaces();

            containerBuilder.RegisterGeneric(typeof(GoogleServiceDependencies<>))
                .WithParameter("apiKey", "AIzaSyB0k9n49t5OXZ9XUfh8n9zUfhmdQ-_Tt5M")
                .AsSelf();

            containerBuilder.RegisterType<GoogleDirectionsService>()
                .AsImplementedInterfaces();

            containerBuilder.RegisterType<GoogleGeocodingService>()
                .AsImplementedInterfaces();

            containerBuilder.RegisterType<TravelService>()
                .AsImplementedInterfaces();

            container = containerBuilder.Build();

            ServiceUnderTest = container.Resolve<ITravelService>();
        }

        public ITravelService ServiceUnderTest { get; set; }

        public class TranslateAddressToLocationAsync : LocationServiceTest
        {
            [Fact]
            public async Task Should_return_correct_location_when_given_correct_address()
            {
                //arrange
                var testAddress = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
                var expectedLocation = new Location(16.882369, 52.4531839);

                //act
                var location = await ServiceUnderTest.TranslateAddressToLocationAsync(testAddress);

                //assert

                location.Latitude.Should().Be(expectedLocation.Latitude);
                location.Longitude.Should().Be(expectedLocation.Longitude);
            }

            [Fact]
            public async Task Should_return_null_when_api_returns_error()
            {
                //arrange
                var testAddress = "";

                //act
                var location = await ServiceUnderTest.TranslateAddressToLocationAsync(testAddress);

                //assert
                location.Should().BeNull();
            }
        }

        public class CalculateInformationForTravelAsync : LocationServiceTest
        {
            [Fact]
            public async Task Should_return_correct_travel_information()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.TravelPlan.Should().Be(travelPlan);
                travelInformation.Duration.Should().BeGreaterThan(TimeSpan.Zero);
                travelInformation.Distance.Should().BeGreaterThan(0);
            }

            [Fact]
            public async Task Should_return_longest_route_time_when_returns_multiple_routes()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.TravelPlan.Should().Be(travelPlan);
                travelInformation.Duration.Should().BeGreaterThan(TimeSpan.Zero);
                travelInformation.Distance.Should().BeGreaterThan(0);
            }

            [Fact]
            public async Task Should_return_null_and_log_when_api_returns_error()
            {
                //arrange
                var destination = new Location(-99999, -99999);
                var origin = new Location(-99999, -99999);
                var travelPlan = new TravelPlan(origin, destination);

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.Should().BeNull();
            }
        }

        protected Route CreateRouteWithOneLeg(int duration) => new Route
        {
            Legs = new List<Leg>
            {
                new Leg
                {
                    Duration = new TextValue
                    {
                        Value = duration
                    }
                }
            }
        };
    }
}