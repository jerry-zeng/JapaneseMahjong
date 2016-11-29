

public class CountFormat 
{
    #region internal class.
    public class Count 
    {
        public int _numKind = 0;
        public int _num = 0;

        public void initialize()
        {
            _numKind = 0;
            _num = 0;
        }
    }

    public class CombiManager 
    {
        // 上がりの組み合わせの配列の最大値
        public const int COMBI_MAX = 10;

        // 上がりの組み合わせの配列
        public Combi[] _combis;

        // 上がりの組み合わせの配列の有効な個数
        public int _combiNum = 0;

        // カウントの配列の残りの個数
        public int _remain = 0;

        public Combi _current;


        public CombiManager(){
            _current = new Combi();

            _combis = new Combi[COMBI_MAX];
            for (int i = 0; i < _combis.Length; i++) {
                _combis[i] = new Combi();
            }
        }

        public void initialize(int remain)
        {
            this._combiNum = 0;
            this._remain = remain;

            _current._atamaNumKind = 0;
            _current._shunNum = 0;
            _current._kouNum = 0;
        }

        public void add()
        {
            Combi.copy(_combis[_combiNum], _current);
            _combiNum++;
        }
    }
    #endregion internal class.


    // カウント(count)の最大値
    public const int COUNT_MAX = 14 + 2;

    // カウントの配列
    public Count[] _counts;

    // カウントの配列の有効な個数
    public int _countNum;

    // 上がりの組み合わせの配列を管理
    private CombiManager _combiManager;


    public CountFormat(){
        _combiManager = new CombiManager();

        _counts = new Count[COUNT_MAX];
        for (int i = 0; i < COUNT_MAX; i++) {
            _counts[i] = new Count();
        }
    }


    private int getTotalCountLength()
    {
        int totalCountLength = 0;

        for (int i = 0; i < _countNum; i++) {
            totalCountLength += _counts[i]._num;
        }

        return totalCountLength;
    }


    public void setCountFormat(Tehai tehai, Hai addHai)
    {
        for (int i = 0; i < _counts.Length; i++) {
            _counts[i].initialize();
        }
        _countNum = 0;

        int addHaiNumKind = 0;
        bool set = true;

        if (addHai != null) {
            addHaiNumKind = addHai.getNumKind();
            set = false;
        }

        Hai[] jyunTehais = tehai.getJyunTehai();
        int jyunTehaiNumKind;
        int jyunTehaiLength = tehai.getJyunTehaiLength();

        for (int i = 0; i < jyunTehaiLength; )
        {
            jyunTehaiNumKind = jyunTehais[i].getNumKind();

            if(!set && (jyunTehaiNumKind > addHaiNumKind))
            {
                set = true;
                _counts[_countNum]._numKind = addHaiNumKind;
                _counts[_countNum]._num = 1;
                _countNum++;
                continue;
            }

            _counts[_countNum]._numKind = jyunTehaiNumKind;
            _counts[_countNum]._num = 1;

            if (!set && (jyunTehaiNumKind == addHaiNumKind))
            {
                set = true;
                _counts[_countNum]._num++;
            }

            while (++i < jyunTehaiLength) 
            {
                if (jyunTehaiNumKind == jyunTehais[i].getNumKind()) {
                    _counts[_countNum]._num++;
                }
                else {
                    break;
                }
            }

            _countNum++;
        }

        if(!set){
            _counts[_countNum]._numKind = addHaiNumKind;
            _counts[_countNum]._num = 1;
            _countNum++;
        }

        for (int i = 0; i < _countNum; i++)
        {
            // 5つ目の追加牌は無効とする
            if (_counts[i]._num > 4)
                _counts[i]._num--;
        }
    }


    public Combi[] getCombis()
    {
        return _combiManager._combis;
    }

    public int getCombiNum()
    {
        return _combiManager._combiNum;
    }


    public int getCombis(Combi[] combis)
    {
        _combiManager.initialize( getTotalCountLength() );
        searchCombi(0);

        if( _combiManager._combiNum == 0 ) 
        {
            _chiitoitsu = checkChiitoitsu();

            if( _chiitoitsu ) {
                _combiManager._combiNum = 1;
            }
            else 
            {
                _kokushi = checkKokushi();
                if( _kokushi )
                    _combiManager._combiNum = 1;
            }
        }

        return _combiManager._combiNum;
    }


    // 七对子.
    private bool _chiitoitsu;
    public bool isChiitoitsu()
    {
        return _chiitoitsu;
    }

    private bool checkChiitoitsu()
    {
        int count = 0;
        for (int i = 0; i < _countNum; i++) 
        {
            if (_counts[i]._num == 2) {
                count++;
            }
            else {
                return false;
            }
        }

        if (count == 7)
            return true;
        return false;
    }


    // 国士无双.
    private bool _kokushi;
    public bool isKokushi()
    {
        return _kokushi;
    }

    private bool checkKokushi()
    {
        //牌の数を調べるための配列 (0番地は使用しない）
        int[] checkId = {
            Hai.ID_WAN_1, Hai.ID_WAN_9, Hai.ID_PIN_1, Hai.ID_PIN_9, Hai.ID_SOU_1, Hai.ID_SOU_9,
            Hai.ID_TON, Hai.ID_NAN, Hai.ID_SYA, Hai.ID_PE, Hai.ID_HAKU, Hai.ID_HATSU, Hai.ID_CHUN
        };
        int[] countHai = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //手牌のIDを検索する
        for(int i = 0; i < _countNum; i++)
        {
            for(int j = 0; j < checkId.Length; j++)
            {
                if(Hai.NumKindToID(_counts[i]._numKind) == checkId[j])
                    countHai[j] = _counts[i]._num;
            }
        }

        bool atama = false;

        //国士無双が成立しているか調べる(手牌がすべて1.9字牌 すべての１,９字牌を持っている）
        for(int i = 0; i < countHai.Length; i++)
        {
            //0枚の牌があれば不成立
            if(countHai[i] == 0){
                return false;
            }
            if(countHai[i] == 2){
                atama = true;
            }
        }

        //条件を満たしていれば成立
        if(atama){
            return true;
        } 
        else {
            return false;
        }
    }


    private void searchCombi(int iSearch)
    {
        // 検索位置を更新する。
        for(; iSearch < _countNum; iSearch++)
        {
            if(_counts[iSearch]._num > 0)
                break;
        }

        if(iSearch >= _countNum)
            return;

        // 頭をチェック(check)する。
        if(_combiManager._current._atamaNumKind == 0) 
        {
            if (_counts[iSearch]._num >= 2)
            {
                // 頭を確定する。
                _counts[iSearch]._num -= 2;
                _combiManager._remain -= 2;
                _combiManager._current._atamaNumKind = _counts[iSearch]._numKind;

                // 上がりの組み合わせを見つけたら追加する。
                if (_combiManager._remain <= 0) {
                    _combiManager.add();
                } 
                else {
                    searchCombi(iSearch);
                }

                // 確定した頭を戻す。
                _counts[iSearch]._num += 2;
                _combiManager._remain += 2;
                _combiManager._current._atamaNumKind = 0;
            }
        }

        // 順子をチェックする。
        int left = iSearch;
        int center = iSearch + 1;
        int right = iSearch + 2;

        if( !Hai.isTsuu(_counts[left]._numKind) ) 
        {
            if ((_counts[left]._numKind + 1 == _counts[center]._numKind) && (_counts[center]._num > 0)) 
            {
                if ((_counts[left]._numKind + 2 == _counts[right]._numKind) && (_counts[right]._num > 0)) 
                {
                    // 順子を確定する
                    _counts[left]._num --;
                    _counts[center]._num --;
                    _counts[right]._num --;
                    _combiManager._remain -= 3;
                    _combiManager._current._shunNumKinds[_combiManager._current._shunNum] = _counts[left]._numKind;
                    _combiManager._current._shunNum++;

                    // 上がりの組み合わせを見つけたら追加する。
                    if (_combiManager._remain <= 0) {
                        _combiManager.add();
                    } 
                    else {
                        searchCombi(iSearch);
                    }

                    // 確定した順子を戻す。
                    _counts[left]._num ++;
                    _counts[center]._num ++;
                    _counts[right]._num ++;
                    _combiManager._remain += 3;
                    _combiManager._current._shunNum --;
                }
            }
        }

        // 刻子をチェックする。
        if (_counts[iSearch]._num >= 3)
        {
            // 刻子を確定する。
            _counts[iSearch]._num -= 3;
            _combiManager._remain -= 3;
            _combiManager._current._kouNumKinds[_combiManager._current._kouNum] = _counts[iSearch]._numKind;
            _combiManager._current._kouNum ++;

            // 上がりの組み合わせを見つけたら追加する。
            if (_combiManager._remain <= 0) {
                _combiManager.add();
            } 
            else {
                searchCombi(iSearch);
            }

            // 確定した刻子を戻す。/
            _combiManager._remain += 3;
            _counts[iSearch]._num += 3;
            _combiManager._current._kouNum --;
        }
    }
}
