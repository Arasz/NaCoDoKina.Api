﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using NaCoDoKina.Api.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class UserService : IUserService
    {
        private const string AuthenticationFailedMessage = "Authentication failed";

        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserId _authenticatedUserId;

        public UserService(IPasswordHasher<ApplicationUser> passwordHasher, IUserRepository userRepository, IMapper mapper, IAuthenticatedUserId authenticatedUserId)
        {
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authenticatedUserId = authenticatedUserId ?? throw new ArgumentNullException(nameof(authenticatedUserId));
        }

        public long GetCurrentUserId()
        {
            return _authenticatedUserId.Id;
        }

        public async Task<Result> AuthenticateAsync(User user, string password)
        {
            var internalUser = await _userRepository.GetUserByIdAsync(user.Id);

            if (internalUser is null)
                return Result.CreateFailed(AuthenticationFailedMessage);

            var result = _passwordHasher.VerifyHashedPassword(internalUser, internalUser.PasswordHash, password);

            switch (result)
            {
                case PasswordVerificationResult.Failed:

                    return Result.CreateFailed(AuthenticationFailedMessage);

                case PasswordVerificationResult.Success:
                    return Result.CreateSucceeded();

                case PasswordVerificationResult.SuccessRehashNeeded:
                    var rehashResult = await _userRepository.UpdateUserPassword(internalUser, password, password);

                    return rehashResult ? Result.CreateSucceeded() : Result.CreateFailed("Password cannot be updated");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (users is null || !users.Any())
                throw new UserNotFoundException();

            return _mapper.MapMany<ApplicationUser, User>(users);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);

            if (user is null)
                throw new UserNotFoundException(userName);

            return _mapper.Map<User>(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user is null)
                throw new UserNotFoundException(email);

            return _mapper.Map<User>(user);
        }

        public async Task<Result> CreateUserWithPasswordAsync(User user, string password)
        {
            var internalUser = _mapper.Map<ApplicationUser>(user);
            var wasCreated = await _userRepository.CreateUserWithPasswordAsync(internalUser, password);

            return wasCreated ? Result.CreateSucceeded() : Result.CreateFailed("User creation failed");
        }
    }
}