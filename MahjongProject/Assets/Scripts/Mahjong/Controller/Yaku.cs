
/// <summary>
/// 手牌全体の役を判定するクラスです
/// 计算役
/// </summary>

public class Yaku 
{    
    private Tehai _tehai;
    private Hai _addHai;
    private HaiCombi _combi;
    private AgariParam _agariParam;

    private int _doraCount = 0;
    private YakuHandler[] _yakuHandlers;

    private bool _nakiFlag = false;
    private bool _kokushi = false;

    public bool isKokushi { get { return _kokushi; } }
    public bool isNaki { get { return _nakiFlag; } }
    public int DoraCount{ get{ return _doraCount; } }

    public Tehai Tehai{ get{ return _tehai; } }
    public Hai AddHai{ get{ return _addHai; } }
    public HaiCombi Combi{ get{ return _combi; } }
    public AgariParam AgariParam{ get{ return _agariParam; } }


    public Yaku(Tehai tehai, Hai addHai, AgariParam param)
    {
        this._tehai = tehai;
        this._addHai = addHai;
        this._agariParam = param;

        #region handlers
        _yakuHandlers = new YakuHandler[]
        {
            new CheckKokushi(this),
            new CheckTenhou(this),
            new CheckTihou(this)
        };
        #endregion

        this._kokushi = _yakuHandlers[0].isHantei();

        FilterYakuPiece();
    }

    public Yaku(Tehai tehai, Hai addHai, HaiCombi combi, AgariParam param)
    {
        this._tehai = tehai;
        this._addHai = addHai;
        this._combi  = combi;
        this._agariParam = param;
        this._nakiFlag = tehai.isNaki();

        #region handlers
        _yakuHandlers = new YakuHandler[]
        {
            new CheckTanyao(this),
            new CheckPinfu(this),
            new CheckIpeikou(this),
            new CheckReach(this),
            new CheckIppatu(this),
            new CheckTsumo(this),
            new CheckTon(this),
            new CheckNan(this),
            new CheckSya(this),
            new CheckPei(this),
            new CheckHaku(this),
            new CheckHatu(this),
            new CheckCyun(this),
            new CheckHaitei(this),
            new CheckHoutei(this),
            new CheckRinsyan(this),
            new CheckCyankan(this),
            new CheckDoubleReach(this),
            new CheckTeetoitu(this),
            new CheckCyanta(this),
            new CheckIkkituukan(this),
            new CheckSansyokuDoukou(this),
            new CheckSansyokuDoujun(this),
            new CheckToitoi(this),
            new CheckSanankou(this),
            new CheckSankantu(this),
            new CheckRyanpeikou(this),
            new CheckHonitu(this),
            new CheckJunCyan(this),
            new CheckSyousangen(this),
            new CheckHonroutou(this),
            new CheckTinitu(this),
            new CheckSuuankou(this),
            new CheckSuukantu(this),
            new CheckDaisangen(this),
            new CheckSyousuushi(this),
            new CheckDaisuushi(this),
            new CheckTuuisou(this),
            new CheckChinroutou(this),
            new CheckRyuuisou(this),
            new CheckCyuurennpoutou(this),
            new CheckKokushi(this),
            new CheckTenhou(this),
            new CheckTihou(this),
            new CheckDora(this)
        };
        #endregion

        _yakuHandlers[_yakuHandlers.Length - 1].setHanSuu(_doraCount);

        FilterYakuPiece();
    }

    public Yaku(Tehai tehai, Hai addHai, HaiCombi combi, AgariParam param, int a_status)
    {
        this._tehai = tehai;
        this._addHai = addHai;
        this._combi  = combi;
        this._agariParam = param;
        this._nakiFlag = tehai.isNaki();

        #region handlers
        _yakuHandlers = new YakuHandler[]
        {
            new CheckTanyao(this),
            new CheckReach(this),
            new CheckIppatu(this),
            new CheckTsumo(this),
            new CheckHaitei(this),
            new CheckHoutei(this),
            new CheckRinsyan(this),
            new CheckDoubleReach(this),
            new CheckTeetoitu(this),
            new CheckHonroutouChiitoitsu(this),
            new CheckHonitu(this),
            new CheckTinitu(this),
            new CheckTuuisou(this),
            new CheckTenhou(this),
            new CheckTihou(this),
            new CheckDora(this)
        };
        #endregion

        _yakuHandlers[_yakuHandlers.Length - 1].setHanSuu(_doraCount);

        FilterYakuPiece();
    }

    protected void FilterYakuPiece()
    {
        //役満成立時は他の一般役は切り捨てる
        if(_yakuHandlers != null)
        {
            for(int i = 0; i < _yakuHandlers.Length; i++)
            {
                if( _yakuHandlers[i].isYakuman() && _yakuHandlers[i].isHantei()) 
                {
                    for(int j = 0; j < _yakuHandlers.Length; j++)
                    {
                        if(_yakuHandlers[j].isYakuman() == false)
                            _yakuHandlers[j].setYakuHantei(false);
                    }
                }
            }
        }
    }


    public int calculateHanSuu()
    {
        int hanSuu = 0;
        for(int i = 0 ; i < _yakuHandlers.Length ; i++)
        {
            if( _yakuHandlers[i].isHantei() == true)
                hanSuu += _yakuHandlers[i].getHanSuu();
        }

        // ドラのみは無し
        if (hanSuu == _yakuHandlers[_yakuHandlers.Length - 1].getHanSuu()) {
            return 0;
        }

        return hanSuu;
    }

    public int getHan()
    {
        int hanSuu = 0;
        for(int i = 0 ; i < _yakuHandlers.Length ; i++)
        {
            if( _yakuHandlers[i].isHantei() == true)
                hanSuu += _yakuHandlers[i].getHanSuu();
        }

        return hanSuu;
    }

    public string[] getYakuNames()
    {
        int count = 0;
        int hanSuu;

        //成立している役の数をカウント
        for(int i = 0 ; i < _yakuHandlers.Length ; i++)
        {
            if( _yakuHandlers[i].isHantei() == true)
                count++;
        }

        string[] yakuNames = new string[count];
        count = 0;

        for(int i = 0 ; i < _yakuHandlers.Length ; i++)
        {
            if( _yakuHandlers[i].isHantei() == true)
            {
                hanSuu = _yakuHandlers[i].getHanSuu();
                if (hanSuu >= 13) {
                    yakuNames[count] = _yakuHandlers[i].getYakuName() + " " + ResManager.getString("yakuman");
                } 
                else {
                    yakuNames[count] = _yakuHandlers[i].getYakuName() + " " + hanSuu.ToString() + ResManager.getString("han");
                }
                count++;
            }
        }

        return yakuNames;
    }


    public bool checkIfYakuMan()
    {
        for(int i = 0; i < _yakuHandlers.Length; i++)
        {
            if(  _yakuHandlers[i].isYakuman() )
                return true;
        }
        return false;
    }


    // 断幺九. //
    public bool checkTanyao() 
    {
        /// 追加牌をチェック
        //１９字牌ならば不成立
        if (_addHai.IsYaochuu == true)
            return false;

        //純手牌をチェック
        Hai[] jyunTehai = _tehai.getJyunTehai();
        for (int i = 0; i < jyunTehai.Length; i++)
        {
            //１９字牌ならば不成立
            if (jyunTehai[i].IsYaochuu == true)
                return false;
        }


        Hai checkHai;

        Fuuro[] fuuros = _tehai.getFuuros();

        // 喰いタンなしで、鳴いていたら不成立
        if( AgariParam.getYakuFlag( (int)EYakuFlagType.KUITAN ) == false)
        {
            if(fuuros.Length > 0)
                return false;
        }

        for (int i = 0; i < fuuros.Length; i++)
        {
            switch( fuuros[i].Type )
            {
                case EFuuroType.MinShun:
                {
                    //明順の牌をチェック
                    checkHai = fuuros[i].Hais[0];

                    //123 と 789 の順子があれば不成立
                    if( checkHai.Num == 1 || checkHai.Num == 7)
                        return false;
                }
                break;

                case EFuuroType.MinKou:
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    //暗槓の牌をチェック
                    checkHai = fuuros[i].Hais[0];

                    if (checkHai.IsYaochuu == true)
                        return false;
                }
                break;
            }
        }

        return true;
    }

    // 平和
    public bool checkPinfu()
    {
        //鳴きが入っている場合は成立しない
        if(_nakiFlag == true)
            return false;

        //面子が順子だけではない
        if(_combi.shunCount != 4)
            return false;

        //頭が三元牌
        Hai atamaHai = new Hai(Hai.NumKindToID(_combi.atamaNumKind));
        if( atamaHai.Kind == Hai.KIND_SANGEN )
            return false;

        //頭が場風
        if( atamaHai.Kind == Hai.KIND_FON
           && (atamaHai.Num-1) == (int)AgariParam.getBakaze())
        {
            return false;
        }

        //頭が自風
        if( atamaHai.Kind == Hai.KIND_FON
           && (atamaHai.Num - 1) == (int)AgariParam.getJikaze())
        {
            return false;
        }

        //字牌の頭待ちの場合は不成立//
        if(_addHai.IsTsuu == true)
            return false;

        //待ちが両面待ちか判定//
        bool ryanmenFlag = false;
        int addHaiNumKind = _addHai.NumKind;

        //上がり牌の数をチェックして場合分け
        switch(_addHai.Num)
        {
            //上がり牌が1,2,3の場合は123,234,345の順子ができているかどうかチェック
            case 1:
            case 2:
            case 3:
            {
                for(int i = 0 ; i < _combi.shunCount ; i++)
                {
                    if(addHaiNumKind == _combi.shunNumKinds[i])
                        ryanmenFlag = true;
                }
            }
            break;

            //上がり牌が4,5,6の場合は456か234,567か345,678か456の順子ができているかどうかチェック
            case 4:
            case 5:
            case 6:
            {
                for(int i = 0 ; i < _combi.shunCount ; i++)
                {
                    if((addHaiNumKind == _combi.shunNumKinds[i]) ||
                       (addHaiNumKind-2 == _combi.shunNumKinds[i]))
                    {
                        ryanmenFlag = true;
                    }
                }
            }
            break;

            //上がり牌が7,8,9の場合は567,678,789の順子ができているかどうかチェック
            case 7:
            case 8:
            case 9:
            {
                for(int i = 0 ; i < _combi.shunCount ; i++)
                {
                    if( addHaiNumKind-2 == _combi.shunNumKinds[i] )
                        ryanmenFlag = true;
                }
            }
            break;
        }

        if(ryanmenFlag == false)
            return false;
        
        return true;
    }

    // 一杯口 //
    public bool checkIpeikou()
    {
        //鳴きが入っている場合は成立しない
        if(_nakiFlag == true)
            return false;

        //順子の組み合わせを確認する
        for (int i = 0; i < _combi.shunCount -1; i++)
        {
            if(_combi.shunNumKinds[i] == _combi.shunNumKinds[i+1])
                return true;
        }

        return false;
    }

    // 立直 //
    public bool checkReach()
    {
        return AgariParam.getYakuFlag( (int)EYakuFlagType.REACH );
    }

    // 一发(立直后，轮牌内和了) //
    public bool checkIppatu()
    {
        return AgariParam.getYakuFlag( (int)EYakuFlagType.IPPATU );
    }

    // 门前清自摸和 //
    public bool checkTsumo()
    {
        //鳴きが入っている場合は成立しない
        if(_nakiFlag == true)
            return false;

        return AgariParam.getYakuFlag((int)EYakuFlagType.TSUMO);
    }


    //役牌ができているかどうかの判定に使う補助メソッド
    public bool checkYakuHai(Tehai tehai, HaiCombi combi, int yakuHaiID)
    {
        int id;

        //純手牌をチェック
        for(int i = 0; i < combi.kouCount ; i++)
        {
            //IDと役牌のIDをチェック
            id = Hai.NumKindToID(combi.kouNumKinds[i]);
            if( id == yakuHaiID )
                return true;
        }

        Fuuro[] fuuros = tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch( fuuros[i].Type ) 
            {
                case EFuuroType.MinKou:
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    //IDと役牌のIDをチェック
                    id = fuuros[i].Hais[0].ID;
                    if( id == yakuHaiID )
                        return true;
                }
                break;
            }
        }
        return false;
    }

    // 东 //
    public bool checkTon()
    {
        if((AgariParam.getJikaze() == EKaze.Ton) || 
           (AgariParam.getBakaze() == EKaze.Ton))
        {
            return checkYakuHai(_tehai,_combi, Hai.ID_TON);
        }
        else{
            return false;
        }
    }

    // 南 //
    public bool checkNan()
    {
        if((AgariParam.getJikaze() == EKaze.Nan) || 
           (AgariParam.getBakaze() == EKaze.Nan))
        {
            return checkYakuHai(_tehai,_combi, Hai.ID_NAN);
        }
        else{
            return false;
        }
    }

    // 西 //
    public bool checkSya()
    {
        if(AgariParam.getJikaze() == EKaze.Sya){
            return checkYakuHai(_tehai, _combi, Hai.ID_SYA);
        }
        else{
            return false;
        }
    }

    // 北 //
    public bool checkPei()
    {
        if(AgariParam.getJikaze() == EKaze.Sya){
            return checkYakuHai(_tehai, _combi, Hai.ID_PE);
        }
        else{
            return false;
        }
    }

    // 白 //
    public bool checkHaku()
    {
        return checkYakuHai(_tehai, _combi, Hai.ID_HAKU);
    }

    // 发 //
    public bool checkHatsu()
    {
        return checkYakuHai(_tehai, _combi, Hai.ID_HATSU);
    }

    // 中 //
    public bool checkCyun()
    {
        return checkYakuHai(_tehai, _combi, Hai.ID_CHUN);
    }

    // 海底捞月 //
    public bool checkHaitei()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.HAITEI);
    }

    // 河底捞鱼 //
    public bool checkHoutei()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.HOUTEI);
    }

    // 岭上开花 //
    public bool checkRinsyan()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.RINSYAN);
    }

    // 抢杠 //
    public bool checkCyankan()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.CHANKAN);
    }

    // 双立直 //
    public bool checkDoubleReach()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.DOUBLE_REACH);
    }

    // 七对子 //
    public bool checkTeetoitu()
    {
        //鳴きが入っている場合は成立しない
        if(_nakiFlag == true)
            return false;

        Tehai tehai = new Tehai(_tehai);
        tehai.addJyunTehai(_addHai);
        tehai.Sort();
        Hai[] jyunTehai = tehai.getJyunTehai();

        Hai checkHai = jyunTehai[0];
        int count = 1;

        for(int i = 1; i < jyunTehai.Length; i++)
        {
            if( jyunTehai[i].ID == checkHai.ID ){
                count++;
            }
            else{
                if(count != 2){
                    return false;
                }
                else{
                    checkHai = jyunTehai[i];
                    count = 1;
                }
            }
        }

        return true;
    }

    // 混全带幺九 //
    public bool checkCyanta()
    {
        Hai checkHai;

        //純手牌の刻子をチェック
        for(int i = 0; i < _combi.kouCount ; i++)
        {
            checkHai = new Hai(Hai.NumKindToID(_combi.kouNumKinds[i]));

            //数牌の場合は数字をチェック
            if (checkHai.IsYaochuu == false)
                return false;
        }

        //純手牌の順子をチェック
        for(int i = 0; i < _combi.shunCount ; i++)
        {
            checkHai = new Hai(Hai.NumKindToID(_combi.shunNumKinds[i]));

            //数牌の場合は数字をチェック
            if (checkHai.IsTsuu == false)
            {
                if ((checkHai.Num > 1) && (checkHai.Num < 7))
                    return false;
            }
        }

        //純手牌の頭をチェック
        checkHai = new Hai(Hai.NumKindToID(_combi.atamaNumKind));
        if (checkHai.IsYaochuu == false)
            return false;

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            checkHai = fuuros[i].Hais[0];

            switch( fuuros[i].Type ) 
            {
                case EFuuroType.MinShun:
                {
                    //123 と 789 以外の順子があれば不成立
                    if( (checkHai.Num > 1) && (checkHai.Num < 7) )
                        return false;
                }
                break;

                case EFuuroType.MinKou:
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    //数牌の場合は数字をチェック
                    if( checkHai.IsYaochuu == false )
                        return false;
                }
                break;
            }
        }

        return true;
    }

    // 一气通贯 //
    public bool checkIkkituukan()
    {
        bool[] ikkituukanFlag = {false,false,false,false,false,false,false,false,false};

        //萬子、筒子、索子の1,4,7をチェック
        int[] checkId = { Hai.ID_WAN_1, Hai.ID_WAN_4, Hai.ID_WAN_7, Hai.ID_PIN_1, Hai.ID_PIN_4, Hai.ID_PIN_7, Hai.ID_SOU_1, Hai.ID_SOU_4, Hai.ID_SOU_7 };


        int id;

        //手牌の順子をチェック
        for(int i = 0; i < _combi.shunCount; i++)
        {
            id = Hai.NumKindToID(_combi.shunNumKinds[i]);

            for(int j =0 ; j < checkId.Length ; j++)
            {
                if(id == checkId[j])
                    ikkituukanFlag[j] = true;
            }
        }

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch( fuuros[i].Type ) 
            {
                case EFuuroType.MinShun:
                {
                    //鳴いた牌をチェック
                    id = fuuros[i].Hais[0].ID;

                    for(int j =0 ; j < checkId.Length ; j++)
                    {
                        if(id == checkId[j])
                            ikkituukanFlag[j] = true;
                    }
                }
                break;
            }
        }

        //一気通貫が出来ているかどうかチェック
        if( (ikkituukanFlag[0] == true && ikkituukanFlag[1] == true && ikkituukanFlag[2] == true ) ||
           (ikkituukanFlag[3] == true && ikkituukanFlag[4] == true && ikkituukanFlag[5] == true ) ||
           (ikkituukanFlag[6] == true && ikkituukanFlag[7] == true && ikkituukanFlag[8] == true ))
        {
            return true;
        }
        else{
            return false;
        }
    }

    //三色ができているかどうかの判定に使う補助メソッド
    static void checkSansyoku(int id, bool[][] sansyokuFlag)
    {
        //萬子、筒子、索子をチェック
        int[] checkId = { Hai.ID_WAN_1, Hai.ID_PIN_1, Hai.ID_SOU_1 };

        for(int i = 0 ; i < sansyokuFlag.Length ; i++)
        {
            for(int j = 0; j < sansyokuFlag[i].Length; j++)
            {
                if( id == (checkId[i] + j) )
                    sansyokuFlag[i][j] = true;
            }
        }
    }

    // 三色同顺 //
    public bool checkSansyokuDoujun()
    {
        const int Column = 9;
        bool[][] sansyokuFlag = new bool[3][];

        //フラグの初期化
        for(int i = 0; i < sansyokuFlag.Length; i++)
        {
            sansyokuFlag[i] = new bool[Column];

            for(int k = 0; k < sansyokuFlag[i].Length; k++){
                sansyokuFlag[i][k] = false;
            }
        }

        int id = 0;

        //手牌の順子をチェック
        for(int i = 0 ; i < _combi.shunCount ; i++)
        {
            id = Hai.NumKindToID(_combi.shunNumKinds[i]);
            checkSansyoku(id, sansyokuFlag);
        }

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++)
        {
            switch(fuuros[i].Type)
            {
                case EFuuroType.MinShun:
                {
                    //鳴いた牌をチェック
                    id = fuuros[i].Hais[0].ID;
                    checkSansyoku(id, sansyokuFlag);
                }
                break;
            }
        }

        //三色同順が出来ているかどうかチェック
        for(int i = 0 ; i < sansyokuFlag[0].Length ; i++)
        {
            if( (sansyokuFlag[0][i] == true) && 
               (sansyokuFlag[1][i] == true ) && 
               (sansyokuFlag[2][i] == true))
            {
                return true;
            }
        }

        return false;
    }

    // 三色同刻 //
    public bool checkSansyokuDoukou()
    {
        const int Column = 9;
        bool[][] sansyokuFlag = new bool[3][];

        //フラグの初期化
        for(int i = 0; i < sansyokuFlag.Length; i++)
        {
            sansyokuFlag[i] = new bool[Column];

            for(int k = 0; k < sansyokuFlag[i].Length; k++){
                sansyokuFlag[i][k] = false;
            }
        }

        int id = 0;

        //手牌の刻子をチェック
        for(int i = 0 ; i < _combi.kouCount ; i++)
        {
            id = Hai.NumKindToID(_combi.kouNumKinds[i]);
            checkSansyoku(id, sansyokuFlag);
        }

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch (fuuros[i].Type)
            {
                case EFuuroType.MinKou:
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    id = fuuros[i].Hais[0].ID;
                    checkSansyoku(id, sansyokuFlag);
                }
                break;
            }
        }

        //三色同刻が出来ているかどうかチェック
        for(int i = 0; i < sansyokuFlag[0].Length ; i++)
        {
            if( (sansyokuFlag[0][i] == true) && 
               (sansyokuFlag[1][i] == true ) && 
               (sansyokuFlag[2][i] == true))
            {
                return true;
            }
        }

        //出来ていない場合 falseを返却
        return false;
    }

    // 对对和 //
    public bool checkToitoi()
    {
        int minShunCount = 0;

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            if( fuuros[i].Type == EFuuroType.MinShun )
                minShunCount++;
        }

        //手牌に順子がある
        if( _combi.shunCount != 0 || minShunCount != 0 ){
            return false;
        }
        else{
            return true;
        }
    }

    // 三暗刻 //
    public bool checkSanankou()
    {
        //対々形で鳴きがなければ成立している【ツモ和了りや単騎の場合、四暗刻が優先される）
        if( checkToitoi() == true && _nakiFlag == false )
            return true;

        int anKanCount = 0;

        Fuuro[] fuuros = _tehai.getFuuros();

        for(int i = 0; i < fuuros.Length; i++) 
        {
            if( fuuros[i].Type == EFuuroType.AnKan )
                anKanCount++;
        }

        //暗刻と暗槓の合計が３つではない場合は不成立
        if( _combi.kouCount + anKanCount != 3 )
            return false;

        //ツモ上がりの場合は成立
        if( AgariParam.getYakuFlag((int)EYakuFlagType.TSUMO) )
        {
            return true;
        }
        else  //ロン上がりの場合、和了った牌と
        {
            int numKind = _addHai.NumKind;

            //ロン上がりで頭待ちの場合は成立
            if(numKind == _combi.atamaNumKind){
                return true;
            }
            else
            {
                //和了った牌と刻子になっている牌が同じか確認
                bool checkFlag = false;
                for(int i = 0 ; i < _combi.kouCount ; i++)
                {
                    if(numKind == _combi.kouNumKinds[i])
                        checkFlag = true;
                }

                //刻子の牌で和了った場合
                if(checkFlag == true)
                {
                    //字牌ならば不成立
                    if(_addHai.IsTsuu == false)
                    {
                        // 順子の待ちにもなっていないか確認する
                        // 例:11123 で1で和了り, 45556の5で和了り
                        bool checkshun = false;

                        for(int i = 0 ; i < _combi.shunCount ; i++)
                        {
                            switch(_addHai.Num)
                            {
                                case 1:
                                {
                                    if(numKind == _combi.shunNumKinds[i]){
                                        checkshun = true;
                                    }
                                }
                                break;

                                case 2:
                                {
                                    if((numKind == _combi.shunNumKinds[i]) ||
                                       (numKind-1 == _combi.shunNumKinds[i]))
                                    {
                                        checkshun = true;
                                    }
                                }
                                break;

                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                {
                                    if( (numKind == _combi.shunNumKinds[i]) ||
                                       (numKind-1 == _combi.shunNumKinds[i]) ||
                                       (numKind-2 == _combi.shunNumKinds[i]) )
                                    {
                                        checkshun = true;
                                    }
                                }
                                break;

                                case 8:
                                {
                                    if( (numKind-1 == _combi.shunNumKinds[i]) ||
                                       (numKind-2 == _combi.shunNumKinds[i]))
                                    {
                                        checkshun = true;
                                    }
                                }
                                break;

                                case 9:
                                {
                                    if( numKind-2 == _combi.shunNumKinds[i] ){
                                        checkshun = true;
                                    }
                                }
                                break;
                            } // end switch().

                        } // end for().

                        return checkshun;
                    }
                    else
                    {
                        return false;
                    } // end else.
                }
                else  //刻子と関係ない牌で和了った場合は成立
                {
                    return true;
                }

            } // end else.

        } // end else.

    }

    // 三杠子 //
    public bool checkSankantu()
    {
        int kanCount = 0;

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch (fuuros[i].Type) 
            {
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                    kanCount ++;
                break;
            }
        }

        if(kanCount == 3){
            return true;
        }
        else{
            return false;
        }
    }

    // 二杯口 //
    public bool checkRyanpeikou()
    {
        //鳴きが入っている場合は成立しない
        if(_nakiFlag == true)
            return false;

        //順子が４つである
        if(_combi.shunCount < 4)
            return false;

        //順子の組み合わせを確認する
        if( _combi.shunNumKinds[0] == _combi.shunNumKinds[1] && 
            _combi.shunNumKinds[2] == _combi.shunNumKinds[3])
        {
            return true;
        }
        else{
            return false;
        }
    }

    // 混一色 //
    public bool checkHonitu()
    {
        //萬子、筒子、索子をチェック
        int[] checkId = { Hai.KIND_WAN, Hai.KIND_PIN, Hai.KIND_SOU };


        Hai checkHai;

        Hai[] jyunTehais = _tehai.getJyunTehai();
        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0 ; i < checkId.Length ; i++)
        {
            bool honituFlag = true;

            //純手牌をチェック
            for (int j = 0; j < jyunTehais.Length; j++)
            {
                //牌が(萬子、筒子、索子)以外もしくは字牌以外
                if ( (jyunTehais[j].Kind != checkId[i]) && (jyunTehais[j].IsTsuu == false) ){
                    honituFlag = false;
                }
            }

            //副露をチェック
            for (int j = 0; j < fuuros.Length; j++)
            {
                checkHai = fuuros[j].Hais[0];

                //牌が(萬子、筒子、索子)以外もしくは字牌以外
                if( (checkHai.Kind != checkId[i]) && (checkHai.IsTsuu == false) ){
                    honituFlag = false;
                }
            }

            //混一が成立している
            if(honituFlag == true)
                return true;

        } // end for(i < checkId.Length).

        return false;
    }

    // 纯全带幺九 //
    public bool checkJunCyan()
    {
        Hai checkHai;

        //純手牌の刻子をチェック
        for(int i = 0; i < _combi.kouCount ; i++)
        {
            checkHai = new Hai(Hai.NumKindToID(_combi.kouNumKinds[i]));

            //字牌があれば不成立
            if( checkHai.IsTsuu == true)
                return false;

            //中張牌ならば不成立
            if(checkHai.IsYaochuu == false )
                return false;
        }

        //純手牌の順子をチェック
        for(int i = 0; i < _combi.shunCount ; i++)
        {
            checkHai = new Hai(Hai.NumKindToID(_combi.shunNumKinds[i]));

            //字牌があれば不成立
            if( checkHai.IsTsuu == true)
                return false;

            //数牌の場合は数字をチェック
            if( (checkHai.Num > Hai.NUM_1) && (checkHai.Num < Hai.NUM_7) ) {
                return false;
            }
        }

        //純手牌の頭をチェック
        checkHai = new Hai(Hai.NumKindToID(_combi.atamaNumKind));

        //字牌があれば不成立
        if( checkHai.IsTsuu == true)
            return false;

        //中張牌ならば不成立
        if(checkHai.IsYaochuu == false )
            return false;

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            checkHai = fuuros[i].Hais[0];

            switch (fuuros[i].Type)
            {
                case EFuuroType.MinShun:
                {
                    //123 と 789 以外の順子があれば不成立
                    if( (checkHai.Num > Hai.NUM_1) && (checkHai.Num < Hai.NUM_7) ) {
                        return false;
                    }
                }
                break;

                case EFuuroType.MinKou:
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    //字牌があれば不成立
                    if( checkHai.IsTsuu == true)
                        return false;

                    //中張牌ならば不成立
                    if( checkHai.IsYaochuu == false )
                        return false;
                }
                break;
            }
        }

        return true;
    }

    // 小三元 //
    public bool checkSyousangen()
    {
        //三元牌役が成立している個数を調べる
        int sangenCount = 0;

        //白が刻子
        if(checkHaku() == true)
            sangenCount++;
        
        //発が刻子
        if(checkHatsu() == true)
            sangenCount++;
        
        //中が刻子
        if(checkCyun() == true)
            sangenCount++;
        
        //頭が三元牌 かつ、三元牌役が2つ成立

        if( ((_combi.atamaNumKind & Hai.KIND_SANGEN) == Hai.KIND_SANGEN) && (sangenCount == 2) ) {
            return true;
        }

        return false;
    }

    // 混老头 //
    public bool checkHonroutou()
    {
        //トイトイが成立している
        if(checkToitoi() == false)
            return false;

        //チャンタが成立している
        if(checkCyanta() == true){
            return true;
        }
        else{
            return false;
        }
    }

    // 混老头七对子 //
    // TODO: 三元牌 and 风牌 check?
    public bool checkHonroutouChiitoitsu()
    {
        //１９字牌ならば成立
        if (_addHai.IsYaochuu == false)
            return false;

        //純手牌をチェック
        Hai[] jyunTehai = _tehai.getJyunTehai();

        for (int i = 0; i < jyunTehai.Length; i++)
        {
            //１９字牌ならば成立
            if (jyunTehai[i].IsYaochuu == false)
                return false;
        }

        return true;
    }

    // 人和 //
    public bool checkRenhou()
    {
        if( AgariParam.getYakuFlag((int)EYakuFlagType.RENHOU) ) {
            return true;
        }
        else{
            return false;
        }
    }

    // 清一色 //
    public bool checkTinitu()
    {
        //萬子、筒子、索子をチェック
        int[] checkId = { Hai.KIND_WAN, Hai.KIND_PIN, Hai.KIND_SOU };

        Hai[] jyunTehais = _tehai.getJyunTehai();
        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0 ; i < checkId.Length ; i++)
        {
            bool tinituFlag = true;

            //純手牌をチェック
            for (int j = 0; j < jyunTehais.Length; j++)
            {
                //牌が(萬子、筒子、索子)以外
                if (jyunTehais[j].Kind != checkId[i]){
                    tinituFlag = false;
                    break;
                }
            }

            for (int j = 0; j < fuuros.Length; j++)
            {
                //牌が(萬子、筒子、索子)以外
                if( fuuros[j].Hais[0].Kind != checkId[i] ){
                    tinituFlag = false;
                    break;
                }
            }

            //清一が成立している
            if(tinituFlag == true)
                return true;

        } // end for().

        return false;
    }

    // 四暗刻 //
    public bool checkSuuankou()
    {
        int anKanCount = 0;

        Fuuro[] fuuros = _tehai.getFuuros();
        for (int i = 0; i < fuuros.Length; i++) 
        {
            if( fuuros[i].Type == EFuuroType.AnKan )
                anKanCount++;
        }

        //手牌の暗刻が4つ
        if( _combi.kouCount + anKanCount < 4 ){
            return false;
        }
        else
        {
            //ツモ和了りの場合は成立
            if( AgariParam.getYakuFlag((int)EYakuFlagType.TSUMO) ) {
                return true;
            }
            else //ロン和了りの場合
            {
                //頭待ちならば成立 (四暗刻単騎待ち)
                if(_addHai.NumKind == _combi.atamaNumKind){
                    return true;
                }
                else{
                    return false;
                }
            }
        }
    }

    // 四杠子 //
    public bool checkSuukantu()
    {
        int kanCount = 0;

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch (fuuros[i].Type)
            {
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                    kanCount++;
                break;
            }
        }

        return kanCount >= 4;
    }

    // 大三元 //
    public bool checkDaisangen()
    {
        //三元牌役が成立している個数を調べる
        int sangenCount = 0;

        //白が刻子
        if(checkHaku() == true)
            sangenCount++;
        
        //発が刻子
        if(checkHatsu() == true)
            sangenCount++;
        
        //中が刻子
        if(checkCyun() == true)
            sangenCount++;

        //３元牌が３つ揃っている
        return sangenCount >= 3;
    }

    // 天和 //
    public bool checkTenhou()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.TENHOU);
    }

    // 地和 //
    public bool checkTihou()
    {
        return AgariParam.getYakuFlag((int)EYakuFlagType.TIHOU);
    }

    // 小四喜 //
    public bool checkSyousuushi()
    {
        //風牌役が成立している個数を調べる
        int fonCount = 0;

        //東が刻子
        if(checkYakuHai(_tehai,_combi, Hai.ID_TON) )
            fonCount++;
        
        //南が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_NAN) )
            fonCount++;
        
        //西が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_SYA) )
            fonCount++;
        
        //北が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_PE) )
            fonCount++;

        //頭が風牌 かつ、風牌役が3つ成立
        if( ((_combi.atamaNumKind & Hai.KIND_FON) == Hai.KIND_FON) && (fonCount == 3) ) {
            return true;
        }
        else{
            return false;
        }
    }

    // 大四喜 //
    public bool checkDaisuushi()
    {
        //風牌役が成立している個数を調べる
        int fonCount = 0;

        //東が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_TON) )
            fonCount++;
        
        //南が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_NAN) )
            fonCount++;
        
        //西が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_SYA) )
            fonCount++;
        
        //北が刻子
        if( checkYakuHai(_tehai, _combi, Hai.ID_PE) )
            fonCount++;
        

        //風牌役が4つ成立
        return fonCount >= 4;
    }

    // 字一色 //
    public bool checkTuuisou()
    {
        if (_addHai.IsTsuu == false)
            return false;

        //順子があるかどうか確認
        if(checkToitoi() == false)
            return false;

        //純手牌をチェック
        Hai[] jyunTehais = _tehai.getJyunTehai();
        for (int j = 0; j < jyunTehais.Length; j++)
        {
            //牌が字牌ではない
            if (jyunTehais[j].IsTsuu == false)
                return false;
        }

        Fuuro[] fuuros = _tehai.getFuuros();
        for (int i = 0; i < fuuros.Length; i++)
        {
            //牌が字牌ではない
            if( fuuros[i].Hais[0].IsTsuu == false )
                return false;
        }

        return true;
    }

    // 清老头 //
    public bool checkChinroutou()
    {
        //順子があるかどうか確認
        if(checkToitoi() == false)
            return false;

        //順子なしでジュンチャンが成立しているか（1と9のみで作成）
        if(checkJunCyan() == false)
            return false;

        return true;
    }

    // 绿一色 //
    public bool checkRyuuisou()
    {
        int[] checkId = { Hai.ID_SOU_2, Hai.ID_SOU_3, Hai.ID_SOU_4, Hai.ID_SOU_6, Hai.ID_SOU_8, Hai.ID_HATSU };

        bool ryuuisouFlag = false;
        int id;

        //純手牌をチェック
        Hai[] jyunTehais = _tehai.getJyunTehai();

        for (int i = 0; i < jyunTehais.Length; i++) 
        {
            id = jyunTehais[i].ID;
            ryuuisouFlag = false;

            for(int j = 0 ; j < checkId.Length ; j++)
            {
                //緑一色に使用できる牌だった
                if( id == checkId[j] )
                    ryuuisouFlag = true;
            }

            //該当する牌ではなかった
            if(ryuuisouFlag == false)
                return false;
        }

        Fuuro[] fuuros = _tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch (fuuros[i].Type)
            {
                case EFuuroType.MinShun:
                {
                    //明順の牌をチェック
                    id = fuuros[i].Hais[0].ID;

                    //索子の2,3,4以外の順子があった場合不成立
                    if( id != Hai.ID_SOU_2 ) 
                        return false;
                }
                break;

                case EFuuroType.MinKou:
                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    id = fuuros[i].Hais[0].ID;
                    ryuuisouFlag = false;

                    for(int j = 0 ; j < checkId.Length ; j++)
                    {
                        //緑一色に使用できる牌だった
                        if(id == checkId[j])
                            ryuuisouFlag = true;
                    }

                    //該当する牌ではなかった
                    if(ryuuisouFlag == false)
                        return false;
                }
                break;
            }
        }

        return true;
    }

    // 九莲宝灯 //
    public bool checkCyuurennpoutou()
    {
        //鳴きがある場合は成立しない
        if( _nakiFlag == true )
            return false;

        //手牌が清一になっていない場合も成立しない
        if(checkTinitu() == false)
            return false;

        //牌の数を調べるための配列 (0番地は使用しない）
        int[] countNumber = {0,0,0,0,0,0,0,0,0,0};

        //手牌をコピーする
        Hai[] checkHais = _tehai.getJyunTehai();

        //手牌にある牌の番号を調べる
        for(int i = 0; i < _tehai.getJyunTehai().Length; i++)
        {
            //数字の番号をインクリメントする
            countNumber[checkHais[i].Num]++;
        }

        //九蓮宝燈になっているか調べる（1と9が３枚以上 2～8が１枚以上)
        if(( countNumber[1] >= 3)&& 
           ( countNumber[2] >= 1)&& 
           ( countNumber[3] >= 1)&& 
           ( countNumber[4] >= 1)&& 
           ( countNumber[5] >= 1)&& 
           ( countNumber[6] >= 1)&& 
           ( countNumber[7] >= 1)&& 
           ( countNumber[8] >= 1)&& 
           ( countNumber[9] >= 3))
        {
            return true;
        }

        return false;
    }

    // 国士无双 //
    public bool checkKokushi()
    {
        //鳴きがある場合は成立しない
        if( _nakiFlag == true )
            return false;

        //牌の数を調べるための配列 (0番地は使用しない）
        int[] checkId = { 
            Hai.ID_WAN_1, Hai.ID_WAN_9,Hai.ID_PIN_1,Hai.ID_PIN_9,Hai.ID_SOU_1,Hai.ID_SOU_9,
            Hai.ID_TON, Hai.ID_NAN,Hai.ID_SYA,Hai.ID_PE,Hai.ID_HAKU,Hai.ID_HATSU,Hai.ID_CHUN
        };

        int[] countNumber = {0,0,0,0,0,0,0,0,0,0,0,0,0};

        //手牌をコピーする
        Hai[] checkHais = _tehai.getJyunTehai();

        //手牌のIDを検索する
        for(int i = 0; i < _tehai.getJyunTehai().Length; i++)
        {
            for(int j = 0 ; j < checkId.Length ; j++)
            {
                if(checkHais[i].ID == checkId[j])
                    countNumber[j]++;
            }
        }

        for(int j = 0; j < checkId.Length; j++)
        {
            if(_addHai.ID == checkId[j])
                countNumber[j]++;
        }

        bool atama = false;

        //国士無双が成立しているか調べる(手牌がすべて1.9字牌 すべての１,９字牌を持っている）
        for(int i = 0 ; i < countNumber.Length ; i++)
        {
            //0枚の牌があれば不成立
            if(countNumber[i] == 0)
                return false;
            
            if(countNumber[i] == 2)
                atama = true;
        }

        //条件を満たしていれば成立
        return atama;
    }

    // 悬赏牌 //
    public bool checkDora()
    {
        int doraCount = 0;

        /// all doras
        Hai[] omoteDoras = AgariParam.getOmoteDoraHais() ?? new Hai[0];
        Hai[] uraDoras = AgariParam.getUraDoraHais() ?? new Hai[0];

        Hai[] allDoraHais = new Hai[omoteDoras.Length + uraDoras.Length];
        for( int i = 0; i < omoteDoras.Length; i++)
        {
            allDoraHais[i] = omoteDoras[i];
        }
        for( int i = 0; i < uraDoras.Length; i++)
        {
            allDoraHais[omoteDoras.Length + i] = uraDoras[i];
        }

        // 手牌
        Hai[] jyunTehai = _tehai.getJyunTehai();
        for(int i = 0; i < allDoraHais.Length; i++)
        {
            for (int j = 0; j < jyunTehai.Length; j++)
            {
                if( allDoraHais[i].NextHaiID == jyunTehai[j].ID )
                    doraCount++;
            }
        }

        // add Hai
        for(int i = 0; i < allDoraHais.Length; i++)
        {
            if( allDoraHais[i].NextHaiID == _addHai.ID )
            {
                doraCount++;
                break;
            }
        }

        // red dora
        for(int j = 0; j < jyunTehai.Length; j++)
        {
            if( jyunTehai[j].IsRed )
                doraCount++;
        }
        if( _addHai.IsRed )
            doraCount++;

        // 副露
        Fuuro[] fuuros = _tehai.getFuuros();
        for(int i = 0; i < fuuros.Length; i++) 
        {
            switch (fuuros[i].Type)
            {
                case EFuuroType.MinShun:
                {
                    for(int j = 0; j < allDoraHais.Length; j++)
                    {
                        for(int k = 0; k < 3; k++)
                        {
                            if(allDoraHais[j].NextHaiID == fuuros[i].Hais[k].ID)
                            {
                                doraCount += 1;
                                goto MinShun_Loop_End;
                            }
                        }
                    }
                    MinShun_Loop_End:{
                        
                    }
                }
                break;

                case EFuuroType.MinKou:
                {
                    for(int j = 0; j < allDoraHais.Length; j++)
                    {
                        if(allDoraHais[j].NextHaiID == fuuros[i].Hais[0].ID) {
                            doraCount += 3;
                            break;
                        }
                    }
                }
                break;

                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    for(int j = 0; j < allDoraHais.Length; j++)
                    {
                        if(allDoraHais[j].NextHaiID == fuuros[i].Hais[0].ID) {
                            doraCount += 4;
                            break;
                        }
                    }
                }
                break;
            }
        }

        // red dora.
        for(int i = 0; i < fuuros.Length; i++) 
        {
            switch(fuuros[i].Type)
            {
                case EFuuroType.MinShun:
                case EFuuroType.MinKou:
                {
                    for(int j = 0; j < 3; j++)
                    {
                        if(fuuros[i].Hais[j].IsRed)
                            doraCount++;
                    }
                }
                break;

                case EFuuroType.DaiMinKan:
                case EFuuroType.KaKan:
                case EFuuroType.AnKan:
                {
                    for(int j = 0; j < 4; j++)
                    {
                        if(fuuros[i].Hais[j].IsRed)
                            doraCount++;
                    }
                }
                break;
            }
        }

        if(doraCount > 0) {
            _doraCount = doraCount;
            return true;
        }

        return false;
    }


    public static string getNagashiManganYakuName()
    {
        return GetYakuName(48);
    }

    public static string GetYakuName(int yakuID)
    {
        return YakuHandler.GetYakuName(yakuID);
    }
}
