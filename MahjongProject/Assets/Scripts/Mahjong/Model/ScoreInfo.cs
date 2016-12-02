using UnityEngine;
using System.Collections;


public class ScoreInfo
{
    // おや:亲，
    public int oyaRon;
    public int oyaTsumo;
    // こ:子
    public int koRon;
    public int koTsumo;

    public ScoreInfo(ScoreInfo score) {
        this.oyaRon = score.oyaRon;
        this.oyaTsumo = score.oyaTsumo;
        this.koRon = score.koRon;
        this.koTsumo = score.koTsumo;
    }

    public ScoreInfo(int oyaRon, int oyaTsumo, int koRon, int koTsumo) {
        this.oyaRon = oyaRon;
        this.oyaTsumo = oyaTsumo;
        this.koRon = koRon;
        this.koTsumo = koTsumo;
    }
}
