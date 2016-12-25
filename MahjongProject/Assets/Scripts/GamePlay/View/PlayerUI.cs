using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerUI : UIObject 
{
    private PlayerInfoUI playerInfo; // 玩家信息.
    private TehaiUI tehai; // 手牌.
    private YamaUI yama;   // 牌山.
    private HouUI hou;     // 河.
    private FuuroUI fuuro; //副露.

    private int panelDepth = 0;


    public PlayerInfoUI Info
    {
        get{ return playerInfo; }
    }
    public YamaUI Yama
    {
        get{ return yama; }
    }
    public TehaiUI Tehai
    {
        get{ return tehai; }
    }
    public HouUI Hou
    {
        get{ return hou; }
    }
    public FuuroUI Fuuro
    {
        get{ return fuuro; }
    }


    void Start () {
        Init();
    }

    public override void BindPlayer(Player p)
    {
        Init();

        base.BindPlayer(p);

        tehai.BindPlayer(p);
        yama.BindPlayer(p);
        hou.BindPlayer(p);
        fuuro.BindPlayer(p);
        playerInfo.BindPlayer(p);
    }

    public override void Init() 
    {
        if( isInit == false ) 
        {
            playerInfo = transform.Find("Info").GetComponent<PlayerInfoUI>();
            tehai = transform.Find("Tehai").GetComponent<TehaiUI>();
            yama = transform.Find("Yama").GetComponent<YamaUI>();
            hou = transform.Find("Hou").GetComponent<HouUI>();
            fuuro = transform.Find("Fuuro").GetComponent<FuuroUI>();

            tehai.Init();
            yama.Init();
            hou.Init();
            fuuro.Init();
            playerInfo.Init();

            isInit = true;
        }
    }

    public override void Clear() 
    {        
        if(!isInit) Init();

        tehai.Clear();
        yama.Clear();
        hou.Clear();   
        fuuro.Clear();
        playerInfo.Clear();
    }

    public override void SetParentPanelDepth(int depth) 
    { 
        if( panelDepth != depth ){
            tehai.SetParentPanelDepth( depth );
            yama.SetParentPanelDepth( depth );
            hou.SetParentPanelDepth( depth );
            fuuro.SetParentPanelDepth( depth );
            playerInfo.SetParentPanelDepth( depth );

            panelDepth = depth;
        }
    }


    public void Speak( ECvType content )
    {
        AudioManager.Get().PlaySFX( AudioConfig.GetCVPath(OwnerPlayer.VoiceType, content) );
        //Debug.LogWarning( type.ToString() + "!!!" );
    }


    // 手牌.
    public void SetTehai(Hai[] hais, bool setLastNew = false) {
        tehai.SetTehai(hais);
    }
    public void PickHai( Hai hai, bool newPicked = false, bool isShow = false ) {
        tehai.AddPai(hai, newPicked, isShow);
    }
    public void PickPai( MahjongPai hai, bool newPicked = false, bool isShow = false ) {
        tehai.AddPai(hai, newPicked, isShow);
    }
    public void SuteHai( int index ){
        MahjongPai pai = tehai.SuteHai(index);
        AddSuteHai( pai );
    }
    public void SortTehai(Hai[] hais, float delay, bool setLastNew = false){
        tehai.SortTehai(hais, delay, setLastNew);
    }
    public void SetTehaiVisiable( bool visiable ) {
        tehai.SetAllHaisVisiable(visiable);
    }
    public void EnableInput(bool isEnable){
        if(isEnable)
            tehai.EnableInput();
        else
            tehai.DisableInput();
    }
    public void SetTehaiStateColor(bool state){
        tehai.SetEnableStateColor(state);
    }

    // 河. 打牌.
    protected void AddSuteHai(MahjongPai hai) {
        hou.AddHai(hai);
    }
    public void SetTedashi(bool isTedashi = true){
        hou.setTedashi(isTedashi);
    }
    public void Reach(bool reach = true) {
        hou.SetReach(reach);
        playerInfo.SetReach(reach);
    }
    public void SetNaki(bool isNaki = true){
        hou.setNaki(isNaki);
    }
    public void SetShining(bool isShining){
        hou.setShining(isShining);
    }

    // 牌山.
    public void SetYamaHais(Dictionary<int, Hai> yamaHais, int start, int end) {
        yama.SetYamaHais(yamaHais, start, end);
    }
    public MahjongPai PickUpYamaHai(int index) {
        return yama.PickUp(index);
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
    public void SetWareme(int index) { 
        yama.SetWareme(index);
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


    public static void CollectMahjongPai(MahjongPai pai) 
    {
        ResManager.CollectMahjongPai(pai);
    }
    public static MahjongPai CreateMahjongPai(Transform parent, Vector3 localPos, Hai info, bool isShow = true) 
    {
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

}
