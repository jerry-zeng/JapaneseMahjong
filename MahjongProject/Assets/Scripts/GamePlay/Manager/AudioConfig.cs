
/// <summary>
/// Roles: m = man, w = women
/// </summary>
public enum EVoiceType
{
    M_A = 1,
    M_B = 2,
    M_C = 3,
    M_D = 4,
    W_A = 5,
    W_B = 6,
    W_C = 7,
    W_D = 8,
}

public enum ECvType
{
    Pon = 1,
    Chii = 2,
    Kan = 3,
    Reach = 4,
    Ron = 5,
    Tsumo = 6,
    RyuuKyoku = 7,
    RKK_HaiTypeOver9 = 8,
    RKK_SuteFonHai4 = 9,
    RKK_Reach4 = 10,
    RKK_KanOver4 = 11,
    RKK_Ron3 = 12,
    ManGan = 13,
    HaReMan = 14,
    BaiMan = 15,
    SanBaiMan = 16,
    YakuMan = 17,
    Double_YakuMan = 18,
    Triple_YakuMan = 19,
    ORaSu = 22, //last kyoku
    Kyoku_Start = 23,
    Survival_Start = 24,
    NanBa_Start = 25,
}

/// <summary>
/// SE type.
/// </summary>
public enum ESeType
{
    SuteHai = 1,
    Agari = 2
}

public class AudioConfig
{
    public static string GetCVPath(EVoiceType role, ECvType cv)
    {
        string roleStr = role.ToString().ToLower();
        string num = ((int)cv).ToString("000");
        return string.Format("Sounds/CV/sm_cv{0}{1}", roleStr, num);
    }

    public static string GetSEPath(ESeType type)
    {
        return string.Format("Sounds/SE/sm_se{0}", ((int)type).ToString("000"));
    }
}
