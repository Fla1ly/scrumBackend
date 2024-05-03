public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserID { get; }
    public UserDto()
    {
        UserID = Guid.NewGuid().ToString();
    }
}
