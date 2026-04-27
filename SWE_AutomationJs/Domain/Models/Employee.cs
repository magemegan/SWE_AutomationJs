using System;
using System.Collections.Generic;

namespace Server.Data.Entities;

public sealed class Employee
{
    public int EmployeeId { get; set; }

    // Login
    public string Username { get; set; } = "";
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    // Display
    public string DisplayName { get; set; } = "";

    // Role (simple 1 role per employee for prototype; can expand to many-to-many later)
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;

    // Employment / tracking
    public bool IsActive { get; set; } = true;
}
