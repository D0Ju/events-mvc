using System.ComponentModel.DataAnnotations;

namespace events_mvc.Models;

public class EventTypeViewModel
{
    public int Id { get; set; }
    
    [Required]
    public string Naziv { get; set; } = "";
    
    public string Opis { get; set; } = "";
    public int MinimalnoPolaznika { get; set; }
}
