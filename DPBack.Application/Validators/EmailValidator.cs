using System.Text.RegularExpressions;
namespace DPBack.Application.Validators;

public static class EmailValidator
{
    private static Regex defaultRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

    public static bool IsValid(string email)
    {
        return defaultRegex.IsMatch(email); 
    }
}