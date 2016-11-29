using UnityEngine;


public class ResManager 
{
    public static  string getString(int id) {
        return id.ToString();
    }
    public static string getString(string key) {
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

    //public static string getMahjongSpriteName(int id) {

    //    return "";
    //}
}
