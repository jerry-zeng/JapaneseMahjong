using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SelectChiiChaPanel : MonoBehaviour
{
    public List<Vector3> kazePosList = new List<Vector3>();

    private List<MahjongPai> kazePaiList = new List<MahjongPai>();

    private float posY = -40f;
    private float leftPosX = -150f;
    private float offset = 100f;

    private int chiiChaIndex = -1;


    void Start(){
        
    }


    public void Hide()
    {
        for( int i = 0; i < kazePaiList.Count; i++ )
        {
            PlayerUI.CollectMahjongPai( kazePaiList[i] );
        }
        kazePaiList.Clear();

        gameObject.SetActive(false);
    }

    public void Show()
    {
        Hai[] init_hais = new Hai[4]{
            new Hai(Hai.ID_TON), new Hai(Hai.ID_NAN),
            new Hai(Hai.ID_SYA), new Hai(Hai.ID_PE),
        };

        Hai temp;
        for( int i = 0; i < init_hais.Length; i++ )
        {
            int index = Random.Range(0, init_hais.Length);

            temp = init_hais[i];
            init_hais[i] = init_hais[index];
            init_hais[index] = temp;
        }

        gameObject.SetActive(true);

        for( int i = 0; i < init_hais.Length; i++ )
        {
            MahjongPai pai = PlayerUI.CreateMahjongPai(transform, new Vector3(leftPosX + i*offset, posY, 0), init_hais[i], false);
            pai.SetOnClick( OnClickMahjong );
            pai.EnableInput();

            kazePaiList.Add( pai );
        }
    }


    void OnClickMahjong()
    {
        for( int i = 0; i < kazePaiList.Count; i++ )
        {
            kazePaiList[i].DisableInput();
        }

        int paiID = MahjongPai.current.ID;

        chiiChaIndex = ((Hai.ID_TON - paiID) + 4) % 4;
        //Debug.Log("chiiChaIndex: " + chiiChaIndex.ToString());

        StartCoroutine( MoveMahjongPaiToKaze(paiID) );
    }


    IEnumerator MoveMahjongPaiToKaze( int startID )
    {
        for( int i = 0, id = startID; i < kazePaiList.Count; i++, id++ )
        {
            if( id > Hai.ID_PE )
                id = Hai.ID_TON;

            MahjongPai pai = kazePaiList.Find( mp => mp.ID == id );
            pai.Show();

            yield return new WaitForSeconds(0.4f);

            TweenPosition tweener = TweenPosition.Begin( pai.gameObject, 0.3f, kazePosList[i] );
            tweener.style = UITweener.Style.Once;

            if( i == kazePaiList.Count-1 )
                tweener.SetOnFinished( OnMoveEnd );

            yield return new WaitForSeconds(0.2f);
        }
    }

    void OnMoveEnd()
    {
        Invoke("OnEnd", 0.5f);
    }

    void OnEnd()
    {
        Hide();

        EventManager.Get().SendEvent(UIEventType.On_Select_ChiiCha_End, chiiChaIndex);
    }

}
