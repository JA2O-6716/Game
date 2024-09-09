using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int _id;
    public int _prefabId;
    public float _damage;
    public int _count;
    public float _speed;

    float _timer;
    Player _player;

    private void Awake()
    {
        _player = GameManager._instance.player;
    }

    private void Update()
    {
        if (!GameManager._instance.islive)
        { return; }

        switch (_id)
        {
            case 0:
                transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
                break;
            default:
                _timer += Time.deltaTime;
                
                if (_timer > _speed)
                {
                    _timer = 0;
                    Fire();
                }
                break;
        }

        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10,1);
        }
    }
    public void LevelUp(float _damage, int _count)
    {
        this._damage = _damage;
        this._count += _count;

        if(_id == 0)
        {
            Batch();
        }

        _player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon" + data.name;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;
        // Property set
        _id = data.itemId;
        _damage = data.baseDamage * Character.Damage;
        _count = data.baseCount + Character.Count;

        for(int index = 0; index < GameManager._instance.pool._prefabs.Length; index++)
        {
            if(data.projectile == GameManager._instance.pool._prefabs[index])
            {
                _prefabId = index;
                break;
            }
        }

        switch (_id)
        {
            case 0:
                _speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                _speed = 0.3f * Character.WeaponSpeed;
                break;
        }

        //Hand Set
        Hand hand = _player._hands[(int)data.itemType];
        hand.Spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index = 0; index < _count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager._instance.pool.Get(_prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / _count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(_damage, -100, Vector3.zero); //관통력 -100 은 무한
        }

    }

    void Fire()
    {
        if (!_player._scanner._nearestTarget)
        {
            return;
        }

        //방향계산
        Vector3 targetPos = _player._scanner._nearestTarget.transform.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        //위치,회전,전달
        Transform bullet = GameManager._instance.pool.Get( _prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(_damage, _count, dir);

        AudioManager.instance.playSfx(AudioManager.Sfx.Range);
    }
}
