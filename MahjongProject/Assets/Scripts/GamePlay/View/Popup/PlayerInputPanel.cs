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

    private PlayerUI playerUI;



    void Start()
    {
        Init();

        btn_Pon.index = 0;
        btn_Pon.SetOnClick( OnClick_Pon );
        btn_Chii.index = 1;
        btn_Chii.SetOnClick( OnClick_Chii );
        btn_Kan.index = 2;
        btn_Kan.SetOnClick( OnClick_Kan );
        btn_Reach.index = 3;
        btn_Reach.SetOnClick( OnClick_Reach );
        btn_Agari.index = 4;
        btn_Agari.SetOnClick( OnClick_Agari );
        btn_Nagashi.index = 5;
        btn_Nagashi.SetOnClick( Onclick_Nagashi );
    }

    public void SetOwnerPlayerUI(PlayerUI ui)
    {
        this.playerUI = ui;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        btn_Pon.SetEnable( isMenuEnable(EActionType.Pon) );
        btn_Chii.SetEnable( isMenuEnable(EActionType.Chii) );
        btn_Kan.SetEnable( isMenuEnable(EActionType.Kan) );
        btn_Reach.SetEnable( isMenuEnable(EActionType.Reach) );
        btn_Reach.ResetTag();
        btn_Agari.SetEnable( isMenuEnable(EActionType.Agari) );
        btn_Nagashi.SetEnable( isMenuEnable(EActionType.Nagashi) );
    }
    public void HideMenu()
    {
        gameObject.SetActive(false);
    }


    public bool isMenuEnable( EActionType menuItem )
    {
        return PlayerAction.MenuList.Contains(menuItem);
    }


    public void OnClick_Chii()
    {
        if( isMenuEnable(EActionType.Chii) ){
            Debug.Log("+ OnClick_Chii()");

            if( PlayerAction.AllSarashiHais.Count > 0 )
            {
                if( PlayerAction.AllSarashiHais.Count > 1 )
                {
                    // list chii hai selection.
                    List<int> enableIndexList = new List<int>();
                    for(int i = 0; i < PlayerAction.AllSarashiHais.Count; i++)
                    {
                        int index = OwnerPlayer.Tehai.getHaiIndex( PlayerAction.AllSarashiHais[i].ID );
                        enableIndexList.Add( index );
                    }

                    playerUI.Tehai.EnableInput( enableIndexList );
                    btn_Chii.SetEnable(false);
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

                    OwnerPlayer.OnPlayerInputFinished();
                    HideMenu();
                }
            }
            else{
                Debug.LogError("Error!!!");
            }
        }
    }

    public void OnClick_Pon()
    {
        if( isMenuEnable(EActionType.Pon) ){
            Debug.Log("+ OnClick_Pon()");

            PlayerAction.Response = EResponse.Pon;
            OwnerPlayer.OnPlayerInputFinished();

            HideMenu();
        }
    }

    public void OnClick_Kan()
    {
        if( isMenuEnable(EActionType.Kan) ){
            Debug.Log("+ OnClick_Kan()");

            if( PlayerAction.IsValidTsumoKan )
            {
                if( PlayerAction.TsumoKanHaiList.Count > 1 ){
                    PlayerAction.State = EActionState.Select_Kan;

                    // list kan hai selection.
                    List<int> haiIndexList = new List<int>();
                    for(int i = 0; i < PlayerAction.TsumoKanHaiList.Count; i++)
                    {
                        int index = OwnerPlayer.Tehai.getHaiIndex( PlayerAction.TsumoKanHaiList[i].ID );
                        haiIndexList.Add( index );
                    }

                    playerUI.Tehai.EnableInput( haiIndexList );
                    btn_Kan.SetEnable(false);
                }
                else{
                    Hai kanHai = PlayerAction.TsumoKanHaiList[0];
                    if( OwnerPlayer.Tehai.validKaKan(kanHai) )
                    {
                        PlayerAction.Response = EResponse.Kakan;

                        OwnerPlayer.OnPlayerInputFinished();
                        HideMenu();
                    }
                    else
                    {
                        PlayerAction.Response = EResponse.Ankan;

                        OwnerPlayer.OnPlayerInputFinished();
                        HideMenu();
                    }
                }
            }
            else{
                PlayerAction.Response = EResponse.DaiMinKan;

                OwnerPlayer.OnPlayerInputFinished();
                HideMenu();
            }
        }
    }

    public void OnClick_Reach()
    {
        if( isMenuEnable(EActionType.Reach) ){
            Debug.Log("+ OnClick_Reach()");

            if(PlayerAction.State == EActionState.Select_Reach) // cancel reach.
            {
                PlayerAction.State = EActionState.Select_Sutehai; // set state to Select_SuteHai

                playerUI.Tehai.EnableInput();

                btn_Reach.ResetTag();
            }
            else{
                PlayerAction.State = EActionState.Select_Reach;

                // list reach hai selection
                playerUI.Tehai.EnableInput( PlayerAction.ReachHaiIndexList );

                btn_Reach.SetTag("Cancel");
            }
        }
    }

    public void OnClick_Agari()
    {
        if( isMenuEnable(EActionType.Agari) ){
            Debug.Log("+ OnClick_Agari()");

            if(PlayerAction.IsValidTsumo)
                PlayerAction.Response = EResponse.Tsumo_Agari;
            else
                PlayerAction.Response = EResponse.Ron_Agari;
            
            OwnerPlayer.OnPlayerInputFinished();
            HideMenu();
        }
    }

    public void Onclick_Nagashi()
    {
        if( isMenuEnable(EActionType.Nagashi) ){
            Debug.Log("+ Onclick_Nagashi()");

            PlayerAction.Response = EResponse.Nagashi;

            OwnerPlayer.OnPlayerInputFinished();
            HideMenu();
        }
    }

}
