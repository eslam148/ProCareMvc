using ProCareMvc.Database.Utility;

public class UserProfileViewModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; } 
    public string ImageProfileUrl { get; set; } = "doc-5.jpg";
    public string Department { get; set; }

}
