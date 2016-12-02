

public class CountFormat 
{
    #region internal class.
    public class CombiManager 
    {
        // 上がりの組み合わせの配列の最大値
        public const int COMBI_MAX = 10;

        // 上がりの組み合わせの配列
        public HaiCombi[] combis;

        // 上がりの組み合わせの配列の有効な個数
        public int combiCount = 0;

        // カウントの配列の残りの個数
        public int remain = 0;

        public HaiCombi current = new HaiCombi();


        public CombiManager()
        {
            combis = new HaiCombi[COMBI_MAX];
            for (int i = 0; i < combis.Length; i++) {
                combis[i] = new HaiCombi();
            }
        }

        public void initialize(int remain)
        {
            this.combiCount = 0;
            this.remain = remain;

            current.atamaNumKind = 0;
            current.shunCount = 0;
            current.kouCount = 0;
        }

        public void add()
        {
            HaiCombi.copy(combis[combiCount], current);
            combiCount++;
        }
    }
    #endregion internal class.


    // カウント(count)の最大値
    public const int COUNTER_MAX = 14 + 2;

    // カウントの配列
    private HaiCounterInfo[] _counterArr;

    // カウントの配列の有効な個数
    private int _counterNum;

    // 上がりの組み合わせの配列を管理
    private CombiManager _combiManager;


    public CountFormat()
    {
        _combiManager = new CombiManager();

        _counterArr = new HaiCounterInfo[COUNTER_MAX];
        for (int i = 0; i < _counterArr.Length; i++) {
            _counterArr[i] = new HaiCounterInfo();
        }
    }

    public HaiCounterInfo[] getCounterArray()
    {
        return _counterArr;
    }
    public int getCounterArrLength()
    {
        return _counterNum;
    }

    public HaiCombi[] getCombis()
    {
        return _combiManager.combis;
    }
    public int getCombisLength()
    {
        return _combiManager.combiCount;
    }


    public void setCounterFormat(Tehai tehai, Hai addHai)
    {
        for (int i = 0; i < _counterArr.Length; i++)
        {
            _counterArr[i].reset();
        }
        _counterNum = 0;

        int addHaiNumKind = 0;
        bool set = true;

        if (addHai != null) {
            addHaiNumKind = addHai.getNumKind();
            set = false;
        }

        Hai[] jyunTehais = tehai.getJyunTehai();
        int jyunTehaiLength = tehai.getJyunTehaiLength();
        int jyunTehaiNumKind;

        for (int i = 0; i < jyunTehaiLength; )
        {
            jyunTehaiNumKind = jyunTehais[i].getNumKind();

            if(!set && (jyunTehaiNumKind > addHaiNumKind))
            {
                set = true;
                _counterArr[_counterNum].numKind = addHaiNumKind;
                _counterArr[_counterNum].count = 1;
                _counterNum++;
                continue;
            }

            _counterArr[_counterNum].numKind = jyunTehaiNumKind;
            _counterArr[_counterNum].count = 1;

            if (!set && (jyunTehaiNumKind == addHaiNumKind))
            {
                set = true;
                _counterArr[_counterNum].count++;
            }

            while (++i < jyunTehaiLength) 
            {
                if (jyunTehaiNumKind == jyunTehais[i].getNumKind()) {
                    _counterArr[_counterNum].count++;
                }
                else {
                    break;
                }
            }

            _counterNum++;
        }

        if(!set){
            _counterArr[_counterNum].numKind = addHaiNumKind;
            _counterArr[_counterNum].count = 1;
            _counterNum++;
        }

        for (int i = 0; i < _counterNum; i++)
        {
            // 5つ目の追加牌は無効とする
            if (_counterArr[i].count > 4)
                _counterArr[i].count--;
        }
    }

    public int calculateCombisCount(HaiCombi[] combis)
    {
        _combiManager.initialize( getTotalCounterLength() );
        searchCombi(0);

        if( _combiManager.combiCount == 0 ) 
        {
            _chiitoitsu = checkChiitoitsu();

            if( _chiitoitsu ) {
                _combiManager.combiCount = 1;
            }
            else 
            {
                _kokushi = checkKokushi();
                if( _kokushi )
                    _combiManager.combiCount = 1;
            }
        }

        return _combiManager.combiCount;
    }


    // 七对子.
    private bool _chiitoitsu;
    public bool isChiitoitsu()
    {
        return _chiitoitsu;
    }

    bool checkChiitoitsu()
    {
        int count = 0;

        for (int i = 0; i < _counterNum; i++) 
        {
            if (_counterArr[i].count == 2) {
                count++;
            }
            else {
                return false;
            }
        }

        return count == 7;
    }


    // 国士无双.
    private bool _kokushi;
    public bool isKokushi()
    {
        return _kokushi;
    }

    bool checkKokushi()
    {
        //牌の数を調べるための配列 (0番地は使用しない）
        int[] checkId = {
            Hai.ID_WAN_1, Hai.ID_WAN_9, Hai.ID_PIN_1, Hai.ID_PIN_9, Hai.ID_SOU_1, Hai.ID_SOU_9,
            Hai.ID_TON, Hai.ID_NAN, Hai.ID_SYA, Hai.ID_PE, Hai.ID_HAKU, Hai.ID_HATSU, Hai.ID_CHUN
        };
        int[] countHai = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //手牌のIDを検索する
        for(int i = 0; i < _counterNum; i++)
        {
            for(int j = 0; j < checkId.Length; j++)
            {
                if(Hai.NumKindToID(_counterArr[i].numKind) == checkId[j])
                    countHai[j] = _counterArr[i].count;
            }
        }

        //条件を満たしていれば不成立
        bool atama = false;

        //国士無双が成立しているか調べる(手牌がすべて1.9字牌 すべての１,９字牌を持っている）
        for(int i = 0; i < countHai.Length; i++)
        {
            //0枚の牌があれば不成立
            if(countHai[i] == 0)
                return false;

            if(countHai[i] == 2)
                atama = true;
        }

        return atama;
    }


    int getTotalCounterLength()
    {
        int totalCountLength = 0;

        for (int i = 0; i < _counterNum; i++) {
            totalCountLength += _counterArr[i].count;
        }

        return totalCountLength;
    }

    void searchCombi(int iSearch)
    {
        // 検索位置を更新する。
        for(; iSearch < _counterNum; iSearch++)
        {
            if(_counterArr[iSearch].count > 0)
                break;
        }

        if(iSearch >= _counterNum)
            return;

        // 頭をチェック(check)する。
        if(_combiManager.current.atamaNumKind == 0) 
        {
            if (_counterArr[iSearch].count >= 2)
            {
                // 頭を確定する。
                _counterArr[iSearch].count -= 2;
                _combiManager.remain -= 2;
                _combiManager.current.atamaNumKind = _counterArr[iSearch].numKind;

                // 上がりの組み合わせを見つけたら追加する。
                if (_combiManager.remain <= 0) {
                    _combiManager.add();
                } 
                else {
                    searchCombi(iSearch);
                }

                // 確定した頭を戻す。
                _counterArr[iSearch].count += 2;
                _combiManager.remain += 2;
                _combiManager.current.atamaNumKind = 0;
            }
        }

        // 順子をチェックする。
        int left = iSearch;
        int center = iSearch + 1;
        int right = iSearch + 2;

        if( !Hai.isTsuu(_counterArr[left].numKind) ) 
        {
            if ((_counterArr[left].numKind + 1 == _counterArr[center].numKind) && (_counterArr[center].count > 0)) 
            {
                if ((_counterArr[left].numKind + 2 == _counterArr[right].numKind) && (_counterArr[right].count > 0)) 
                {
                    // 順子を確定する
                    _counterArr[left].count --;
                    _counterArr[center].count --;
                    _counterArr[right].count --;
                    _combiManager.remain -= 3;
                    _combiManager.current.shunNumKinds[_combiManager.current.shunCount] = _counterArr[left].numKind;
                    _combiManager.current.shunCount++;

                    // 上がりの組み合わせを見つけたら追加する。
                    if (_combiManager.remain <= 0) {
                        _combiManager.add();
                    } 
                    else {
                        searchCombi(iSearch);
                    }

                    // 確定した順子を戻す。
                    _counterArr[left].count ++;
                    _counterArr[center].count ++;
                    _counterArr[right].count ++;
                    _combiManager.remain += 3;
                    _combiManager.current.shunCount --;
                }
            }
        }

        // 刻子をチェックする。
        if (_counterArr[iSearch].count >= 3)
        {
            // 刻子を確定する。
            _counterArr[iSearch].count -= 3;
            _combiManager.remain -= 3;
            _combiManager.current.kouNumKinds[_combiManager.current.kouCount] = _counterArr[iSearch].numKind;
            _combiManager.current.kouCount ++;

            // 上がりの組み合わせを見つけたら追加する。
            if (_combiManager.remain <= 0) {
                _combiManager.add();
            } 
            else {
                searchCombi(iSearch);
            }

            // 確定した刻子を戻す。/
            _combiManager.remain += 3;
            _counterArr[iSearch].count += 3;
            _combiManager.current.kouCount --;
        }
    }

}
