using System.ComponentModel.DataAnnotations;

public class TokenDTO
{
    [Required(ErrorMessage = "This field is Required!")]
    public string Username { get; set; }

    [Required(ErrorMessage = "This field is Required!")]
    public string Password { get; set; }
}