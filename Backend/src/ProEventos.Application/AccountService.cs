using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager,
                            IMapper mapper, IUserPersist userPersist)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
            _userManager = userManager;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userUpdateDto.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar senha.{ex.Message}");
            }
        }
        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar criar conta.{ex.Message}");
            }
        }
        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userName);
                if (user == null) return null;
                var userUpdate = _mapper.Map<UserUpdateDto>(user);
                return userUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar pegar usuário pelo nome de usuário.{ex.Message}");
            }
        }
        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName);
                if(user == null) return null;

                userUpdateDto.Id = user.Id;
                _mapper.Map(userUpdateDto, user);
                if(userUpdateDto.Password != null){
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);              
                }
                _userPersist.Update<User>(user);
                if(await _userPersist.SaveChangesAsync()){
                    var userRetorno = await _userPersist.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar conta.{ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(u => u.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar a existência do usuário.{ex.Message}");
            }
        }
    }
}