namespace SolutionAnalysisBug;

public static class UserRoles
{
    // These roles are saved in the database
    public const string Admin = "Admin";
    public const string Guest = "Guest";
    public const string Permanent = "Permanent";
    public const string Flex = "Flex";
    public const string OneTime = "OneTime";
    
    public static List<string> ActiveRoles { get; } = [Admin, Guest, Permanent, Flex, OneTime];

    public const string LoggedInUserRoles = $"{Admin},{Guest},{Permanent},{Flex}";
}
