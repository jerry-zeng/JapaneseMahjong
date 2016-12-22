
/// <summary>
/// 山を管理する, 所有堆起来的牌叫山牌
/// 最后的7幢牌，共14张，叫王牌
/// </summary>

public class Yama 
{
    // 山牌の配列の最大数
    public readonly static int YAMA_HAIS_MAX = 136;

    // 自摸牌の配列の最大数
    public readonly static int TSUMO_HAIS_MAX = 122;

    // 杠上开花牌の配列の最大数
    public readonly static int RINSHAN_HAIS_MAX = 4;

    // 表と裏ドラの配列の最大数
    public readonly static int DORA_HAIS_MAX = 5; //x2


    // 山牌の配列
    private Hai[] _yamaHais;


    // ツモ牌の配列
    private Hai[] _tsumoHais;

    // リンシャン(岭上开花)牌の配列
    private Hai[] _rinshanHais;

    // ツモ牌のインデックス(index)
    private int _tsumoHaisIndex = 0;

    // used to calculate last tsumo hai index.
    //private int _tsumoHaiStartIndex;

    // リンシャン牌の位置
    private int _rinshanHaisIndex = 0;


    // 表ドラ牌の配列
    private Hai[] _omoteDoraHais;

    // 裏ドラ牌の配列
    private Hai[] _uraDoraHais;


    private int[] TsumoHaiIndex_InYama = new int[TSUMO_HAIS_MAX];
    private int[] RinshanHaiIndex_InYama = new int[RINSHAN_HAIS_MAX];
    private int[] OmoteDoraHaiIndex_InYama = new int[DORA_HAIS_MAX];
    private int[] UraDoraHaiIndex_InYama = new int[DORA_HAIS_MAX];


    public Yama()
    {
        _yamaHais = new Hai[YAMA_HAIS_MAX];

        for(int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++)
        {
            for( int n = 0; n < 4; n++ ){
                _yamaHais[(id * 4) + n] = new Hai(id);
            }
        }

        _tsumoHais = new Hai[TSUMO_HAIS_MAX];
        _rinshanHais = new Hai[RINSHAN_HAIS_MAX];
        _omoteDoraHais = new Hai[DORA_HAIS_MAX];
        _uraDoraHais = new Hai[DORA_HAIS_MAX];

        setTsumoHaisStartIndex(0);
    }

    // temply implement.
    public Hai[] getYamaHais()
    {
        return _yamaHais;
    }

    // ツモ牌の残り数を取得する
    public int getTsumoNokori()
    {
        return TSUMO_HAIS_MAX - _rinshanHaisIndex - _tsumoHaisIndex;
    }

    // リンシャン(岭上开花)牌の残り数を取得する
    public int getRinshanNokori()
    {
        return RINSHAN_HAIS_MAX - _rinshanHaisIndex;
    }

    public int getTsumoHaiIndex()
    {
        return _tsumoHaisIndex;
    }

    public int getPreTsumoHaiIndex()
    {
        /*
        return (_tsumoHaisIndex-1 + this._tsumoHaiStartIndex) % YAMA_HAIS_MAX;
        */
        return TsumoHaiIndex_InYama[_tsumoHaisIndex-1];
    }

    public int getPreRinshanHaiIndex()
    {
        /*
        int index = _tsumoHaiStartIndex - (_rinshanHaisIndex + 1);
        if(index < 0) 
            index += YAMA_HAIS_MAX;
        return index % YAMA_HAIS_MAX;
        */
        if(_rinshanHaisIndex == 0) return -1;

        return RinshanHaiIndex_InYama[_rinshanHaisIndex-1];
    }

    public int getLastOmoteHaiIndex()
    {
        /*
        int index = _tsumoHaiStartIndex - ( 6 + _rinshanHaisIndex*2); //6=4+2, 4 is RINSHAN_HAIS_MAX, 2 is the first omote and ura dora hai.
        if(index < 0) 
            index += YAMA_HAIS_MAX;
        return index % YAMA_HAIS_MAX;
        */
        return OmoteDoraHaiIndex_InYama[_rinshanHaisIndex];
    }

    // 洗牌する
    public void Shuffle()
    {
        Hai temp;
        int j;

        for (int i = 0; i < YAMA_HAIS_MAX; i++) 
        {
            // get a random index.
            j = Utils.GetRandomNum(0, YAMA_HAIS_MAX);

            // exchange hais.
            temp = _yamaHais[i];
            _yamaHais[i] = _yamaHais[j];
            _yamaHais[j] = temp;
        }
    }

    /// <summary>
    /// 初始拿牌.
    /// </summary>
    public Hai[] PickHaipai()
    {
        Hai[] hais = new Hai[4];
        for( int i = 0; i < hais.Length; i++ ) 
        {
            hais[i] = new Hai( _tsumoHais[_tsumoHaisIndex] );

            _tsumoHaisIndex++;
        }

        return hais;
    }


    // ツモ牌を取得する
    public Hai PickTsumoHai()
    {
        if( getTsumoNokori() <= 0)
            return null;

        Hai tsumoHai = new Hai(_tsumoHais[_tsumoHaisIndex]);
        _tsumoHaisIndex++;

        return tsumoHai;
    }

    // リンシャン(岭上开花)牌を取得する
    public Hai PickRinshanTsumoHai()
    {
        if( getRinshanNokori() <= 0 )
            return null;

        Hai rinshanHai = new Hai(_rinshanHais[_rinshanHaisIndex]);
        _rinshanHaisIndex++;

        return rinshanHai;
    }

    // 表ドラの配列を取得する
    public Hai[] getOmoteDoraHais()
    {
        int omoteDoraHaisCount = _rinshanHaisIndex + 1;
        Hai[] omoteDoraHais = new Hai[omoteDoraHaisCount];

        for( int i = 0; i < omoteDoraHais.Length; i++ )
            omoteDoraHais[i] = new Hai(this._omoteDoraHais[i]);

        return omoteDoraHais;
    }
    public Hai[] getAllOmoteDoraHais()
    {
        Hai[] allHais = new Hai[_omoteDoraHais.Length];
        System.Array.Copy( _omoteDoraHais, allHais, allHais.Length );
        return allHais;
    }

    // 裏ドラの配列を取得する
    public Hai[] getUraDoraHais()
    {
        int uraDoraHaisCount = _rinshanHaisIndex + 1;
        Hai[] uraDoraHais = new Hai[uraDoraHaisCount];

        for( int i = 0; i < uraDoraHais.Length; i++ )
            uraDoraHais[i] = new Hai(this._uraDoraHais[i]);

        return uraDoraHais;
    }
    public Hai[] getAllUraDoraHais()
    {
        Hai[] allHais = new Hai[_uraDoraHais.Length];
        System.Array.Copy( _uraDoraHais, allHais, allHais.Length );
        return allHais;
    }

    public Hai[] getAllDoraHais()
    {
        int omoteDoraHaisLength = _rinshanHaisIndex + 1;
        int uraDoraHaisLength = _rinshanHaisIndex + 1;
        int allDoraHaisLength = omoteDoraHaisLength + uraDoraHaisLength;

        Hai[] allDoraHais = new Hai[allDoraHaisLength];

        for(int i = 0; i < omoteDoraHaisLength; i++)
            allDoraHais[i] = new Hai(this._omoteDoraHais[i]);

        for(int i = 0; i < uraDoraHaisLength; i++)
            allDoraHais[omoteDoraHaisLength + i] = new Hai(this._uraDoraHais[i]);

        return allDoraHais;
    }

    /**
     * ツモ牌の開始位置を設定する。
     *
     *  the correct array is like:
     *  
     *  Tsumo Start <--| Wareme |<-- Rinshan 2x2 <-- Doras 2x5 |<-- Tsumo End <--.
     */

    public bool setTsumoHaisStartIndex(int tsumoHaiStartIndex)
    {
        if (tsumoHaiStartIndex >= YAMA_HAIS_MAX)
            return false;

        //this._tsumoHaiStartIndex = tsumoHaiStartIndex;


        int yamaHaisIndex = tsumoHaiStartIndex;

        // tsumo hais. 122.
        for(int i = 0; i < TSUMO_HAIS_MAX; i++)
        {
            _tsumoHais[i] = _yamaHais[yamaHaisIndex];

            TsumoHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;

            if(yamaHaisIndex >= YAMA_HAIS_MAX)
                yamaHaisIndex = 0;
        }
        _tsumoHaisIndex = 0;


        // dora hais. 1+4=5. 
        for( int i = DORA_HAIS_MAX - 1; i >= 0; i-- ) // reverse.
        {
            // 表dora.
            _omoteDoraHais[i] = _yamaHais[yamaHaisIndex];
            OmoteDoraHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;

            // 里dora.
            _uraDoraHais[i] = _yamaHais[yamaHaisIndex];
            UraDoraHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;
        }

        // rinshan hais. 4.
        for( int i = 0; i < RINSHAN_HAIS_MAX; i++ )
        {
            _rinshanHais[i] = _yamaHais[yamaHaisIndex];
            RinshanHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;
        }

        /// reverse.
        ///   2 0  ->  0 2
        ///   3 1      1 3
        Hai temp = _rinshanHais[0];
        _rinshanHais[0] = _rinshanHais[2];
        _rinshanHais[2] = temp;

        temp = _rinshanHais[1];
        _rinshanHais[1] = _rinshanHais[3];
        _rinshanHais[3] = temp;

        //index
        int tempIndex = RinshanHaiIndex_InYama[0];
        RinshanHaiIndex_InYama[0] = RinshanHaiIndex_InYama[2];
        RinshanHaiIndex_InYama[2] = tempIndex;

        tempIndex = RinshanHaiIndex_InYama[1];
        RinshanHaiIndex_InYama[1] = RinshanHaiIndex_InYama[3];
        RinshanHaiIndex_InYama[3] = tempIndex;

        _rinshanHaisIndex = 0;

        return true;
    }

    // 赤ドラ牌
    public void setRedDora(int id, int num)
    {
        if(num <= 0) return;

        for(int i = 0; i < _yamaHais.Length; i++) 
        {
            if( _yamaHais[i].ID == id )
            {
                _yamaHais[i].IsRed = true;

                num--;
                if(num <= 0) break;
            }
        } 
    }

}