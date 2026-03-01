namespace Shared;

public enum EmployeeRole
{
    WaitStaff = 1,
    KitchenStaff = 2,
    Busboy = 3,
    Manager = 4
}

public enum TableStatus
{
    Ready = 1,
    Occupied = 2,
    Dirty = 3
}

public enum OrderStatus
{
    Open = 1,
    SubmittedToKitchen = 2,
    Preparing = 3,
    Ready = 4,
    Completed = 5,
    Voided = 6
}
