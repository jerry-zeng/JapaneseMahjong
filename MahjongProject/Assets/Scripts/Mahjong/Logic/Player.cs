﻿using System;

/// <summary>
/// Player.
/// </summary>

public abstract class Player
{
    protected string _name;
    protected PlayerAction _action = new PlayerAction();

    public Player(string name)
    {
        this._name = name;
    }

    public string Name
    {
        get{ return _name; }
    }
    public PlayerAction getAction()
    {
        return _action;
    }
    public int getSutehaiIndex()
    {
        return _action.SutehaiIndex;
    }


    // 手牌
    protected Tehai _tehai = new Tehai();
    public Tehai Tehai
    {
        get{ return _tehai; }
    }

    // 河
    protected Hou _hou = new Hou();
    public Hou Hou
    {
        get{ return _hou; }
    }

    // 自風
    protected EKaze _jikaze;
    public EKaze JiKaze
    {
        get{ return _jikaze; }
        set{ _jikaze = value; }
    }

    // 点棒
    protected int _tenbou;
    public int Tenbou 
    {
        get{ return _tenbou; }
        set{ _tenbou = value; }
    }

    // リーチ
    protected bool _reach;
    public bool IsReach
    {
        get{ return _reach; }
        set{ _reach = value; }
    }

    // ダブルリーチ
    protected bool _doubleReach;
    public bool IsDoubleReach
    {
        get{ return _doubleReach; }
        set{ _doubleReach = value; }
    }

    // 一発
    protected bool _ippatsu;
    public bool IsIppatsu
    {
        get{ return _ippatsu; }
        set{ _ippatsu = value; }
    }

    // 捨牌数
    protected int _suteHaisCount;
    public int SuteHaisCount
    {
        get{ return _suteHaisCount; }
        set{ _suteHaisCount = value; }
    }

    protected CountFormat _countFormat = new CountFormat();
    public CountFormat FormatWorker
    {
        get{ return _countFormat; }
    }


    #region Logic

    public virtual void Init() 
    {
        // 手牌を初期化します。
        _tehai.initialize();

        // 河を初期化します。
        _hou.initialize();

        // リーチを初期化します。
        _reach = false;
        _doubleReach = false;
        _ippatsu = false;

        _suteHaisCount = 0;
    }

    // 点棒を増やします
    public void increaseTenbou(int value)
    {
        Tenbou += value;
    }
    // 点棒を減らします
    public void reduceTenbou(int value)
    {
        Tenbou -= value;
    }

    // 听牌
    public bool isTenpai()
    {
        if( _reach == true )
            return true;

        for( int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++ )
        {
            Hai addHai = new Hai(id);
            FormatWorker.setCounterFormat(_tehai, addHai);

            if( FormatWorker.calculateCombisCount(null) > 0 )
                return true;
        }

        return false;
    }


    public void HaiPai(Hai[] hais)
    {
        for( int i = 0; i < hais.Length; i++ )
        {
            Tehai.addJyunTehai( hais[i] );
        }
    }

    public void PickNewHai(Hai newHai)
    {
        Tehai.addJyunTehai( newHai );
    }

    public void SortTehai()
    {
        Tehai.Sort();
    }

    public void SuteHai()
    {
        
    }

    #endregion

    protected GameAgent MahjongAgent
    {
        get{ return GameAgent.Instance; }
    }

    protected Action<EResponse> _onResponse;
    protected EResponse DoResponse(EResponse response)
    {
        _action.Response = response;

        if(_onResponse != null) _onResponse.Invoke(response);
        return response;
    }

    // playerAction is also the returned parameter.
    public virtual void HandleRequest(ERequest request, EKaze fromPlayerKaze, Hai haiToHandle, Action<EResponse> onResponse)
    {
        this._onResponse = onResponse;

        switch( request )
        {
            case ERequest.Tsumo_OrNot:
            {
                Check_TsumoOrNot(fromPlayerKaze, haiToHandle);
            }
            break;
            case ERequest.Ron_OrNot:
            {
                Check_RonOrNot(fromPlayerKaze, haiToHandle);
            }
            break;
            case ERequest.PonKan_OrNot:
            {
                Check_PonKanOrNot(fromPlayerKaze, haiToHandle);
            }
            break;
            case ERequest.Chii_OrNot:
            {
                Check_ChiiOrNot(fromPlayerKaze, haiToHandle);
            }
            break;
            default:
            {
                DoResponse( EResponse.Nagashi );
            }
            break;
        }
    }

    protected abstract EResponse Check_TsumoOrNot(EKaze fromPlayerKaze, Hai haiToHandle);
    protected abstract EResponse Check_RonOrNot(EKaze fromPlayerKaze, Hai haiToHandle);
    protected abstract EResponse Check_PonKanOrNot(EKaze fromPlayerKaze, Hai haiToHandle);
    protected abstract EResponse Check_ChiiOrNot(EKaze fromPlayerKaze, Hai haiToHandle);


    public abstract bool IsAI { get; }
}