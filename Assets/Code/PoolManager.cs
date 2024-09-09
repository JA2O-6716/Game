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
        
        // ������ ������Ʈ�� ��Ȱ��ȯ �� ���� ������Ʈ�� ����
        foreach (GameObject item in _pools[index]) 
        {
            if(!item.activeSelf) //��Ȱ��ȭ �� ������Ʈ�� Ȯ��
            {
                //���� �ִٸ� select ��Ȱ��ȭ ������Ʈ ���� �Ҵ�
                select = item; // �߰��ϸ� select ������ �Ҵ�
                select.SetActive(true); //select�ȿ� �ִ� ��Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ ��Ű�� �Լ�
                break;
            }
        }
        // ��ã������
        // ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select) //(select == null)
        {
            select = Instantiate(_prefabs[index], transform);
            _pools[index].Add(select);
        }

        return select;
    }
}
