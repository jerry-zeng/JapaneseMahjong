
public enum UIEventType 
{
    #region event id
    // pick a tsumo hai. 
    PickTsumoHai,
    PickRinshanHai,

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
    #endregion

    DisplayMenuList,
    HideMenuList,

    OnPlayerInput,

    // ゲームの開始 
    Start_Game,
    // 連荘
    RenChan,
    // 局の開始 
    Start_Kyoku,

    Init_Game,

    // Saifuri
    Saifuri,
    On_Saifuri_End,

    // 配牌
    HaiPai,

    Init_PlayerInfoUI,

    Saifuri_For_Haipai,
    On_Saifuri_For_Haipai_End,

    SetYama_BeforeHaipai,
    SetUI_AfterHaipai,

    Display_Agari_Panel,

    // 流局 
    RyuuKyoku,

    // 局の終了 
    End_Kyoku,

    // ゲームの終了 
    End_Game,


}
