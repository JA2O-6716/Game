using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 _inputVec;
    public float _speed;
    public Scanner _scanner;
    public Hand[] _hands;
    public RuntimeAnimatorController[] _animCon;

    Rigidbody2D _rigid;
    SpriteRenderer _SpriteRender;
    Animator _Animator;

    private void Awake()//�����ũ���� ���� �ʱ�ȭ
    {  
        //���۳�Ʈ�� �޴��Լ�
        _rigid = GetComponent<Rigidbody2D>();
        _SpriteRender = GetComponent<SpriteRenderer>();
        _Animator = GetComponent<Animator>();
        _scanner = GetComponent<Scanner>();
        _hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        _speed *= Character.Speed;
        _Animator.runtimeAnimatorController = _animCon[GameManager._instance.playerId];
    }

     // pc �̵� �Ҷ� ���
    //void Update()
    //{
    //    if (!GameManager._instance.islive)
    //    { return; }
    //    //ĳ���� �̵� ����
    //    _inputVec.x = Input.GetAxisRaw("Horizontal");
    //    _inputVec.y = Input.GetAxisRaw("Vertical");
    //}

    // ����� �̵� �Ҷ� ���
    private void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate() //������ ������ ���
    {
        if (!GameManager._instance.islive)
        { return; }
        // pc�� �̿��Ҷ��� _nextVec �� normalized �ֱ�
        Vector2 _nextVec = _inputVec * _speed * Time.fixedDeltaTime; // �밢���� 1��ŭ ������ * �ӵ� * �����Ӹ��� ���̸� ���δ�
        //3. ��ġ �̵�
        _rigid.MovePosition(_rigid.position + _nextVec); //���� ĳ���� (����)��ġ + ������ ����
    }

    private void LateUpdate() // 1�������� ���� �Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager._instance.islive)
        { return; }
        if (_inputVec.x != 0)
        {
            _SpriteRender.flipX = _inputVec.x < 0; // true,false
        }

        _Animator.SetFloat("Speed", _inputVec.magnitude);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager._instance.islive)
        {
            return;
        }

        GameManager._instance.health -= Time.deltaTime * 10;
        AudioManager.instance.playSfx(AudioManager.Sfx.Hit);
        if (GameManager._instance.health <= 0)
        {
            for(int index = 2; index <transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            _Animator.SetTrigger("Dead");
            GameManager._instance.GameOver();
        }
    }
}
