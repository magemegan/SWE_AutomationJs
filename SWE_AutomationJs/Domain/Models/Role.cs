using System;
using System.Collections.Generic;


namespace Server.Data.Entities;

public sealed class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; } = ""; // "WaitStaff", "KitchenStaff", etc.
}
