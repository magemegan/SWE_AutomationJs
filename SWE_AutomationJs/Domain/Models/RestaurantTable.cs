using System;
using System.Collections.Generic;

namespace Server.Data.Entities;

using Shared;

public sealed class RestaurantTable
{
    public int RestaurantTableId { get; set; }
    public string Code { get; set; } = "";     // "A1"... "F6"

    // Layout coordinates used by WinForms to draw the grid
    public int RowIndex { get; set; }          // 0..5 for A..F
    public int ColIndex { get; set; }          // 0..5 for 1..6

    public TableStatus Status { get; set; } = TableStatus.Ready;

    // For highlighting assigned tables
    public int? AssignedWaiterId { get; set; }
    public Employee? AssignedWaiter { get; set; }

    // Concurrency control
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
