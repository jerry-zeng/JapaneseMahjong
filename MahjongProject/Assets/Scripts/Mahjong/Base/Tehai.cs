
/// <summary>
/// 手牌を管理する。
/// 手牌:拿到手上的牌
/// </summary>

public class Tehai 
{
    // 純手牌の長さの最大値
    public readonly static int JYUN_TEHAI_LENGTH_MAX = 14;

    // 副露の最大値
    public readonly static int FUURO_MAX = 4;

    // 面子の長さ(3)
    public readonly static int MENTSU_LENGTH_3 = 3;

    // 面子の長さ(4)
    public readonly static int MENTSU_LENGTH_4 = 4;


    // 純手牌
    private Hai[] _jyunTehai = new Hai[JYUN_TEHAI_LENGTH_MAX];

    // 純手牌の長さ
    private int _jyunTehaiLength;

    // 副露の配列
    private Fuuro[] _fuuros = new Fuuro[FUURO_MAX];

    // 副露の個数
    private int _fuuroNums;


    public Tehai() 
    {
        initialize();

        for( int i = 0; i < JYUN_TEHAI_LENGTH_MAX; i++ ) {
            _jyunTehai[i] = new Hai();
        }

        for( int i = 0; i < FUURO_MAX; i++ ) {
            _fuuros[i] = new Fuuro();
        }
    }

    public void initialize()
    {
        _jyunTehaiLength = 0;
        _fuuroNums = 0;
    }

    // 手牌をコピーする
    public static void copy(Tehai dest, Tehai src, bool jyunTehaiCopy)
    {
        if (jyunTehaiCopy == true) {
            dest._jyunTehaiLength = src._jyunTehaiLength;
            Tehai.copyJyunTehai(dest._jyunTehai, src._jyunTehai, dest._jyunTehaiLength);
        }

        dest._fuuroNums = src._fuuroNums;
        copyFuuros(dest._fuuros, src._fuuros, dest._fuuroNums);
    }

    // 純手牌を取得する
    public Hai[] getJyunTehai()
    {
        return _jyunTehai;
    }

    // 純手牌の長さを取得する
    public int getJyunTehaiLength()
    {
        return _jyunTehaiLength;
    }

    // 純手牌に牌を追加する
    public bool addJyunTehai(Hai hai)
    {
        if (_jyunTehaiLength >= JYUN_TEHAI_LENGTH_MAX)
            return false;

        int i;
        for(i = _jyunTehaiLength; i > 0; i--)
        {
            if (_jyunTehai[i-1].ID <= hai.ID)
                break;

            Hai.copy(_jyunTehai[i], _jyunTehai[i-1]);
        }

        Hai.copy(_jyunTehai[i], hai);
        _jyunTehaiLength ++;

        return true;
    }

    // 純手牌から指定位置の牌を削除する
    public bool removeJyunTehai(int index)
    {
        if (index >= JYUN_TEHAI_LENGTH_MAX)
            return false;

        for (int i = index; i < _jyunTehaiLength-1; i++)
        {
            Hai.copy(_jyunTehai[i], _jyunTehai[i+1]);
        }

        _jyunTehaiLength --;

        return true;
    }

    // 純手牌から指定の牌を削除する
    public bool removeJyunTehai(Hai hai)
    {
        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if( _jyunTehai[i].ID == hai.ID )
                return removeJyunTehai(i);
        }

        return false;
    }

    // 純手牌をコピーする
    public static bool copyJyunTehai(Hai[] dest, Hai[] src, int length)
    {
        if(length > JYUN_TEHAI_LENGTH_MAX)
            return false;

        for (int i = 0; i < length; i++)
        {
            Hai.copy(dest[i], src[i]);
        }

        return true;
    }

    // 純手牌の指定位置の牌をコピーする
    public bool copyJyunTehaiIndex(Hai hai, int index)
    {
        if (index >= _jyunTehaiLength)
            return false;

        Hai.copy(hai, _jyunTehai[index]);

        return true;
    }

    // チー(左)の可否をチェックする
    public bool validChiiLeft(Hai suteHai, Hai[] sarashiHais)
    {
        if (suteHai.isTsuu())
            return false;

        if (suteHai.getNum() == Hai.NUM_8 || suteHai.getNum() == Hai.NUM_9)
            return false;

        if (_fuuroNums >= FUURO_MAX)
            return false;

        int noKindLeft = suteHai.getNumKind();
        int noKindCenter = noKindLeft + 1;
        int noKindRight = noKindLeft + 2;

        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if (_jyunTehai[i].getNumKind() == noKindCenter) 
            {
                for (int j = i + 1; j < _jyunTehaiLength; j++) 
                {
                    if (_jyunTehai[j].getNumKind() == noKindRight) 
                    {
                        sarashiHais[0] = new Hai(_jyunTehai[i]);
                        sarashiHais[1] = new Hai(_jyunTehai[j]);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(左)を設定する
    public bool setChiiLeft(Hai suteHai, int relation)
    {
        Hai[] sarashiHais = new Hai[2];

        if (!validChiiLeft(suteHai, sarashiHais))
            return false;

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        hais[0] = new Hai(suteHai);
        int newPickIndex = 0;

        int noKindLeft = suteHai.getNumKind();
        int noKindCenter = noKindLeft + 1;
        int noKindRight = noKindLeft + 2;

        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if (_jyunTehai[i].getNumKind() == noKindCenter)
            {
                hais[1] = new Hai(_jyunTehai[i]);

                removeJyunTehai(i);

                for (int j = i; j < _jyunTehaiLength; j++)
                {
                    if (_jyunTehai[j].getNumKind() == noKindRight)
                    {
                        hais[2] = new Hai(_jyunTehai[j]);

                        removeJyunTehai(j);

                        hais[3] = new Hai();

                        _fuuros[_fuuroNums].Update(EFuuroType.MinShun, hais, relation, newPickIndex);
                        _fuuroNums++;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(中央)の可否をチェックする
    public bool validChiiCenter(Hai suteHai, Hai[] sarashiHais)
    {
        if (suteHai.isTsuu())
            return false;

        if (suteHai.getNum() == Hai.NUM_1 || suteHai.getNum() == Hai.NUM_9)
            return false;

        if (_fuuroNums >= FUURO_MAX)
            return false;

        int noKindCenter = suteHai.getNumKind();
        int noKindLeft = noKindCenter - 1;
        int noKindRight = noKindCenter + 1;

        for (int i = 0; i < _jyunTehaiLength; i++) 
        {
            if (_jyunTehai[i].getNumKind() == noKindLeft)
            {
                for (int j = i + 1; j < _jyunTehaiLength; j++) 
                {
                    if (_jyunTehai[j].getNumKind() == noKindRight)
                    {
                        sarashiHais[0] = new Hai(_jyunTehai[i]);
                        sarashiHais[1] = new Hai(_jyunTehai[j]);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(中央)を設定する
    public bool setChiiCenter(Hai suteHai, int relation)
    {
        Hai[] sarashiHais = new Hai[2];

        if (!validChiiCenter(suteHai, sarashiHais))
            return false;

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        hais[1] = new Hai(suteHai);
        int newPickIndex = 1;

        int noKindCenter = suteHai.getNumKind();
        int noKindLeft = noKindCenter - 1;
        int noKindRight = noKindCenter + 1;

        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if (_jyunTehai[i].getNumKind() == noKindLeft)
            {
                hais[0] = new Hai(_jyunTehai[i]);

                removeJyunTehai(i);

                for (int j = i; j < _jyunTehaiLength; j++)
                {
                    if (_jyunTehai[j].getNumKind() == noKindRight)
                    {
                        hais[2] = new Hai(_jyunTehai[j]);

                        removeJyunTehai(j);

                        hais[3] = new Hai();

                        _fuuros[_fuuroNums].Update(EFuuroType.MinShun, hais, relation, newPickIndex);
                        _fuuroNums++;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(右)の可否をチェックする
    public bool validChiiRight(Hai suteHai, Hai[] sarashiHais)
    {
        if (suteHai.isTsuu())
            return false;

        if (suteHai.getNum() == Hai.NUM_1 || suteHai.getNum() == Hai.NUM_2)
            return false;

        if (_fuuroNums >= FUURO_MAX)
            return false;

        int noKindRight = suteHai.getNumKind();
        int noKindLeft = noKindRight - 2;
        int noKindCenter = noKindRight - 1;

        for (int i = 0; i < _jyunTehaiLength; i++) 
        {
            if (_jyunTehai[i].getNumKind() == noKindLeft)
            {
                for (int j = i + 1; j < _jyunTehaiLength; j++) 
                {
                    if (_jyunTehai[j].getNumKind() == noKindCenter)
                    {
                        sarashiHais[0] = new Hai(_jyunTehai[i]);
                        sarashiHais[1] = new Hai(_jyunTehai[j]);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(右)を設定する
    public bool setChiiRight(Hai suteHai, int relation)
    {
        Hai[] sarashiHais = new Hai[2];

        if (!validChiiRight(suteHai, sarashiHais))
            return false;

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        hais[2] = new Hai(suteHai);
        int newPickIndex = 2;

        int noKindRight = suteHai.getNumKind();
        int noKindLeft = noKindRight - 2;
        int noKindCenter = noKindRight - 1;

        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if (_jyunTehai[i].getNumKind() == noKindLeft)
            {
                hais[0] = new Hai(_jyunTehai[i]);

                removeJyunTehai(i);

                for (int j = i; j < _jyunTehaiLength; j++)
                {
                    if (_jyunTehai[j].getNumKind() == noKindCenter)
                    {
                        hais[1] = new Hai(_jyunTehai[j]);

                        removeJyunTehai(j);

                        hais[3] = new Hai();

                        _fuuros[_fuuroNums].Update(EFuuroType.MinShun, hais, relation, newPickIndex);
                        _fuuroNums++;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // ポン(碰)の可否をチェックする
    public bool validPon(Hai suteHai)
    {
        if (_fuuroNums >= FUURO_MAX)
            return false;

        for (int i = 0, count = 1; i < _jyunTehaiLength; i++)
        {
            if(_jyunTehai[i].ID == suteHai.ID)
            {
                count++;

                if (count >= MENTSU_LENGTH_3)
                    return true;
            }
        }

        return false;
    }

    // ポンを設定する
    public bool setPon(Hai suteHai, int relation)
    {
        if (!validPon(suteHai))
            return false;

        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        int iHais = 0;
        hais[iHais] = new Hai(suteHai);
        int newPickIndex = iHais;

        iHais++;

        for (int i = 0; i < _jyunTehaiLength; i++) 
        {
            if( _jyunTehai[i].ID == suteHai.ID)
            {
                hais[iHais] = new Hai(_jyunTehai[i]);
                iHais++;

                removeJyunTehai(i);
                i--;

                if (iHais >= MENTSU_LENGTH_3)
                    break;
            }
        }

        hais[iHais] = new Hai();

        _fuuros[_fuuroNums].Update(EFuuroType.MinKou, hais, relation, newPickIndex);
        _fuuroNums++;

        return true;
    }

    // 槓の可否をチェックする
    public int validKan(Hai addHai, Hai[] kanHais)
    {
        if (_fuuroNums >= FUURO_MAX)
            return 0;

        int kanCount = 0;

        addJyunTehai(addHai);

        // 加カンのチェック
        for (int i = 0; i < _fuuroNums; i++) 
        {
            if (_fuuros[i].Type == EFuuroType.MinKou)
            {
                for (int j = 0; j < _jyunTehaiLength; j++) 
                {
                    if (_fuuros[i].Hais[0].ID == _jyunTehai[j].ID) 
                    {
                        kanHais[kanCount] = new Hai(_jyunTehai[j]);
                        kanCount++;
                    }
                }
            }
        }

        int id = _jyunTehai[0].ID;
        int count = 1;
        for (int i = 1; i < _jyunTehaiLength; i++) 
        {
            if (id == _jyunTehai[i].ID)
            {
                count++;
                if (count >= MENTSU_LENGTH_4) {
                    kanHais[kanCount] = new Hai(_jyunTehai[i]);
                    kanCount++;
                }
            } 
            else{
                id = _jyunTehai[i].ID;
                count = 1;
            }
        }

        removeJyunTehai(addHai);

        return kanCount;
    }

    public bool validDaiMinKan(Hai suteHai)
    {
        if (_fuuroNums >= FUURO_MAX)
            return false;

        for (int i = 0, count = 1; i < _jyunTehaiLength; i++)
        {
            if(_jyunTehai[i].ID == suteHai.ID)
            {
                count++;
                if (count >= MENTSU_LENGTH_4)
                    return true;
            }
        }

        return false;
    }

    // 大明槓を設定する
    public bool setDaiMinKan(Hai suteHai, int relation)
    {
        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        int iHais = 0;
        hais[iHais] = new Hai(suteHai);

        int newPickIndex = iHais;

        iHais++;

        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if( _jyunTehai[i].ID == suteHai.ID)
            {
                hais[iHais] = new Hai(_jyunTehai[i]);
                iHais++;

                removeJyunTehai(i);
                i--;

                if (iHais >= MENTSU_LENGTH_4)
                    break;
            }
        }

        _fuuros[_fuuroNums].Update(EFuuroType.DaiMinKan, hais, relation, newPickIndex);
        _fuuroNums++;

        return true;
    }

    // 加槓の可否をチェックする
    public bool validKaKan(Hai tsumoHai)
    {
        if (_fuuroNums >= FUURO_MAX)
            return false;

        for (int i = 0; i < _fuuroNums; i++)
        {
            if (_fuuros[i].Type == EFuuroType.MinKou)
            {
                if( _fuuros[i].Hais[0].ID == tsumoHai.ID)
                    return true;
            }
        }

        return false;
    }

    // 加槓を設定する
    public bool setKaKan(Hai tsumoHai)
    {
        if (!validKaKan(tsumoHai))
            return false;

        Hai[] hais = null;

        for (int i = 0; i < _fuuroNums; i++) 
        {
            if (_fuuros[i].Type == EFuuroType.MinKou) 
            {
                hais = _fuuros[i].Hais;

                if(hais[0].ID == tsumoHai.ID)
                {
                    Hai.copy(hais[3], tsumoHai);

                    _fuuros[i].Update(EFuuroType.KaKan, hais, 3);
                }
            }
        }

        return true;
    }

    // 暗槓を設定する
    public bool setAnKan(Hai tsumoHai, int relation)
    {
        Hai[] fuuroHais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        for (int i = 0; i < _fuuroNums; i++) 
        {
            if (_fuuros[i].Type == EFuuroType.MinKou) 
            {
                fuuroHais = _fuuros[i].Hais;

                if( fuuroHais[0].ID == tsumoHai.ID)
                {
                    Hai.copy(fuuroHais[3], tsumoHai);
                    removeJyunTehai(tsumoHai);

                    _fuuros[i].Update(EFuuroType.KaKan, fuuroHais, 3);
                    //m_fuuroNums++;

                    return true;
                }
            }
        }

        int iHais = 0;
        Hai[] hais = new Hai[Mahjong.MENTSU_HAI_MEMBERS_4];

        for (int i = 0; i < _jyunTehaiLength; i++)
        {
            if(_jyunTehai[i].ID == tsumoHai.ID)
            {
                hais[iHais] = new Hai(_jyunTehai[i]);
                iHais++;

                removeJyunTehai(i);
                i--;

                if (iHais >= MENTSU_LENGTH_4)
                    break;
            }
        }

        _fuuros[_fuuroNums].Update(EFuuroType.AnKan, hais, relation, MENTSU_LENGTH_4 - 1);
        _fuuroNums++;

        return true;
    }

    // 副露の配列を取得する
    public Fuuro[] getFuuros()
    {
        return _fuuros;
    }

    // 副露の個数を取得する
    public int getFuuroNum()
    {
        return _fuuroNums;
    }

    // 鸣听Flag
    public bool isNaki()
    {
        for(int i = 0; i < _fuuroNums; i++)
        {
            if(_fuuros[i].Type != EFuuroType.AnKan)
                return true;
        }

        return false;
    }


    /// <summary>
    /// 副露の配列をコピーする
    /// </summary>

    public static bool copyFuuros(Fuuro[] dest, Fuuro[] src, int length)
    {
        if (length > FUURO_MAX)
            return false;

        for (int i = 0; i < length; i++) {
            Fuuro.copy(dest[i], src[i]);
        }

        return true;
    }
}
