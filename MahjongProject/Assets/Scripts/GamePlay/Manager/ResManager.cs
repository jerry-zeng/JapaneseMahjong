using UnityEngine;
using System.Collections.Generic;


public class ResManager 
{
    public static bool UseJapanese = false;

    private static Dictionary<string, string> StringTable;

    public static void LoadStringTable()
    {
        if( StringTable == null ){
            StringTable = new Dictionary<string, string>();

            string fileName = "string_cn";
            if(UseJapanese) fileName = "string_jp";

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

    public static GameObject CreateMahjongObject() {
        return Object.Instantiate(Resources.Load<GameObject>("GameObject/Prefabs/Mahjong/MahjongPai")) as GameObject;
    }


    public static MahjongPai getMahjongObject() {
        return null;
    }
    public static bool collectMahjongObject(MahjongPai obj) {
        if( obj == null )
            return false;

        obj.transform.parent = null;
        GameObject.Destroy(obj.gameObject);

        return true;
    }

    public static GameObject CreatePlayerUIObject() {
        return Object.Instantiate(Resources.Load<GameObject>("GameObject/Prefabs/Mahjong/PlayerUI")) as GameObject;
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
