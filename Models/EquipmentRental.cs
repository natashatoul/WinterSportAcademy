namespace WinterSportAcademy.Models;
public class EquipmentRental
{
    public int EquipmentRentalID {get; set;}
    public int StudentID {get; set;}
    public string EquipmentType {get; set;} = string.Empty;
    public string EquipmentSize {get; set;} = string.Empty;
    public DateTime RentalDate {get; set;}
    public DateTime ReturnDate {get; set;}
    public decimal DailyRate {get; set;}
    public decimal TotalCost {get; set;}
    public string Status {get; set;} = "Active";
    public Student Student {get; set;} = null!;
}