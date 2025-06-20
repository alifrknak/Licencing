namespace Domain;

public class LessPrivilegeException : Exception
{
    public override string Message => "You tried to perform an operation that  you are not authorized to";
}