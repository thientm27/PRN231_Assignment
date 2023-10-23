using CarRenting.DTOs;
using CarRenting.DTOs.Request;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepo _repository = new CustomerRepo();
    private readonly AppSetting _appSettings = new AppSetting();

    public CustomerController( IOptionsMonitor<AppSetting> appSettings)
    {
        _appSettings = appSettings.CurrentValue;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        
        var reslut = await _repository.GetAsync();

        return Ok(reslut);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reslut = await _repository.GetByIdAsync(id);
        return Ok(reslut);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerDto customerDto)
    {
        var createdCustomer = await _repository.AddAsync(customerDto);
        return Ok(createdCustomer);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomerDto customerDto)
    {
        var result = await _repository.UpdateAsync(customerDto);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginCustomer = await _repository.LoginAsync(loginRequest.Email, loginRequest.Password);
            
        if (loginCustomer == null)
        {
            return NotFound("Invalid email or password");
        }

        var tokenModel = GenerateToken(loginCustomer);
        loginCustomer.Data = tokenModel;
        SetRefreshToken(tokenModel.RefreshToken);

        return Ok(loginCustomer);
    }
    private TokenModel GenerateToken(CustomerDto customerDto)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                   new Claim("Id", customerDto.CustomerId.ToString()),
                    new Claim(ClaimTypes.Name, customerDto.CustomerName),
                    new Claim(ClaimTypes.Email, customerDto.Email),


                    //roles

                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescription);

        var acessModel = jwtTokenHandler.WriteToken(token);

        return new TokenModel
        {
            AccessToken = acessModel,
            RefreshToken = GenerateRefreshToken(),
        };
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            TokenExpires = DateTime.Now.AddDays(7)
        };
        return refreshToken;
    }

    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOption = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.TokenExpires,
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOption);

    }
}