using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] _spawnPoint;
    public SpawnData[] _spawnData;
    public float levelTime;

    int Level; //int형 이므로 FloorInt사용
    float _timer;
    float[] spawnTime;

    private void Awake()
    {
        _spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager._instance.MaxGameTime / _spawnData.Length;
    }

    void Update()
    {
        if (!GameManager._instance.islive)
        { return; }

        _timer += Time.deltaTime;
        Level = Mathf.Min(Mathf.FloorToInt(GameManager._instance.GameTime / levelTime),_spawnData.Length - 1); //FloorToInt 소수점자리를 삭제

        if (_timer > _spawnData[Level].spawnTime)
        {
            _timer = 0;
            Spawn();
            
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager._instance.pool.Get(0); 
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(_spawnData[Level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}