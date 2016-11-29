

/** イベントID */
public enum EventId 
{
    None = 0,

    /** ゲームの開始 */
    START_GAME = 1,
    /** 局の開始 */
    START_KYOKU,
    /** ツモ */
    TSUMO,
    /** 捨牌の選択 */
    SELECT_SUTEHAI,
    /** 捨牌 */
    SUTEHAI,
    /** リーチ */
    REACH,
    /** ポン */
    PON,
    /** チー(左) */
    CHII_LEFT,
    /** チー(中央) */
    CHII_CENTER,
    /** チー(右) */
    CHII_RIGHT,
    /** 大明槓 */
    DAIMINKAN,
    /** 加槓 */
    KAN,
    /** 暗槓 */
    ANKAN,
    /** ツモあがり */
    TSUMO_AGARI,
    /** ロンあがり */
    RON_AGARI,
    /** 流し */
    NAGASHI,
    /** 流局 */
    RYUUKYOKU,
    /** 局の終了 */
    END_KYOKU,
    /** ゲームの終了 */
    END_GAME,


    //-----------------------------About UI Event----------------------------
    Init_Game = 100,

    Saifuri_For_Qin,
    On_Saifuri_For_Qin_End,

    Init_PlayerInfoUI,

    Saifuri_For_Haipai,
    On_Saifuri_For_Haipai_End,

    SetYama_BeforeHaipai,
    SetUI_AfterHaipai,

    /** ロンのチェック */
    RON_CHECK,

    /** 理牌待ち */
    UI_WAIT_RIHAI,
    /** 進行待ち */
    UI_WAIT_PROGRESS,
    /** プレイヤーアクションの入力 */
    UI_INPUT_PLAYER_ACTION,

    // 連荘 //
    RENCHAN,

    // init ui //
    INIT_UI,

    // 摇色子sai furi.//
    SAIFURI,

    // 配牌 //
    HAIPAI
}
