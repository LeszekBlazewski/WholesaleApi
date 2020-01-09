using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Wholesale.BL.Models;
using Wholesale.BL.RepositoryInterfaces;
using Wholesale.BL.Helpers;

namespace Wholesale.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;

        public UserService(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _usersRepository.GetByEmail(email);

            if (user == null)
                return null;

            return password.VerifyPasswordHash(user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        public async Task<User> GetById(int userId)
        {
            return await _usersRepository.GetById(userId);
        }

        public async Task<List<User>> GetAll()
        {
            var users = await _usersRepository.GetAll();
            return users;
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Password is required");

            if (await _usersRepository.IsEmailTaken(user.Email))
                throw new InvalidOperationException($"An account with an email address \"{user.Email}\" already exists");

            password.CreatePasswordHash(out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _usersRepository.Create(user);

            return user;
        }

        public async Task<User> Update(User user, string password = null)
        {
            var userToUpdate = await _usersRepository.GetById(user.UserId.Value);

            if (userToUpdate == null)
                throw new InvalidOperationException("User not found");

            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email != userToUpdate.Email)
            {
                if (await _usersRepository.IsEmailTaken(user.Email))
                    throw new InvalidOperationException($"Email {user.Email} is already taken");

                userToUpdate.Email = user.Email;
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName))
                userToUpdate.FirstName = user.FirstName;

            if (!string.IsNullOrWhiteSpace(user.LastName))
                userToUpdate.LastName = user.LastName;

            if (!string.IsNullOrWhiteSpace(user.CompanyName))
                userToUpdate.CompanyName = user.CompanyName;

            if (!string.IsNullOrWhiteSpace(user.Phone))
                userToUpdate.Phone = user.Phone;

            userToUpdate.Role = user.Role;

            if (!string.IsNullOrWhiteSpace(password))
            {
                password.CreatePasswordHash(out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            await _usersRepository.Update(userToUpdate);
            return userToUpdate;
        }

        public async Task Delete(int id)
        {
            if (!await _usersRepository.IsIdTaken(id))
                throw new InvalidOperationException("User not found");

            await _usersRepository.Delete(id);
        }
    }
}
