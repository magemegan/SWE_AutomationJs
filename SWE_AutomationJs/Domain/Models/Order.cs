using System;
using System.Collections.Generic;


namespace Server.Data.Entities;

using Shared;

public sealed class Order
{
    public int OrderId { get; set; }

    public int RestaurantTableId { get; set; }
    public RestaurantTable RestaurantTable { get; set; } = default!;

    public int ServerEmployeeId { get; set; }
    public Employee ServerEmployee { get; set; } = default!;

    public OrderStatus Status { get; set; } = OrderStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedToKitchenAt { get; set; }
    public DateTime? ReadyAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
