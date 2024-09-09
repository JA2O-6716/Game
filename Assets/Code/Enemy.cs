using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speed;
    public float _health;
    public float _maxHealth; 

    public RuntimeAnimatorController[] _animCon;
    public Rigidbody2D _target;

    bool _isLive;

    Rigidbody2D _rigid;
    Collider2D _coll;
    Animator _anim;
    SpriteRenderer _spriter;
    WaitForFixedUpdate _wait;

    private void Awake() //대부분 초기화 할때 사용
    {
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
        _spriter = GetComponent<SpriteRenderer>();
        _wait = new WaitForFixedUpdate();
    }

    void FixedUpdate() //물리적인 프레임마다 호출
    {
        if (!GameManager._instance.islive)
        { return; }

        if (!_isLive || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        Vector2 dirVec = _target.position - _rigid.position;
       Vector2 nextVec = dirVec.normalized * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate() // 프레임마다 호출
    {
        if (!GameManager._instance.islive)
        { return; }

        if (!_isLive)
        {
            return;
        }

        _spriter.flipX = _target.position.x < _rigid.position.x; //몬스터의 움직일떄마다 좌우
    }

    private void OnEnable() //활성화 될 때마다 호출되는 함수입니다. Awake/Start와 달리 활성화 될 때마다
    {
        _target = GameManager._instance.player.GetComponent<Rigidbody2D>();
        _isLive = true;
        _coll.enabled = true;
        _rigid.simulated = true; //물리일때는 simulated = false 가 비활성화
        _spriter.sortingOrder = 2;
        _anim.SetBool("Dead", false);
        _health = _maxHealth;
    }

    public void Init(SpawnData spawnData)
    {
        _anim.runtimeAnimatorController = _animCon[spawnData.spriteType];
        _speed = spawnData.speed;
        _maxHealth = spawnData.health;
        _health = spawnData.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !_isLive) // 시체 공격했을때 로직실행 안되도록
        {
            return;
        }

        _health -= collision.GetComponent<Bullet>()._damage;
        StartCoroutine(KnockBack());

        if(_health > 0)
        {
            _anim.SetTrigger("Hit");
            _isLive = true;
            AudioManager.instance.playSfx(AudioManager.Sfx.Hit);
        }
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false; //물리일때는 simulated가 비활성화
            _spriter.sortingOrder = 1;
            _anim.SetBool("Dead", true);
            GameManager._instance.kill++;
            GameManager._instance.GetExp();

            if (GameManager._instance.islive)
            { AudioManager.instance.playSfx(AudioManager.Sfx.Dead); }
            
        }
    }

     IEnumerator KnockBack()
    {
        yield return _wait; // 다음 하나의 물리 프레임을 딜레이
        Vector3 playerPos = GameManager._instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3,ForceMode2D.Impulse); // 노멀라이즈 사용 = 위치를 1로
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
