using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using suja;

public class UIManager : MonoBehaviour
{
	#region Singleton class: UIManager

	public static UIManager Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
		OffAllUIObjects();
		levelHanler = FindObjectOfType<LevelHanler>();
		StartCoroutine( ActiveBack(true,0));
	}

	#endregion

	[Header ("Level Progress UI")]
	//sceneOffset: because you may add other scenes like (Main menu, ...)
	//[SerializeField] int sceneOffset;
	[SerializeField] Text nextLevelText;
	[SerializeField] Text currentLevelText;
	[SerializeField] Image progressFillImage;

	[Space]
	[SerializeField] Text levelCompletedText;

	[Space]
	//white fading panel at the start
	[SerializeField] Image fadePanel;

	[SerializeField] List<GameObject> UIObjects;
	LevelHanler levelHanler;
	[SerializeField] GameObject back;
	[SerializeField] Button pauseButton;
	[SerializeField] GameObject transition;
	public IEnumerator ActiveBack(bool value,float wait)
    {
		yield return new WaitForSeconds(wait);
		back.SetActive(value);
    }
	void Start ()
	{
		//reset progress value
		progressFillImage.fillAmount = 0f;

		SetLevelProgressText ();
		if (Game.retryCount == 0)
        {
			ShowUI(Game.Menu);
		}
		pauseButton.onClick.AddListener(OnPause);
	}
	void OffAllUIObjects()
	{
		for (int i = 0; i < UIObjects.Count; i++)
		{
			UIObjects[i].SetActive(false);
		}
	}
	void PlayButtonClip()
    {
		SoundManager.PlaySfx("button");
    }

	public GameObject UIObject(string name)
	{
		int objectIndex = UIObjects.FindIndex(gameObject => string.Equals(name, gameObject.name));
		return UIObjects[objectIndex];
	}
	public void WaitAndShowUI(float wait, string uiName)
	{
		waitTime = wait;
		StartCoroutine(ContineuShowUI(uiName));
	}
	float waitTime;
	public void ShowUI(string uiName)
	{
		StartCoroutine(ContineuShowUI(uiName));
	}
	IEnumerator PlayTransition()
	{
		//SoundManager.PlaySfx("transition");
		transition.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		transition.SetActive(true);
		yield return new WaitForSeconds(2f);
		transition.SetActive(false);

	}
	IEnumerator ContineuShowUI(string uiName)
	{
		yield return new WaitForSeconds(waitTime);
		//SoundManager.PlaySfx("button");
		StartCoroutine(PlayTransition());
		yield return new WaitForSeconds(1f);
		OffAllUIObjects();
		UIObject(uiName).SetActive(true);
		waitTime = 0;
		Button[] allButtons = FindObjectsOfType<Button>();
        if (allButtons.Length > 0)
        {
            for (int i = 0; i < allButtons.Length; i++)
            {
				allButtons[i].onClick.AddListener(PlayButtonClip);
			}
        }
		AdmobAdmanager.Instance.ShowInterstitial();
	}
	void SetLevelProgressText ()
	{
		int level = Game.CurrentLevel;
			//SceneManager.GetActiveScene ().buildIndex + sceneOffset;
		currentLevelText.text = level.ToString ();
		nextLevelText.text = (level + 1).ToString ();
	}

	public void UpdateLevelProgress ()
	{
		float val = 1f - ((float)Level.Instance.objectsInScene / Level.Instance.totalObjects);
		//transition fill (0.4 seconds)
		progressFillImage.DOFillAmount (val, .4f);
	}

	//--------------------------------------
	public void ShowLevelCompletedUI ()
	{
		//fade in the text (from 0 to 1) with 0.6 seconds
		levelCompletedText.DOFade (1f, .6f).From (0f);
	}

	public void Fade ()
	{
		//fade out the panel (from 1 to 0) with 1.3 seconds
		fadePanel.DOFade (0f, 1.3f).From (1f);
	}	
	public void OnGameover()
    {

		ShowUI(Game.Gameover);
		StartCoroutine(Gameover());
		MusicManager.PauseMusic(0.2f); 
	}
	IEnumerator Gameover()
    {

		yield return new WaitForSeconds(1.2f);
		StartCoroutine(ActiveBack(true, 0));
		Game.retryCount=0;
		Text Totall_Food = UIObject(Game.Gameover).transform.Find("Totall_Food").GetComponent<Text>();
	    Text totall_eaten = UIObject(Game.Gameover).transform.Find("totall_eaten").GetComponent<Text>();
		Text diemond_Number = UIObject(Game.Gameover).transform.Find("Diemond_Number").GetComponent<Text>();
		Totall_Food.text = Game.foodCountToEat.ToString();
		totall_eaten.text = Game.foodEaten.ToString();
		diemond_Number.text = Game.TotalDiemonds.ToString();
		Button retryButton=UIObject(Game.Gameover).transform.Find("Retry").GetComponent<Button>();
		retryButton.onClick.AddListener(RetryLevel);
		Button homeButton = UIObject(Game.Gameover).transform.Find("home").GetComponent<Button>();
		homeButton.onClick.AddListener(GoHome);		
	}
	public void OnLevelWon()
	{
		StartCoroutine(ActiveBack(true, 1.5f));
	    WaitAndShowUI(2.0f, Game.GameWin);
		StartCoroutine(LevelWon());
		string levelKey = Game.levelKey + Game.CurrentLevel;
		if (!PlayerPrefs.HasKey(levelKey))
		{			
			PlayerPrefs.SetString(levelKey, levelKey);
		}
	}
	IEnumerator LevelWon()
	{
		yield return new WaitForSeconds(.2f);		
		Game.retryCount = 0;
		Button retryButton = UIObject(Game.GameWin).transform.Find("Retry").GetComponent<Button>();
		retryButton.onClick.AddListener(RetryLevel);
		Button homeButton = UIObject(Game.GameWin).transform.Find("home").GetComponent<Button>();
		homeButton.onClick.AddListener(GoHome);		
	}
	void GoHome()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void RetryLevel()
    {
		Game.retryCount++;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);		
    }
	IEnumerator Retry()
    {
	
		yield return new WaitForSeconds(2);
        //SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        if (Game.retryCount > 0)
        {
			levelHanler.ActivateLevel(Game.CurrentLevel);
		}
		
	}
	void OnResume()
    {
		Game.gameStatus = Game.GameStatus.isPlaying;
		ShowUI(Game.HUD);
		MusicManager.PlayMusic("Music1");
		StartCoroutine(ActiveBack(false, 1));
	}
	void OnPause()
    {
		Game.gameStatus = Game.GameStatus.isPaused;
		ShowUI(Game.Pause);
		MusicManager.PauseMusic(0.2f);
		StartCoroutine(ActiveBack(true, 0.7f));
		StartCoroutine(Pause());
	}
	IEnumerator Pause()
    {
		yield return new WaitForSeconds(1.2F);		
		Button resumeButton = UIObject(Game.Pause).transform.Find("RESUME").GetComponent<Button>();
		resumeButton.onClick.AddListener(OnResume);
	}
	void OnEnable()
	{		
		SceneManager_New.onSceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		StartCoroutine(Retry());
		//Debug.Log("OnSceneLoaded: " + scene.name);
		//Debug.Log(mode);
		
	}
	
	// called when the game is terminated
	void OnDisable()
	{		
		SceneManager_New.onSceneLoaded -= OnSceneLoaded;
	}

}
