using System;

namespace SWE_AutomationJs_UI_Design.Session
{
    public sealed class AuthenticatedEmployee
    {
        public string EmployeeId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }

        public string DisplayName
        {
            get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
        }
    }

    internal static class SessionContext
    {
        public static AuthenticatedEmployee CurrentEmployee { get; private set; }

        public static bool IsAuthenticated
        {
            get { return CurrentEmployee != null; }
        }

        public static void Start(AuthenticatedEmployee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            CurrentEmployee = employee;
        }

        public static void Clear()
        {
            CurrentEmployee = null;
        }
    }
}
