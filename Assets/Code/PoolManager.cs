using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] _prefabs;
    List<GameObject>[] _pools;

    private void Awake()
    {
        _pools = new List<GameObject>[_prefabs.Length];

        for (int index = 0; index < _pools.Length; index++)
        {
            _pools[index] = new List<GameObject>();
        }

    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        
        // 선택한 오브젝트가 비활성환 된 게임 오브젝트에 접근
        foreach (GameObject item in _pools[index]) 
        {
            if(!item.activeSelf) //비활성화 된 오브젝트를 확인
            {
                //값이 있다면 select 비활성화 오브젝트 변수 할당
                select = item; // 발견하면 select 변수에 할당
                select.SetActive(true); //select안에 있는 비활성화된 오브젝트를 활성화 시키는 함수
                break;
            }
        }
        // 못찾았으면
        // 새롭게 생성하고 select 변수에 할당
        if (!select) //(select == null)
        {
            select = Instantiate(_prefabs[index], transform);
            _pools[index].Add(select);
        }

        return select;
    }
}
