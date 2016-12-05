using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FuuroUI : UIObject 
{

    //public Vector2 AlignRightLocalPos = new Vector2(0, 0);
    private int FuuroOffsetX = 6;

    // max length = 16.
    private List<MahjongPai> fuuroHais = new List<MahjongPai>( Tehai.FUURO_MAX * Tehai.MENTSU_LENGTH_4 );

    private float curMaxPosX = 0;


    void Start () {

    }


    public void UpdateFuuro( Fuuro[] fuuros )
    {
        if( fuuros == null  ){
            return;
        }

        // clear all fuuro.
        Clear();

        // create new.
        for( int i = 0; i < fuuros.Length; i++ ) 
        {
            Fuuro fuu = fuuros[i];
            EFuuroType fuuroType = fuu.Type;

            if( i > 0 )
                curMaxPosX -= FuuroOffsetX;

            int newPickIndex = fuu.NewPickIndex;
            Hai[] hais = fuu.Hais;
            //int relation = fuu.Relation;

            bool shouldSetLand = false;

            switch(fuuroType)
            {
                case EFuuroType.MinShun: //Chii.
                case EFuuroType.MinKou:  // Pon.
                case EFuuroType.KaKan:   // 加杠.
                case EFuuroType.DaiMinKan: // 大明杠.
                {
                    for( int j = 0; j < hais.Length; j++ ) 
                    {
                        if( hais[j].ID < 0 )
                            continue;

                        shouldSetLand = (j == newPickIndex);

                        float posX = curMaxPosX - PlayerUI.GetMahjongRange(shouldSetLand) * 0.5f;
                        Vector3 localPos = new Vector3(posX, 0, 0);

                        MahjongPai pai = PlayerUI.CreateMahjongPai(transform, localPos, hais[j], true);

                        if( shouldSetLand ) {
                            pai.SetOrientation(EOrientation.Landscape_Left);

                            pai.transform.localPosition += new Vector3(0, MahjongPai.LandHaiPosOffsetY, 0);
                        }
                        fuuroHais.Add(pai);

                        // update curMaxPosX.
                        curMaxPosX -= PlayerUI.GetMahjongRange(shouldSetLand);
                    }
                }
                break;

                case EFuuroType.AnKan: // 暗杠.
                {
                    for( int j = 0; j < hais.Length; j++ ) 
                    {
                        if( hais[j].ID < 0 )
                            continue;

                        shouldSetLand = false;

                        float posX = curMaxPosX - PlayerUI.GetMahjongRange(shouldSetLand) * 0.5f;
                        Vector3 localPos = new Vector3(posX, 0, 0);

                        bool isShow = (j != 0 && j != hais.Length - 1); // 2 sides hide.

                        MahjongPai pai = PlayerUI.CreateMahjongPai(transform, localPos, hais[j], isShow);

                        fuuroHais.Add(pai);

                        // update curMaxPosX.
                        curMaxPosX -= PlayerUI.GetMahjongRange(shouldSetLand);
                    }
                }
                break;
            }
        } // end for().

    }

    public override void Clear() {
        base.Clear();

        // clear all pai.
        for( int i = 0; i < fuuroHais.Count; i++ ) {
            MahjongPai pai = fuuroHais[i];
            ResManager.collectMahjongObject(pai);
        }
        fuuroHais.Clear();

        curMaxPosX = 0;
    }



}