using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public int _per;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float _damage, int _per , Vector3 _dir) //초기화 하는 함수
    {
        this._damage = _damage;
        this._per = _per;

        if (_per >= 0)
        {
            _rb.velocity = _dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") ||  _per == -100) 
        {
            return;
        }

        _per--;

        if(_per < 0)
        {
            _rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || _per == -100)
        {
            return;
        }

        gameObject.SetActive(false);
    }
}
