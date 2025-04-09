using InTN.Debugging;

namespace InTN;

public class InTNConsts
{
    public const string LocalizationSourceName = "InTN";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "7ca34ecc9ec34668b21f68fd5803b395";
}
