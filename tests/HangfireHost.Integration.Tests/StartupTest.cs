﻿using ApplicationCore.Repositories;
using AutoMapper;
using FluentAssertions;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Infrastructure.Settings;
using Infrastructure.Settings.Google;
using IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HangfireHost.Integration.Tests
{
    [Collection("Tests with hangfire")]
    public class StartupTest : HttpTestBase<Startup>
    {
        public class ConfigureServicesTest : StartupTest
        {
            [Fact]
            public void Should_return_cinema_service()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var cinemaService = serviceProvider.GetService<ICinemaService>();

                // Assert
                cinemaService.Should().NotBeNull();
            }

            [Fact]
            public void Should_return_google_api_settings()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var googleApiSettings = serviceProvider.GetService<GoogleApiSettings>();

                // Assert
                googleApiSettings.Should().NotBeNull();
                googleApiSettings.ApiKey.Should().NotBeNullOrEmpty();
            }

            [Fact]
            public void Should_return_user_authenticated_user_id_provider()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var userManager = serviceProvider.GetService<IAuthenticatedUserId>();

                // Assert
                userManager.Should().NotBeNull();
            }

            [Fact]
            public void Should_return_mapper()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var mapper = serviceProvider.GetService<IMapper>();

                // Assert
                mapper.Should().NotBeNull();
            }

            [Fact]
            public void Should_return_cinema_repository()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var repository = serviceProvider.GetService<ICinemaRepository>();

                // Assert
                repository.Should().NotBeNull();
            }

            [Fact]
            public void Should_return_cinema_city_cinemas_task()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var repository = serviceProvider.GetService<LoadCinemaCityCinemasTask>();

                // Assert
                repository.Should().NotBeNull();
            }
        }
    }
}