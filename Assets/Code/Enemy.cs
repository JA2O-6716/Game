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

    private void Awake() //��κ� �ʱ�ȭ �Ҷ� ���
    {
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
        _spriter = GetComponent<SpriteRenderer>();
        _wait = new WaitForFixedUpdate();
    }

    void FixedUpdate() //�������� �����Ӹ��� ȣ��
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

    private void LateUpdate() // �����Ӹ��� ȣ��
    {
        if (!GameManager._instance.islive)
        { return; }

        if (!_isLive)
        {
            return;
        }

        _spriter.flipX = _target.position.x < _rigid.position.x; //������ �����ϋ����� �¿�
    }

    private void OnEnable() //Ȱ��ȭ �� ������ ȣ��Ǵ� �Լ��Դϴ�. Awake/Start�� �޸� Ȱ��ȭ �� ������
    {
        _target = GameManager._instance.player.GetComponent<Rigidbody2D>();
        _isLive = true;
        _coll.enabled = true;
        _rigid.simulated = true; //�����϶��� simulated = false �� ��Ȱ��ȭ
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
        if(!collision.CompareTag("Bullet") || !_isLive) // ��ü ���������� �������� �ȵǵ���
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
            _rigid.simulated = false; //�����϶��� simulated�� ��Ȱ��ȭ
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
        yield return _wait; // ���� �ϳ��� ���� �������� ������
        Vector3 playerPos = GameManager._instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3,ForceMode2D.Impulse); // ��ֶ����� ��� = ��ġ�� 1��
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
