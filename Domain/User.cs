using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public PasswordModel PasswordModel { get; set; }
    public UserType UserType { get; private set; } = UserType.Member;

    /// <summary>
    ///  a user who has more privilege can assign a usertype that is lower than its to a user 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="requestContextUserType"></param>
    public void SetUserType(UserType type, UserType requestContextUserType)
    {
        if (((int)type) <= ((int)requestContextUserType))
        {
            throw new LessPrivilegeException();
        }

        UserType = type;
    }


}


public class PasswordModel
{
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
}
