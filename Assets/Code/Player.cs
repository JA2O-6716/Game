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

    private void Awake()//어웨이크에서 값을 초기화
    {  
        //컴퍼넌트를 받는함수
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

     // pc 이동 할때 사용
    //void Update()
    //{
    //    if (!GameManager._instance.islive)
    //    { return; }
    //    //캐릭터 이동 방향
    //    _inputVec.x = Input.GetAxisRaw("Horizontal");
    //    _inputVec.y = Input.GetAxisRaw("Vertical");
    //}

    // 모바일 이동 할때 사용
    private void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate() //물리적 연산을 사용
    {
        if (!GameManager._instance.islive)
        { return; }
        // pc를 이용할때는 _nextVec 의 normalized 넣기
        Vector2 _nextVec = _inputVec * _speed * Time.fixedDeltaTime; // 대각선을 1만큼 움직임 * 속도 * 프레임마다 차이를 줄인다
        //3. 위치 이동
        _rigid.MovePosition(_rigid.position + _nextVec); //현재 캐릭의 (물리)위치 + 백터의 방향
    }

    private void LateUpdate() // 1프레임이 종료 되기 전 실행되는 생명주기 함수
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
