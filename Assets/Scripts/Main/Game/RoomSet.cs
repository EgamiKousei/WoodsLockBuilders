using UnityEngine;

public class RoomSet : MonoBehaviour
{
    private GameObject itemObject=null;
    public static int objNum=0;
    public GameObject CreateMap;
    public static string RoomData=null;

    // Start is called before the first frame update
    private void Start()
    {
        SetRoom();
    }
    void Update()
    {
        // ユーザーの行動情報があったら同期処理を行い、ユーザーの行動情報を初期化
        if (RoomData != null)
        {
            SetRoom();
            RoomData = null;
        }
    }
    private void SetRoom()
    {
        foreach (Transform n in CreateMap.transform)
        {
            Destroy(n.gameObject);
        }
        foreach (var i in PlayerData.PlayMap.Values)
        {
            itemObject = (GameObject)Resources.Load(i.name);
            Vector3 arPoint = new Vector3(i.x, i.y,i.z);
            GameObject obj = Instantiate(itemObject, arPoint, Quaternion.identity);
            obj.name = i.num.ToString();
            Vector3 rotationAngles = new Vector3(i.xr, i.yr, i.zr);
            obj.transform.rotation= Quaternion.Euler(rotationAngles);
            obj.transform.parent = CreateMap.transform;
            if (objNum <= i.num)
                objNum = i.num;
        }
        RoomData = null;
    }
}
