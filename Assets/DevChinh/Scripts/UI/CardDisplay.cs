using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Animator animator;
    public Button btn;
    public Text Coint;
    private void Start()
    {
        Coint.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        btn.onClick.AddListener(() =>
        {
            btn.enabled = false;
            FindObjectOfType<ButtonController>().State(card.id);
            animator.SetTrigger(Cards.Open.ToString());
        });
    }

    public void randomValue()
    {
        int value = Random.Range(40, 100);
        Coint.text =  value + " COINT";
        Prefs.COINT += value;
    }
}
