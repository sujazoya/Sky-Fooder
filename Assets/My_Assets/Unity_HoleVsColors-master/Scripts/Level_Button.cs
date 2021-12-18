using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using suja;

[RequireComponent(typeof(Button))]
public class Level_Button : MonoBehaviour
{
    GameObject @lock;
    Text number;
    Button myButton;
    LevelHanler levelHanler;
    int levelNo;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(Game.levelKey + 1))
        {
            string startKey = Game.levelKey + 1;
            PlayerPrefs.SetString(startKey, startKey);
        }
    }
    private void OnEnable()
    {
       
        myButton = GetComponent<Button>();       
        levelHanler = FindObjectOfType<LevelHanler>();
        levelNo = int.Parse(transform.name);
        @lock = transform.Find("lock").gameObject;
        number = transform.Find("no").GetComponent<Text>();
        number.text = levelNo.ToString();
        @lock.SetActive(false);
        myButton.onClick.AddListener(OnClick);
    }
    private void Start()
    {
        StartCoroutine(LockStatus());

    }
    IEnumerator LockStatus()
    {
        yield return new WaitForSeconds(0.5f);
        if (!PlayerPrefs.HasKey(Game.levelKey + levelNo))
        {
            @lock.SetActive(true);
            myButton.interactable = false;
        }
        else
        {
            @lock.SetActive(false);
            myButton.interactable = true;
        }
    }
    void OnClick()
    {
        levelHanler.ActivateLevel(levelNo);
        SoundManager.PlaySfx("button");
    }  
}
