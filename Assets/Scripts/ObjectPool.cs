using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public int maxObject = 30;
    List<GameObject> pool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pool = new List<GameObject>(); // 초기화

        for (int i = 0; i < maxObject; i++) // 맥스 오브젝트 갯수만큼 인스턴시에이트를 해서 풀에다 넣어줌
        {
            GameObject obj = Instantiate(prefab, parent); // 인스턴시에이트에서 프리팹 뒤에 패런트라는 트랜스폼을 달면 패런트의 자식 오브젝트가 된다는 뜻
            obj.SetActive(false); // 유니티에서 액티브한 상태라는 것은 화면에 보이기도 하고, 업데이트가 쭉 불리우기도 한다라는 뜻
            // 셋엑티브를 펄스로 해주면 화면에 보이지도 않고 업데이트가 불리지도 않음, 화면에 보이지 않는 상태에서 오브젝트가 멈춰있게 됨
            pool.Add(obj);
        }

    }

    public GameObject Get()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) // 하이어라키에서 액티브 돼 있냐 물어봄, 액티브되어있지 않은걸 가져다가 돌려줘야함
            {
                // 액티브 되어있지 않으면
                obj.SetActive(true); // 셋액티브로 액티브 시켜주고
                return obj; // 리턴으로 오브젝트 돌려줌
            }
        }
        return null; // 여기까지 내려온다면 액티브되어 있지 않는 오브젝트가 없다는 뜻, 널을 돌려줌 -> 쓸 수 있는 오브젝트가 없구나
    }
}
