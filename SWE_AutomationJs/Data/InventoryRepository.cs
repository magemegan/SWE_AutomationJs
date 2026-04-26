using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SWE_AutomationJs_UI_Design.Data
{
    public sealed class InventoryItemInfo
    {
        public int InventoryItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal ReorderLevel { get; set; }

        public string Status
        {
            get
            {
                if (QuantityOnHand <= 0)
                {
                    return "Reorder";
                }

                return QuantityOnHand <= ReorderLevel ? "Low Stock" : "In Stock";
            }
        }
    }

    public static class InventoryRepository
    {
        public static IReadOnlyList<InventoryItemInfo> GetActiveInventoryItems()
        {
            using (var connection = Db.Open())
            {
                const string sql = @"
SELECT
    InventoryItemId,
    ItemName,
    UnitOfMeasure,
    QuantityOnHand,
    ReorderLevel
FROM InventoryItems
WHERE IsActive = 1
ORDER BY ItemName;";

                return connection.Query<InventoryItemInfo>(sql).ToList();
            }
        }
    }
}
