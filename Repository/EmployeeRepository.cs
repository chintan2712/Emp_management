using EmployeeManagementProject.Context;
using EmployeeManagementProject.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementProject.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _appDbContext;
    public EmployeeRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Employee> CreateEmployee(Employee employee)
    {
        await _appDbContext.Employee.AddAsync(employee);
        _appDbContext.SaveChanges();
        return employee;
    }

    public async Task<List<EmployeeViewModel>> GetAllEmployee()
    {
        var emp = await _appDbContext.Employee.Select(x=>new EmployeeViewModel
        {
            Id=x.Id,
            Name=x.Name,
            Email=x.Email,
            City=x.City,
            Salary = x.Salary
        }). ToListAsync();
        return emp;
    }

    public async Task<EmployeeViewModel> GetEmployeeById(int id)
    {
        EmployeeViewModel employee = await _appDbContext.Employee.Select(x=>new EmployeeViewModel
        {
            Id=x.Id,
            Name = x.Name,
            Email = x.Email,
            City = x.City,
            Salary = x.Salary
        }).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (employee != null)
        {
            return employee;
        }
        return null;
    }

    public async Task<Employee> UpdateEmployee(Employee emp)
    {
        Employee employee = await _appDbContext.Employee.Where(x => x.Id == emp.Id).FirstOrDefaultAsync();
        if (employee != null)
        {
            if(emp.Salary!=employee.Salary)
                employee.Salary= emp.Salary;
            if(emp.Name!=null)
                employee.Name = emp.Name;
            if(emp.Email!=null)
                employee.Email = emp.Email;
            if(emp.City!=null)
                employee.City = emp.City;
            if(emp.Password !=null)
                employee.Password = emp.Password;
            _appDbContext.SaveChanges();
            return employee;
        }
        return null;
    }

    public async Task<Employee> DeleteEmployee(int id)
    {
        Employee employee = await _appDbContext.Employee.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (employee != null)
        {
            _appDbContext.Employee.Remove(employee);
            await _appDbContext.SaveChangesAsync();
        }
        return null;
    }
    public async Task<Employee> Login(string email, string password)
    {
        var employee = await _appDbContext.Employee.Where(x => x.Email == email && x.Password==password).FirstOrDefaultAsync();
        return employee;
    }
    public async Task<Employee> DownloadPdfForEmployee(int id)
    {
        Employee employee = await _appDbContext.Employee.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (employee != null)
        {
            return employee;
        }
        return null;
    }

   
}