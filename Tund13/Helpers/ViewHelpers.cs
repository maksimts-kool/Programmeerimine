using Tund13.Models;

namespace Tund13.Helpers;

public static class ViewHelpers
{
    /// CSS klass kursuse taseme järgi (kk-level-*)
    public static string TaseBadgeClass(KursuseTase tase) => tase switch
    {
        KursuseTase.A1 or KursuseTase.A2 => "kk-level kk-level-a",
        KursuseTase.B1 or KursuseTase.B2 => "kk-level kk-level-b",
        KursuseTase.C1 or KursuseTase.C2 => "kk-level kk-level-c",
        _ => "kk-level kk-level-b"
    };

    /// Riigilipp emojina keele nime järgi
    public static string KeeleFlag(string keel) => keel.ToLowerInvariant() switch
    {
        "inglise" => "🇬🇧",
        "saksa" => "🇩🇪",
        "prantsuse" => "🇫🇷",
        "hispaania" => "🇪🇸",
        "itaalia" => "🇮🇹",
        "vene" => "🇷🇺",
        "soome" => "🇫🇮",
        "eesti" => "🇪🇪",
        "hiina" => "🇨🇳",
        "jaapani" => "🇯🇵",
        "araabia" => "🇸🇦",
        "portugali" => "🇵🇹",
        _ => "🌐"
    };

    /// Registreerimise staatuse CSS klass
    public static string StaatusBadgeClass(RegistreerimiseStaatus staatus) => staatus switch
    {
        RegistreerimiseStaatus.Kinnitatud => "kk-status kk-status-ok",
        RegistreerimiseStaatus.Ootel     => "kk-status kk-status-wait",
        RegistreerimiseStaatus.Tuhistatud => "kk-status kk-status-cancel",
        _ => "kk-status kk-status-cancel"
    };
}
