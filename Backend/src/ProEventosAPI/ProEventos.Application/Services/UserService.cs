using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DTO;
using ProEventos.Application.Services.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInMananger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager,
                           SignInManager<User> signInMananger,
                           IMapper mapper,
                           IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInMananger = signInMananger;
            _mapper = mapper;
        }
        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar verificar o usuário existente. Erro :{e.Message}");
            }
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDTO, string password)
        {
            try
            {
                var user = await _userManager.Users
                                             .SingleOrDefaultAsync(user => user.UserName == userUpdateDTO.UserName.ToLower());

                return await _signInMananger.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar verificar a senha. Erro :{e.Message}");
            }
        }

        public async Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    var userReturn = _mapper.Map<UserUpdateDTO>(user);

                    return userReturn;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao criar conta. Erro :{e.Message}");
            }
        }

        public async Task<UserUpdateDTO> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userName);
                if (user == null) return null;

                var userUpdateDTO = _mapper.Map<UserUpdateDTO>(user);
                return userUpdateDTO;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao buscar por usuário. Erro :{e.Message}");
            }
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userUpdateDTO.UserName);
                if (user == null) return null;

                _mapper.Map(userUpdateDTO, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user); 
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDTO.Password);

                _userRepository.Update<User>(user);

                if (await _userRepository.SaveChangesAsync())
                {
                    var userReturn = await _userRepository.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDTO>(userReturn);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao atualizar a conta. Erro :{e.Message}");
            }
        }
    }
}
