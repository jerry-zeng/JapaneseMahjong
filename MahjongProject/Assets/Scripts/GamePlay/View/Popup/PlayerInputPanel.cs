using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerInputPanel : UIObject 
{
    public PlayerInputButton btn_Pon;
    public PlayerInputButton btn_Chii;
    public PlayerInputButton btn_Kan;
    public PlayerInputButton btn_Reach;
    public PlayerInputButton btn_Agari;
    public PlayerInputButton btn_Nagashi;
    public PlayerInputButton btn_RyuuKyoku;

    private PlayerUI playerUI;



    void Start()
    {
        Init();

        btn_Pon.index = 0;
        btn_Pon.SetOnClick( OnClick_Pon );
        btn_Pon.SetTag( ResManager.getString("button_pon") );
        btn_Chii.index = 1;
        btn_Chii.SetOnClick( OnClick_Chii );
        btn_Chii.SetTag( ResManager.getString("button_chii") );
        btn_Kan.index = 2;
        btn_Kan.SetOnClick( OnClick_Kan );
        btn_Kan.SetTag( ResManager.getString("button_kan") );
        btn_Reach.index = 3;
        btn_Reach.SetOnClick( OnClick_Reach );
        btn_Reach.SetTag( ResManager.getString("button_reach") );
        btn_Agari.index = 4;
        btn_Agari.SetOnClick( OnClick_Agari );
        btn_Agari.SetTag( ResManager.getString("button_tsumo") );
        btn_Nagashi.index = 5;
        btn_Nagashi.SetOnClick( Onclick_Nagashi );
        btn_Nagashi.SetTag( ResManager.getString("button_pass") );

        btn_RyuuKyoku.index = 6;
        btn_RyuuKyoku.SetOnClick( Onclick_Nagashi );
        btn_RyuuKyoku.SetTag( ResManager.getString(ERyuuKyokuReason.HaiTypeOver9.ToString()) );
    }

    public void SetOwnerPlayerUI(PlayerUI ui)
    {
        this.playerUI = ui;
    }


    public bool isMenuEnable( EActionType menuItem )
    {
        return PlayerAction.MenuList.Contains(menuItem);
    }


    protected void NotifyHide()
    {
        EventManager.Get().SendEvent(UIEventType.HideMenuList);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);

        RefreshMenuButtons();
    }

    void RefreshMenuButtons()
    {
        btn_Pon.SetEnable( isMenuEnable(EActionType.Pon) );

        btn_Chii.SetEnable( isMenuEnable(EActionType.Chii) );
        btn_Chii.SetTag( ResManager.getString("button_chii") );

        btn_Kan.SetEnable( isMenuEnable(EActionType.Kan) );
        btn_Kan.SetTag( ResManager.getString("button_kan") );

        btn_Reach.SetEnable( isMenuEnable(EActionType.Reach) );
        btn_Reach.SetTag( ResManager.getString("button_reach") );

        btn_Agari.SetEnable( isMenuEnable(EActionType.Ron) || isMenuEnable(EActionType.Tsumo) );

        if( isMenuEnable(EActionType.Ron) || OwnerPlayer.Action.State != EActionState.Select_Sutehai )
            btn_Agari.SetTag( ResManager.getString("button_ron") );
        if( isMenuEnable(EActionType.Tsumo) || OwnerPlayer.Action.State == EActionState.Select_Sutehai )
            btn_Agari.SetTag( ResManager.getString("button_tsumo") );

        btn_Nagashi.SetEnable( isMenuEnable(EActionType.Nagashi) );

        btn_RyuuKyoku.SetEnable( isMenuEnable(EActionType.RyuuKyoku) );
        btn_RyuuKyoku.gameObject.SetActive( isMenuEnable(EActionType.RyuuKyoku) );
    }

    void DisableButtonsExcept( EActionType type )
    {
        btn_Pon.SetEnable( type == EActionType.Pon );
        btn_Chii.SetEnable( type == EActionType.Chii );
        btn_Kan.SetEnable( type == EActionType.Kan );
        btn_Reach.SetEnable( type == EActionType.Reach );
        btn_Agari.SetEnable( type == EActionType.Ron || type == EActionType.Tsumo );
        btn_Nagashi.SetEnable( type == EActionType.Nagashi );

        btn_RyuuKyoku.SetEnable( type == EActionType.RyuuKyoku );
        btn_RyuuKyoku.gameObject.SetActive( type == EActionType.RyuuKyoku );
    }

    public void OnClick_Pon()
    {
        if( isMenuEnable(EActionType.Pon) ){
            //Debug.Log("+ OnClick_Pon()");

            PlayerAction.Response = EResponse.Pon;

            NotifyHide();
            OwnerPlayer.OnPlayerInputFinished();
        }
    }

    public void OnClick_Chii()
    {
        if( isMenuEnable(EActionType.Chii) ){
            //Debug.Log("+ OnClick_Chii()");

            if( PlayerAction.AllSarashiHais.Count > 2 )
            {
                if(PlayerAction.State == EActionState.Select_Chii) // cancel reach.
                {
                    PlayerAction.State = EActionState.None; // set state to Select_SuteHai

                    playerUI.Tehai.EnableInput( true );

                    btn_Chii.SetTag( ResManager.getString("button_chii") );

                    // refresh other menu buttons
                    RefreshMenuButtons();
                }
                else{
                    PlayerAction.State = EActionState.Select_Chii;

                    // list chii hai selection.
                    List<int> enableIndexList = new List<int>();
                    Hai[] jyunTehais = OwnerPlayer.Tehai.getJyunTehai();

                    for(int i = 0; i < PlayerAction.AllSarashiHais.Count; i++)
                    {
                        for( int j = 0; j < jyunTehais.Length; j++){
                            if( jyunTehais[j].ID == PlayerAction.AllSarashiHais[i].ID )
                                enableIndexList.Add( j );
                        }
                    }

                    playerUI.Tehai.EnableInput( enableIndexList );

                    btn_Chii.SetTag( ResManager.getString("button_cancel") );

                    // disable other menu buttons.
                    DisableButtonsExcept(EActionType.Chii);
                }
            }
            else
            {
                // check Chii type.
                if( PlayerAction.IsValidChiiLeft ){
                    PlayerAction.Response = EResponse.Chii_Left;
                }
                else if( PlayerAction.IsValidChiiCenter ){
                    PlayerAction.Response = EResponse.Chii_Center;
                }
                else{
                    PlayerAction.Response = EResponse.Chii_Right;
                }

                PlayerAction.ChiiSelectType = 0;

                NotifyHide();
                OwnerPlayer.OnPlayerInputFinished();
            }
        }
    }

    public void OnClick_Kan()
    {
        if( isMenuEnable(EActionType.Kan) ){
            //Debug.Log("+ OnClick_Kan()");

            if( PlayerAction.IsValidTsumoKan )
            {
                if( PlayerAction.TsumoKanHaiList.Count > 1 )
                {
                    if(PlayerAction.State == EActionState.Select_Kan) // cancel reach.
                    {
                        PlayerAction.State = EActionState.Select_Sutehai; // set state to Select_SuteHai

                        playerUI.Tehai.EnableInput( true );

                        btn_Kan.SetTag( ResManager.getString("button_kan") );

                        // refresh other menu buttons
                        RefreshMenuButtons();
                    }
                    else{
                        PlayerAction.State = EActionState.Select_Kan;

                        // list kan hai selection.
                        List<int> enableIndexList = new List<int>();
                        Hai[] jyunTehais = OwnerPlayer.Tehai.getJyunTehai();

                        for(int i = 0; i < PlayerAction.TsumoKanHaiList.Count; i++)
                        {
                            for( int j = 0; j < jyunTehais.Length; j++){
                                if( jyunTehais[j].ID == PlayerAction.TsumoKanHaiList[i].ID )
                                    enableIndexList.Add( j );
                            }
                        }

                        Hai tsumoHai = GameAgent.Instance.getTsumoHai();
                        for(int i = 0; i < PlayerAction.TsumoKanHaiList.Count; i++)
                        {
                            if( tsumoHai.ID == PlayerAction.TsumoKanHaiList[i].ID )
                                enableIndexList.Add( OwnerPlayer.Tehai.getJyunTehaiCount() );
                        }

                        playerUI.Tehai.EnableInput( enableIndexList );

                        btn_Kan.SetTag( ResManager.getString("button_cancel") );

                        // disable other menu buttons.
                        DisableButtonsExcept(EActionType.Kan);
                    }
                }
                else{
                    Hai kanHai = PlayerAction.TsumoKanHaiList[0];
                    OwnerPlayer.Action.KanSelectIndex = 0;

                    if( OwnerPlayer.Tehai.validKaKan(kanHai) )
                        PlayerAction.Response = EResponse.Kakan;
                    else
                        PlayerAction.Response = EResponse.Ankan;

                    NotifyHide();
                    OwnerPlayer.OnPlayerInputFinished();
                }
            }
            else{
                PlayerAction.Response = EResponse.DaiMinKan;

                NotifyHide();
                OwnerPlayer.OnPlayerInputFinished();
            }
        }
    }

    public void OnClick_Reach()
    {
        if( isMenuEnable(EActionType.Reach) ){
            //Debug.Log("+ OnClick_Reach()");

            if(PlayerAction.State == EActionState.Select_Reach) // cancel reach.
            {
                PlayerAction.State = EActionState.Select_Sutehai; // set state to Select_SuteHai

                playerUI.Tehai.EnableInput( true );

                btn_Reach.SetTag( ResManager.getString("button_reach") );

                // refresh other menu buttons
                RefreshMenuButtons();
            }
            else{
                PlayerAction.State = EActionState.Select_Reach;

                // list reach hai selection
                playerUI.Tehai.EnableInput( PlayerAction.ReachHaiIndexList );

                btn_Reach.SetTag( ResManager.getString("button_cancel") );

                // disable other menu buttons.
                DisableButtonsExcept(EActionType.Reach);
            }
        }
    }

    public void OnClick_Agari()
    {
        if( isMenuEnable(EActionType.Ron) || isMenuEnable(EActionType.Tsumo) ){
            //Debug.Log("+ OnClick_Agari()");

            if(PlayerAction.IsValidTsumo)
                PlayerAction.Response = EResponse.Tsumo_Agari;
            else
                PlayerAction.Response = EResponse.Ron_Agari;

            NotifyHide();
            OwnerPlayer.OnPlayerInputFinished();
        }
    }

    public void Onclick_Nagashi()
    {
        if( isMenuEnable(EActionType.Nagashi) || isMenuEnable(EActionType.RyuuKyoku) ){
            //Debug.Log("+ Onclick_Nagashi()");

            if( PlayerAction.State == EActionState.Select_Kan ) // enable Ankan after Reach.
            {
                PlayerAction.Response = EResponse.SuteHai;
                PlayerAction.SutehaiIndex = OwnerPlayer.Tehai.getJyunTehaiCount();
            }
            else{
                PlayerAction.Response = EResponse.Nagashi;
            }

            NotifyHide();
            OwnerPlayer.OnPlayerInputFinished();
        }
    }

}
