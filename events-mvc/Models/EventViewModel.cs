using System.ComponentModel.DataAnnotations;

namespace events_mvc.Models;

public class EventViewModel
{
    public int Id { get; set; }
    
    [Required]
    public string Naziv { get; set; } = "";
    
    [Required]
    public string Lokacija { get; set; } = "";
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime? DatumPocetka { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime? DatumZavrsetka { get; set; }
    
    public int BrojPolaznika { get; set; }
    public decimal Cijena { get; set; }
    public string Opis { get; set; } = "";
    public bool Aktivan { get; set; }
    public int VrstaId { get; set; }
    public string? VrstaNaziv { get; set; }
}
