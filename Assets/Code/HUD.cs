using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp = 0,
        Level,
        Kill,
        Time,
        Health
    }

    public InfoType _type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (_type) 
        { 
            case InfoType.Exp:
                float curExp = GameManager._instance.exp;
                float maxExp = GameManager._instance.nextExp[Mathf.Min(GameManager._instance.Level, GameManager._instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}",GameManager._instance.Level);
                break;
            
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager._instance.kill);
                break;
            
            case InfoType.Time:
                float remainTime = GameManager._instance.MaxGameTime - GameManager._instance.GameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min,sec);
                break;
            
            case InfoType.Health:
                float curHp = GameManager._instance.health;
                float maxHp = GameManager._instance.maxHealth;
                mySlider.value = curHp / maxHp;
                break;
        
        }


    }
}
