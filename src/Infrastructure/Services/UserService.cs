using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using NaCoDoKina.Api.Models.Users;
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

        public async Task<Result<User>> AuthenticateAsync(User user, string password)
        {
            var internalUser = await _userRepository.GetUserByNameAsync(user.UserName);

            if (internalUser is null)
                return Result<User>.Failure<User>(AuthenticationFailedMessage);

            var result = _passwordHasher.VerifyHashedPassword(internalUser, internalUser.PasswordHash, password);

            switch (result)
            {
                case PasswordVerificationResult.Failed:

                    return Result<User>.Failure<User>(AuthenticationFailedMessage);

                case PasswordVerificationResult.Success:
                    var completeUser = _mapper.Map<User>(internalUser);
                    return Result<User>.CreateSucceeded(completeUser);

                case PasswordVerificationResult.SuccessRehashNeeded:
                    var rehashResult = await _userRepository.UpdateUserPassword(internalUser, password, password);
                    completeUser = _mapper.Map<User>(internalUser);
                    return rehashResult ? Result<User>.CreateSucceeded(completeUser) : Result<User>.Failure<User>("Password cannot be updated");

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

        public async Task<Result<User>> CreateUserWithPasswordAsync(User user, string password)
        {
            var internalUser = _mapper.Map<ApplicationUser>(user);
            internalUser = await _userRepository.CreateUserWithPasswordAsync(internalUser, password);

            if (internalUser is null)
                return Result<User>.Failure<User>("User creation failed");

            user = _mapper.Map<User>(internalUser);
            return Result<User>.CreateSucceeded(user);
        }
    }
}