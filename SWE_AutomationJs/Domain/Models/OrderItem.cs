using System;
using System.Collections.Generic;


namespace Server.Data.Entities;

public sealed class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = default!;

    public int SeatNumber { get; set; }     // seat-specific requirement
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }  // price at order time
    public string? Notes { get; set; }
}
