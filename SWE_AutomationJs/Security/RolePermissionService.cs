using System;
using SWE_AutomationJs_UI_Design.Session;

namespace SWE_AutomationJs_UI_Design.Security
{
    internal static class RolePermissionService
    {
        // ===== ROLE CHECKS =====

        public static bool IsManager()
        {
            return HasRole("Manager") || HasRole("Admin");
        }

        public static bool IsWaiter()
        {
            return HasRole("Waiter") || HasRole("Server");
        }

        public static bool IsKitchen()
        {
            return HasRole("Cook") || HasRole("Chef") || HasRole("Kitchen");
        }

        public static bool IsBusboy()
        {
            return HasRole("Busboy");
        }

        // ===== PERMISSION LOGIC =====

        public static bool CanChangeTableStatus(string currentStatus, string targetStatus)
        {
            if (!SessionContext.IsAuthenticated)
                return false;

            // Manager can do anything
            if (IsManager())
                return true;

            // WAITER RULES
            // Available → Occupied
            // Occupied → Dirty
            if (IsWaiter())
            {
                return
                    (currentStatus == "Available" && targetStatus == "Occupied") ||
                    (currentStatus == "Occupied" && targetStatus == "Dirty");
            }

            // BUSBOY RULE
            // Dirty → Available
            if (IsBusboy())
            {
                return currentStatus == "Dirty" && targetStatus == "Available";
            }

            // Kitchen cannot change table states
            return false;
        }

        // ===== HELPER =====

        private static bool HasRole(string roleName)
        {
            return SessionContext.IsAuthenticated &&
                   SessionContext.CurrentEmployee != null &&
                   string.Equals(
                       SessionContext.CurrentEmployee.RoleName,
                       roleName,
                       StringComparison.OrdinalIgnoreCase
                   );
        }
    }
}