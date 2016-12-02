using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerUI : UIObject 
{
    public EKaze DeskKaze = EKaze.Ton;

    private TehaiUI tehai; // 手牌.
    private YamaUI yama;   // 牌山.
    private HouUI hou;     // 河.
    private PlayerInfoUI playerInfo; // 玩家信息.
    private FuuroUI fuuro; //副露.

    private int panelDepth = 0;


    void Start () {
        Init();
    }

    public override void Init() {
        if( isInit == false ) {
            tehai = transform.Find("Tehai").GetComponent<TehaiUI>();
            yama = transform.Find("Yama").GetComponent<YamaUI>();
            hou = transform.Find("Hou").GetComponent<HouUI>();
            fuuro = transform.Find("Fuuro").GetComponent<FuuroUI>();
            playerInfo = transform.Find("Info").GetComponent<PlayerInfoUI>();

            tehai.Init();
            yama.Init();
            hou.Init();
            fuuro.Init();
            playerInfo.Init();

            isInit = true;
        }
    }

    public override void Clear() {        
        if(!isInit) Init();

        tehai.Clear();
        yama.Clear();
        hou.Clear();   
        fuuro.Clear();
        playerInfo.Clear();
    }

    public override void SetParentPanelDepth(int depth) { 
        if( panelDepth != depth ){
            tehai.SetParentPanelDepth( depth );
            yama.SetParentPanelDepth( depth );
            hou.SetParentPanelDepth( depth );
            fuuro.SetParentPanelDepth( depth );
            playerInfo.SetParentPanelDepth( depth );

            panelDepth = depth;
        }
    }

    public TehaiUI getTehaiUI() {
        return tehai;
    }
    public YamaUI getYamaUI() {
        return yama;
    }
    public HouUI getHouUI() {
        return hou;
    }
    public FuuroUI getFuuroUI() {
        return fuuro;
    }
    public PlayerInfoUI getPlayerInfoUI() {
        return playerInfo;
    }


    // 手牌.
    public void SetTehai(Hai[] hais) {
        tehai.SetTehai(hais);
    }
    public void PickHai( Hai hai, int newIndex = -1, bool isShow = false ) {
        tehai.AddHai(hai, newIndex, isShow);
    }
    public void SetAllHaisVisiable( bool visiable ) {
        tehai.SetAllHaisVisiable(visiable);
    }

    // 河. 打牌.
    public void SuteHai(Hai hai) {
        hou.AddHai(hai);
    }

    public void Reach(bool reach = true) {
        hou.SetReach(reach);
        playerInfo.SetReach(reach);
    }

    // 牌山.
    public void SetYamaHais(Dictionary<int, Hai> yamaHais, int start, int end) {
        yama.SetYamaHais(yamaHais, start, end);
    }
    public void PickUpYamaHai(int index) {
        yama.PickUp(index);
    }
    public void ShowYamaHai(int index) {
        yama.ShowHai(index);
    }
    public void HideYamaHai(int index) {
        yama.HideHai(index);
    }
    public void ShowAllYamaHais() {
        yama.ShowAllYamaHais();
    }
    public void HideAllYamaHais() {
        yama.HideAllYamaHais();
    }

    // 副露.
    public void UpdateFuuro(Fuuro[] fuuros) {
        fuuro.UpdateFuuro(fuuros);
    }

    // player info.
    public void SetKaze(EKaze kaze) {
        playerInfo.SetKaze(kaze);
    }
    public void SetTenbou(int point) {
        playerInfo.SetTenbou(point);
    }
    public void SetOyaKaze(bool isOya) {
        playerInfo.SetOyaKaze(isOya); 
    }



    public static MahjongPai CreateMahjongPai(Transform parent, Vector3 localPos, Hai info, bool isShow = true) {
        if( Hai.IsValidHai(info) == false ){
            Debug.LogError("PlayerUI: Invalid hai for ID == " + info.ID);
            return null;
        }

        GameObject newInst = ResManager.CreateMahjongObject();
        newInst.transform.parent = parent;
        newInst.transform.localScale = Vector3.one;
        newInst.transform.localPosition = localPos;

        // set component info.
        MahjongPai pai = newInst.GetComponent<MahjongPai>();
        if( pai == null ) {
            pai = newInst.AddComponent<MahjongPai>();
        }

        pai.SetInfo(info);
        pai.Init();
        pai.UpdateImage();

        if( isShow ) {
            pai.Show();
        }
        else {
            pai.Hide();
        }

        return pai;
    }

    public static float GetMahjongRange(bool shouldSetLand) {
        float ret = 0;

        if( shouldSetLand ) { // set landscape left.
            ret = MahjongPai.Height;
        }
        else {  // set portrias.
            ret = MahjongPai.Width;
        }

        return ret;
    }
}
