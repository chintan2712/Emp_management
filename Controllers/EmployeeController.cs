using EmployeeManagementProject.Model;
using EmployeeManagementProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EmployeeManagementProject.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    [HttpGet]
    public async Task<ActionResult<List<EmployeeViewModel>>> GetAll()
    {
        return Ok(await _employeeRepository.GetAllEmployee());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeViewModel>> GetById(int id)
    {
        var empdetail = await _employeeRepository.GetEmployeeById(id);
        if (empdetail != null)
        {
            return Ok(empdetail);
        }
        return BadRequest();
    }
     
    [HttpPut]
    public async Task<ActionResult<Employee>> Update(Employee employee)
    {
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(await _employeeRepository.UpdateEmployee(employee));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Employee>> Delete(int id)
    {
        var empdetail = await _employeeRepository.DeleteEmployee(id);
        if (empdetail == null)
        {
            return Ok(empdetail);
        }
        return BadRequest();
    }


[HttpGet("{id:int}")]
public async Task<IActionResult> GeneratePdf(int id)
{
    EmployeeViewModel employee = await _employeeRepository.GetEmployeeById(id);
    if (employee != null)
    {
        var document = new PdfDocument();
        string content = "<h1 style=\"text-align:center;\"> Welcome to Employee Management Portal </h1>";
        content += "<div style='width:100%;padding:5px;margin:5px;border:1px solid #ccc'>";
        content += "<h2 style=\"text-align:center;\"> Employee Id : " + employee.Id + "</<h2>";
        content += "<h6 style=\"text-align:left;\"> Name : " + employee.Name + "</h6>";
        content += "<h6 style=\"text-align:left;\"> City : " + employee.City + "</h6>";
        content += "<h6 style=\"text-align:left;\"> Salary :" + employee.Salary + "</h6>";
        content += "<h6 style=\"text-align:left;\"> Email :" + employee.Email + "</h6>";
        content += "</div>";
        PdfGenerator.AddPdfPages(document, content, PageSize.A4);
        byte[] resp = null;
        using (MemoryStream stream = new MemoryStream())
        {
            document.Save(stream);
            resp = stream.ToArray();
        }
        string fileName = String.Empty;
        fileName = Guid.NewGuid().ToString() + "_" + employee.Id + ".pdf";
        return File(resp, "application/pdf", fileName);
    }
    return BadRequest();
}
}
