using UnityEngine;
using System.Collections.Generic;


public class ResManager 
{
    private static Dictionary<string, string> StringTable;

    public static void LoadStringTable(bool useJapanese = false)
    {
        if( StringTable == null ){
            StringTable = new Dictionary<string, string>();

            string fileName = "string_cn";
            if(useJapanese) fileName = "string_jp";

            TextAsset ta = Resources.Load<TextAsset>("Table/" + fileName);

            List<string> valueList = new List<string>();

            List<object> stringList = (List<object>)MiniJSON.Deserialize(ta.text);
            for( int i = 0; i < stringList.Count; i++ )
            {
                Dictionary<string, object> kv = (Dictionary<string, object>)stringList[i];

                foreach(var kvs in kv)
                    valueList.Add(kvs.Value.ToString());
            }

            for( int i = 0; i < valueList.Count-1; i += 2 )
                StringTable.Add( valueList[i], valueList[i+1] );

            //PrintStringTable();
        }
    }

    static void PrintStringTable()
    {
        Debug.Log("---------------- String Table -------------");
        foreach( var kvs in StringTable )
        {
            Debug.Log( kvs.Value );
        }
    }

    public static string getString(string key)
    {
        LoadStringTable();

        string value;
        if( StringTable.TryGetValue(key, out value) ){
            return value;
        }
        return key;
    }


    private static List<GameObject> _mahjongPaiPool = new List<GameObject>();
    private static Transform poolRoot = null;
    private static GameObject mahjongPaiPrefab = null;


    public static void SetPoolRoot(Transform root)
    {
        poolRoot = root;
    }

    public static void ClearMahjongPaiPool()
    {
        for(int i = 0; i < _mahjongPaiPool.Count; i++)
        {
            if(_mahjongPaiPool[i] != null)
                GameObject.Destroy( _mahjongPaiPool[i] );
        }
        _mahjongPaiPool.Clear();
    }

    public static GameObject CreateMahjongObject()
    {
        if(_mahjongPaiPool.Count > 0)
        {
            GameObject pai = _mahjongPaiPool[0].gameObject;
            pai.SetActive(true);

            _mahjongPaiPool.RemoveAt(0);

            return pai;
        }
        else{
            if( mahjongPaiPrefab == null )
                mahjongPaiPrefab = Resources.Load<GameObject>("Prefabs/Mahjong/MahjongPai");
            return Object.Instantiate(mahjongPaiPrefab) as GameObject;
        }
    }

    public static bool CollectMahjongPai(MahjongPai pai)
    {
        if( pai == null )
            return false;

        pai.Clear();

        if(poolRoot == null)
            poolRoot = new GameObject("MahjongPoolRoot").transform;

        pai.transform.parent = poolRoot;

        pai.gameObject.SetActive(false);
        _mahjongPaiPool.Add( pai.gameObject );

        return true;
    }


    public static GameObject CreatePlayerUIObject()
    {
        return Object.Instantiate(Resources.Load<GameObject>("Prefabs/Mahjong/PlayerUI")) as GameObject;
    }

    public static string getMahjongSpriteName(int kind, int num) {
        string head = "w";
        if(kind == Hai.KIND_WAN){
            head = "w";
        }
        else if( kind == Hai.KIND_PIN) {
            head = "tong";
        }
        else if( kind == Hai.KIND_SOU ) {
            head = "tiao";
        }
        else if( kind == Hai.KIND_FON ) {
            head = "c";
        }
        else if( kind == Hai.KIND_SANGEN ) {
            head = "c";
            num += 4;
        }
        else {
            Debug.LogError("Unknown mahjong kind of " + kind);
        }

        return head + "_" + num;
    }

}
