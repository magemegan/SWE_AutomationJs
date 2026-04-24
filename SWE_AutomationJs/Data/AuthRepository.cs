using Dapper;
using SWE_AutomationJs_UI_Design.Security;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design.Data
{
    internal static class AuthRepository
    {
        public static AuthenticatedEmployee Authenticate(string employeeId, string password)
        {
            if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    e.EmployeeId,
    e.RoleId,
    r.RoleName,
    e.FirstName,
    e.LastName,
    e.IsActive,
    e.PasswordHash
FROM Employees e
INNER JOIN Roles r ON r.RoleId = e.RoleId
WHERE e.EmployeeId = @EmployeeId;";

                AuthenticatedEmployee employee = connection.QuerySingleOrDefault<AuthenticatedEmployee>(
                    sql,
                    new { EmployeeId = employeeId.Trim().ToUpperInvariant() });

                if (employee == null || !employee.IsActive)
                {
                    return null;
                }

                return PasswordHasher.VerifyPassword(password, employee.PasswordHash)
                    ? employee
                    : null;
            }
        }
    }
}
