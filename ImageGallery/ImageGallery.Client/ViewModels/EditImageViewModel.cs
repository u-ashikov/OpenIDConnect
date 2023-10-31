namespace ImageGallery.Client.ViewModels;

using System.ComponentModel.DataAnnotations;

public class EditImageViewModel
{
    [Required]
    public Guid Id { get; set; }  
    
    [Required]
    public string Title { get; set; } = string.Empty;
}