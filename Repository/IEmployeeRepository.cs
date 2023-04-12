using EmployeeManagementProject.Model;

namespace EmployeeManagementProject.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeViewModel>> GetAllEmployee();
        Task<EmployeeViewModel> GetEmployeeById(int id);
        Task<Employee> CreateEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<Employee> DeleteEmployee(int id);
        Task<Employee> Login(string email, string password);
        Task<Employee> DownloadPdfForEmployee(int id);
        
    }
}
