using EmployeeManagementProject.Model;
using EmployeeManagementProject.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementProject.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IEmployeeRepository _employeeRepository;
    public AccountController(IEmployeeRepository employeeRepository,  IConfiguration configuration)
    {
        _employeeRepository = employeeRepository;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create(Employee employee)
    {
        return Ok(await _employeeRepository.CreateEmployee(employee));
    }
    [HttpPost]
    public async Task<IActionResult> Login(string emailId, string password)
    {
        Employee login = await _employeeRepository.Login(emailId, password);
        if (login != null)
        {
            var wantedToken = GenerateJsonWebToken(login);

            return Ok(wantedToken);
        }
        return BadRequest();
    }

    [NonAction]
    private string GenerateJsonWebToken(Employee employee)
    {
        try
        {
            var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "ABCXYZ",
                audience: "https://" + HttpContext.Request.Host.Value,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        catch
        {
            throw new Exception(BadRequest().ToString());
        }
    }
}
