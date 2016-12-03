

public enum EventID 
{
    None = 0,

    // pick a tsumo hai. 
    PickHai,
    // 捨牌の選択 
    Select_SuteHai,
    // 捨牌 
    SuteHai,
    // リーチ 
    Reach,
    // ポン 
    Pon,
    // チー(左) 
    Chii_Left,
    // チー(中央) 
    Chii_Center,
    // チー(右) 
    Chii_Right,
    // 大明槓 
    DaiMinKan,
    // 加槓 
    Kakan,
    // 暗槓 
    Ankan,
    // ロンのチェック 
    Ron_Check,
    // ツモあがり 
    Tsumo_Agari,
    // ロンあがり 
    Ron_Agari,
    // 流し 
    Nagashi,
}

public static class EventIDHelper
{
    public static UIEventID ToUIEventID( this EventID evt)
    {
        return (UIEventID)((int)evt);
    }
}
