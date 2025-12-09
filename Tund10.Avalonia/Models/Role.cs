namespace Tund10.Avalonia.Models;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool CanManageOwners { get; set; }
    public bool CanManageCars { get; set; }
    public bool CanManageServices { get; set; }
    public bool CanChangeStatus { get; set; }
    public bool CanManageWorkers { get; set; }
}
