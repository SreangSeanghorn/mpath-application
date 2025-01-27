using FluentValidation;
using MPath.Application.Commands;
using MPath.Application.Exceptions;
using MPath.Application.Exceptions.Roles;
using MPath.Application.Exceptions.UserRegistered;
using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.Domain;
using MPath.Domain.Core.Interfaces;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.CommandHandlers;

public class UserRegisteredCommandHandler : ICommandHandler<UserRegisterCommand,UserRegisteredResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<UserRegisterCommand> _userRegisterCommandValidator;
    private readonly IRoleRepository _roleRepository;
    private IUnitOfWork _unitOfWork;
    
    public UserRegisteredCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IValidator<UserRegisterCommand> userRegisterCommandValidator, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _userRegisterCommandValidator = userRegisterCommandValidator;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UserRegisteredResponse> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var valid = _userRegisterCommandValidator.Validate(request);
        if (!valid.IsValid)
        {
            throw new InvalidUserRegisterException(valid.Errors.First().ErrorMessage);
        }
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var user = _userRepository.GetByEmail(request.Email).Result;
        if (user != null)
        {
            throw new UserAlreadyExistException(request.Email);
        }
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new RoleNotFoundException();
        }
        var providedMail = new Email(request.Email);
        var newUser =  User.Register(request.UserName, providedMail, hashedPassword, role);
        await _userRepository.AddAsync(newUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var userRegisteredResponse = new UserRegisteredResponse(newUser.UserName, newUser.Email.Value);
        return userRegisteredResponse;
    }
}