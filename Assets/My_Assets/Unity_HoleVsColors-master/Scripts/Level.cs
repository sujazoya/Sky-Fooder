using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
	#region Singleton class: Level

	public static Level Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
		winFx.gameObject.SetActive(false);
	}

	#endregion

	[SerializeField] ParticleSystem winFx;

	[Space]
	//remaining objects
	[HideInInspector] public int objectsInScene;
	//total objects at the beginning
	[HideInInspector] public int totalObjects;

	//the Objects parent
	//[SerializeField] Transform objectsParent;

	[Space]
	[Header ("Materials & Sprites")]
	[SerializeField] Material groundMaterial;
	//[SerializeField] Material objectMaterial;
	//[SerializeField] Material obstacleMaterial;
	//[SerializeField] SpriteRenderer groundBorderSprite;
	[SerializeField] Material groundSideMaterial;
	[SerializeField] Image progressFillImage;

	[SerializeField] SpriteRenderer bgFadeSprite;

	[Space]
	[Header ("Level Colors-------")]
	[Header ("Ground")]
	[SerializeField] Color[] groundColor;
	//[SerializeField] Color bordersColor;
	[SerializeField] Color[] sideColor;

	//[Header ("Objects & Obstacles")]
	//[SerializeField] Color objectColor;
	//[SerializeField] Color obstacleColor;

	[Header ("UI (progress)")]
	[SerializeField] Color[] progressFillColor;

	[Header ("Background")]
	[SerializeField] Color[] cameraColor;
	[SerializeField] Color fadeColor;
	UIManager uIManager;
	 

	void Start ()
	{
		//CountObjects ();
		UpdateLevelColors ();
		uIManager = FindObjectOfType<UIManager>();
	}

	public void CountObjects ()
	{
		//Count collectable white objects
		totalObjects = GameObject.FindGameObjectsWithTag(Game.itemTag).Length;
			//= objectsParent.childCount;
		objectsInScene = totalObjects;
		Game.foodCountToEat = objectsInScene;
	}

	public void PlayWinFx ()
	{
		MusicManager.PauseMusic(0.2f);
		winFx.gameObject.SetActive(true);		
		uIManager.OnLevelWon();
	}

	public void LoadNextLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	public void RestartLevel ()
	{
		 uIManager.OnGameover();
		 //Application.LoadLevel(Application.loadedLevel);
		 //SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	void UpdateLevelColors ()
	{
		int index = Random.Range(0, groundColor.Length);
		groundMaterial.color = groundColor[index];
		groundSideMaterial.color = sideColor[index];
		//groundBorderSprite.color = bordersColor;

		//obstacleMaterial.color = obstacleColor;
		//objectMaterial.color = objectColor;

		Color newColor = progressFillColor[index];
		newColor.a = 1;

		progressFillImage.color = newColor;

		Camera.main.backgroundColor = cameraColor[index];
		bgFadeSprite.color = fadeColor;
	}

	void OnValidate ()
	{
		//This method will exeute whenever you change something of this script in the inspector
		//this method won't be included in the final Build (Editor only)

		UpdateLevelColors ();
	}
}
