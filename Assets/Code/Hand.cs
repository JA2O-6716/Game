using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool _isLeft;
    public SpriteRenderer Spriter;

    SpriteRenderer _player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosRevers = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotRevers = Quaternion.Euler(0, 0, -135);
    
    private void Awake()
    {
        _player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isRevers = _player.flipX;

        if (_isLeft) 
        {
            transform.localRotation = isRevers ? leftRotRevers : leftRot;
            Spriter.flipY = isRevers;
            Spriter.sortingOrder = isRevers ? 4 : 6; //레이어
        }
        else    
        {
            transform.localPosition = isRevers ? rightPosRevers : rightPos;
            Spriter.flipX = isRevers;
            Spriter.sortingOrder = isRevers ? 6 : 4; //레이어
        }
    }
}
