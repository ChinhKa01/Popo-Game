using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text Life;
    public bool hasBoss, isAutoTakeCoint;
    public GameObject Dialog;
    public GameObject[] Effect;
    public GameObject[] EffectOfPlayer;
    public string _state;
    public GameObject player;

    //Character
    public GameObject insSkin;
    public DBCharacter DBCharacter;
    public Text nameCharacter;
    private int skinSelected;
    public Text coint,CointIns2,costSkin,Mess;
    public GameObject port,Door;
    public GameObject winPanel, losePanel,panelMess,Arrow;

    public Text txtLife, txtDame, txtSpeed, txtCoint, txtLevel, txtSkin, txtTimeAttack, txtTimeTele;
    public AudioClip gameover, win;

    void Start()
    {
        Prefs.COINT = 500;
        isAutoTakeCoint = false;
        SystemVariable.gameController = this;
        _state = stateOfGame.Pause.ToString();
       /* losePanel.SetActive(false);
        winPanel.SetActive(false);*/
        Mess.text = "";
        costSkin.text = DBCharacter.GetCharacter(Prefs.SKIN).cost + "$";
        if (hasBoss)
        {
            port.SetActive(false);
            Arrow.SetActive(false);
        }
        if (Dialog != null)
        {
            Dialog.SetActive(false);
        }

        if (!PlayerPrefs.HasKey(SystemVariable.SKIN))
        {
            skinSelected = 0;
        }
        else
        {
            skinSelected = Prefs.SKIN;
        }
        coint.text = Prefs.COINT.ToString();
        CointIns2.text = Prefs.COINT.ToString();
        panelMess.SetActive(false);
    }

    private void Update()
    {
        UpdateCharacterInShop(skinSelected);
        UpdateCharacter();
        if (DBCharacter.GetCharacter(skinSelected).cost == 0)
        {
            costSkin.text = "Claim";
        }
        coint.text = Prefs.COINT.ToString();
        CointIns2.text = Prefs.COINT.ToString();
      
        Life.text = Prefs.LIFE.ToString();
        UpdateInfoPlayer();
        winGame();
        EndGame();
        if(_state == stateOfGame.GameOver.ToString())
        {
            losePanel.SetActive(true);
            SystemVariable.audioController.unmuteBG();
            SystemVariable.audioController.Play(gameover);
          
            //SystemVariable.playerController.enabled = false;
            if (hasBoss)
            {
                 SystemVariable.bossController.enabled = false;
            }
            this.enabled = false;
        }
    }

    //Process dialog
    public void OpenDialog() => Dialog.SetActive(true);

    public void winGame()
    {
        if(_state == stateOfGame.Win.ToString())
        {
            winPanel.SetActive(true);
            SystemVariable.audioController.unmuteBG();
            SystemVariable.audioController.Play(win);
            this.enabled = false;
        }
    }

    public void EndGame()
    {
        if (SystemVariable.quantityCurrentHeart <= 0 && Prefs.LIFE > 0)
        {
            SystemVariable.quantityCurrentHeart = 4;
            if(Prefs.LIFE <=0)
            {
                Prefs.LIFE = 0;
            }
            else
            {
                 Prefs.LIFE--;
            }
            foreach (GameObject obj in FindObjectOfType<PlayerController>().listHeart)
            {
                obj.SetActive(true);
            }
        }
        if (SystemVariable.quantityCurrentHeart <= 0 && Prefs.LIFE == 0)
        {
            _state = stateOfGame.GameOver.ToString();
        }
    }

    //Process skin

    public void NextSkin()
    {
        insSkin.GetComponent<Animator>().SetTrigger(stateOfSkin.MoveRight.ToString());
        skinSelected++;
        if(skinSelected >= DBCharacter.CountCharacter())
        {
            skinSelected = 0;
        }
        
        costSkin.text = DBCharacter.GetCharacter(skinSelected).cost + "$";
    }

    public void BackSkin()
    {
        insSkin.GetComponent<Animator>().SetTrigger(stateOfSkin.MoveLeft.ToString());
        skinSelected--;
        if (skinSelected < 0)
        {
            skinSelected = DBCharacter.CountCharacter() - 1;
        }
        
        costSkin.text = DBCharacter.GetCharacter(skinSelected).cost + "$";
    }

    private void UpdateCharacterInShop(int Idxselected)
    {
        character objSelected = DBCharacter.GetCharacter(Idxselected);
        Color colorSkin = new Color(objSelected.SkinColor.r, objSelected.SkinColor.g, objSelected.SkinColor.b);
        //player.GetComponent<SpriteRenderer>().color = colorSkin;
        insSkin.GetComponent<Image>().color = colorSkin;
        nameCharacter.text = objSelected.name;
        nameCharacter.color = colorSkin;
    }

    private void UpdateCharacter()
    {
        character objSelected = DBCharacter.GetCharacter(Prefs.SKIN);
        Color colorSkin = new Color(objSelected.SkinColor.r, objSelected.SkinColor.g, objSelected.SkinColor.b);
        player.GetComponent<SpriteRenderer>().color = colorSkin;
    }

    public void NextScene()
    {
        Prefs.LEVEL++;
        SceneManager.LoadScene(Prefs.LEVEL);
    }

    public void RetryScene()
    {
        SceneManager.LoadScene(Prefs.LEVEL);
    }

    public void BuySkin()
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - DBCharacter.GetCharacter(skinSelected).cost;
        if(cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= DBCharacter.GetCharacter(skinSelected).cost;
            DBCharacter.GetCharacter(skinSelected).cost = 0;
            panelMess.SetActive(false);
            Prefs.SKIN = skinSelected;
        }
    }

    public void AddLife(Text textIns)
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - int.Parse(textIns.text);
        if (cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= int.Parse(textIns.text);
            Prefs.LIFE++;
            panelMess.SetActive(false);
        }
    }

    public void AddPower(Text textIns)
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - int.Parse(textIns.text);
        if (cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= int.Parse(textIns.text);
            Prefs.DAME += 10;
            panelMess.SetActive(false);
        }
    }

    public void AddSpeed(Text textIns)
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - int.Parse(textIns.text);
        if (cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= int.Parse(textIns.text);
            Prefs.SPEED += 1;
            panelMess.SetActive(false);
        }
    }

    public void ReduceTimeAttack(Text textIns)
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - int.Parse(textIns.text);
        if (cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= int.Parse(textIns.text);
            Prefs.TIMEATTACK -= 0.2f;
            panelMess.SetActive(false);
        }
    }

    public void ReduceTimeTele(Text textIns)
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - int.Parse(textIns.text);
        if (cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= int.Parse(textIns.text);
            Prefs.TIMETELE -= 0.2f;
            panelMess.SetActive(false);
        }
    }

    public void AutoTakeCoint(Text textIns)
    {
        int cointCurr = Prefs.COINT;
        int cal = cointCurr - int.Parse(textIns.text);
        if (cal < 0)
        {
            Mess.text = "Không đủ vàng!";
            panelMess.SetActive(true);
        }
        else
        {
            Prefs.COINT -= int.Parse(textIns.text);
            isAutoTakeCoint = true;
            panelMess.SetActive(false);
        }
    }

    public void UpdateInfoPlayer()
    {
        txtLife.text = "Mạng: " + Prefs.LIFE;
        txtDame.text = "Sức mạnh: " + Prefs.DAME;
        txtSpeed.text = "Tốc độ: " + Prefs.SPEED;
        if(Prefs.TIMEATTACK <= 0)
        {
            txtTimeAttack.text = "TG tấn công: 0";
        }
        else
        {
            txtTimeAttack.text = "TG tấn công: " + Prefs.TIMEATTACK;
        }

        if (Prefs.TIMETELE <= 0)
        {
            txtTimeTele.text = "TG dịch chuyển: 0";
        }
        else
        {
            txtTimeTele.text = "TG dịch chuyển: " + Prefs.TIMETELE;
        }
             
        txtCoint.text = "Vàng: " + Prefs.COINT;
        txtSkin.text = "Trang phục: " + DBCharacter.GetCharacter(Prefs.SKIN).name;
        txtLevel.text = "Level: " + Prefs.LEVEL;
    }

    public void Quite() => Application.Quit();

    public void setTimeScaleOn() => Time.timeScale = 1;
    public void setTimeScaleOff() => Time.timeScale = 0;

    public void resetGame()
    {
        PlayerPrefs.DeleteAll();
        Prefs.LEVEL++;
        SceneManager.LoadScene(Prefs.LEVEL);
        setTimeScaleOn();
    }
}
