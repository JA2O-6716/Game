using UnityEngine;

public class RePosition : MonoBehaviour
{
    Collider2D _coll;
    private void Awake()
    {
        _coll = GetComponent<Collider2D>();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area")) // Area 태그가 아니면 멈추는 로직
        { return; }
        Vector3 playerPos = GameManager._instance.player.transform.position;
        Vector3 myPos = transform.position;
        

        switch (transform.tag)
        {
            case "Ground":
                float diffx = playerPos.x - myPos.x;
                float diffy = playerPos.y - myPos.y;
                float dirX = diffx < 0 ? -1 : 1;
                float dirY = diffy < 0 ? -1 : 1;

                diffx = Mathf.Abs(diffx);
                diffy = Mathf.Abs(diffy);

                if (diffx >= diffy)
                    transform.Translate(Vector3.right * dirX * 40);
                else if (diffx <= diffy)
                    transform.Translate(Vector3.up * dirY * 40);
                break;
            case "Enemy":
                if(_coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3),0);

                    transform.Translate(ran + dist * 2); 
                }
                break;
        }
    }   
}
