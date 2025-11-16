namespace Library.Net2.Models.Enums;

public enum LoanStatus
{
    Pending = 0,    // Onay bekliyor
    Borrowed = 1,   // Ödünç verildi
    Returned = 2,   // İade edildi
    Late = 3,       // Gecikmiş
    Cancelled = 4   // İptal edildi
}

