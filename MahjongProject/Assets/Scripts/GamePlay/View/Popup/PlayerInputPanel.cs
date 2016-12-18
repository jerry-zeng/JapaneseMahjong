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


    public void Show()
    {
        gameObject.SetActive(true);

        btn_Pon.SetEnable( isMenuEnable(EActionType.Pon) );
        btn_Chii.SetEnable( isMenuEnable(EActionType.Chii) );
        btn_Kan.SetEnable( isMenuEnable(EActionType.Kan) );
        btn_Reach.SetEnable( isMenuEnable(EActionType.Reach) );
        btn_Agari.SetEnable( isMenuEnable(EActionType.Agari) );
        btn_Nagashi.SetEnable( isMenuEnable(EActionType.Nagashi) );
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public bool isMenuEnable( EActionType menuItem )
    {
        return OwnerPlayer.Action.MenuList.Contains(menuItem);
    }

    public void OnClick_Chii()
    {
        if( isMenuEnable(EActionType.Chii) ){
            Debug.Log("+ OnClick_Chii()");

            // TODO: check Chii_Left or Chii_Center or Chii_Right.
            OwnerPlayer.Action.Response = EResponse.Chii_Left;
            OwnerPlayer.OnPlayerInputFinished();

            Hide();
        }
    }

    public void OnClick_Pon()
    {
        if( isMenuEnable(EActionType.Pon) ){
            Debug.Log("+ OnClick_Pon()");

            OwnerPlayer.Action.Response = EResponse.Pon;
            OwnerPlayer.OnPlayerInputFinished();

            Hide();
        }
    }

    public void OnClick_Kan()
    {
        if( isMenuEnable(EActionType.Kan) ){
            Debug.Log("+ OnClick_Kan()");

            if(OwnerPlayer.CurrentRequest == ERequest.Handle_TsumoHai)
            {
                // TODO: check Ankan or Kakan.
                OwnerPlayer.Action.Response = EResponse.Ankan;
            }
            else{
                OwnerPlayer.Action.Response = EResponse.DaiMinKan;
            }
            OwnerPlayer.OnPlayerInputFinished();

            Hide();
        }
    }

    public void OnClick_Reach()
    {
        if( isMenuEnable(EActionType.Reach) ){
            Debug.Log("+ OnClick_Reach()");

            OwnerPlayer.Action.Response = EResponse.Reach;
            OwnerPlayer.OnPlayerInputFinished();

            Hide();
        }
    }

    public void OnClick_Agari()
    {
        if( isMenuEnable(EActionType.Agari) ){
            Debug.Log("+ OnClick_Agari()");

            if(OwnerPlayer.CurrentRequest == ERequest.Handle_TsumoHai)
                OwnerPlayer.Action.Response = EResponse.Tsumo_Agari;
            else
                OwnerPlayer.Action.Response = EResponse.Ron_Agari;
            OwnerPlayer.OnPlayerInputFinished();

            Hide();
        }
    }

    public void Onclick_Nagashi()
    {
        if( isMenuEnable(EActionType.Nagashi) ){
            Debug.Log("+ Onclick_Nagashi()");

            OwnerPlayer.Action.Response = EResponse.Nagashi;
            OwnerPlayer.OnPlayerInputFinished();

            Hide();
        }
    }

}
