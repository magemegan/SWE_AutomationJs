namespace Server.Data.Entities;

public sealed class MenuCategory
{
    public int MenuCategoryId { get; set; }
    public string Name { get; set; } = "";
    public List<MenuItem> Items { get; set; } = new();
}

public sealed class MenuItem
{
    public int MenuItemId { get; set; }
    public int MenuCategoryId { get; set; }
    public MenuCategory Category { get; set; } = default!;

    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
}