using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHanler : MonoBehaviour
{
	public LevelItem[] Levels;
    public GameObject[] items;	
	[SerializeField] GameObject[] powerupItems;
	[SerializeField] GameObject[] blastItems;
	public Transform posParent;
	public List <Transform> allPoses;
	UIManager uIManager;

	[HideInInspector] public List<GameObject> createdItems;
	// Start is called before the first frame update
	Level levelscript;
	void Start()
    {
		levelscript = GetComponent<Level>();
		//itemIndex = Random.Range(0, items.Length);
		foreach (Transform tr in posParent)
		{
			allPoses.Add(tr);
		}
		//ActivateLevel(0);
		uIManager = FindObjectOfType<UIManager>();
	}
	public void Play()
    {
        if (Game.CurrentLevel == 0)
        {
			Game.CurrentLevel = 1;
		}
		ActivateLevel(Game.CurrentLevel);
		Game.gameStatus = Game.GameStatus.isPlaying;
	}
	public void ActivateLevel(int levelIndex)
    {
		Game.CurrentLevel = levelIndex;
		Game.gameStatus = Game.GameStatus.isPlaying;
		uIManager.ShowUI(Game.HUD);
		createdItems.Clear();
		CreateItems      (Levels[levelIndex]);
		CreatePowerUps   (Levels[levelIndex]);
		CreateBlasts     (Levels[levelIndex]);
		levelscript.CountObjects();
		StartCoroutine(StartMusic());
		StartCoroutine(uIManager.ActiveBack(false,1));
	} 
	IEnumerator StartMusic()
    {
		yield return new WaitForSeconds(2);
		MusicManager.PlayMusic("Music1");	
		
    }


	 void CreateItems(LevelItem level)
    {
		int[] count = level.itemIndex;
		int index;
        for (int i = 0; i < count.Length; i++)
        {
			index = Random.Range(0, items.Length);
			GameObject newObj = Instantiate(items[index], allPoses[count[i]].position, Quaternion.identity, transform);
			newObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			newObj.tag =Game. itemTag;
   //         if (newObj.transform.GetComponent<BoxCollider>())
   //         {
			//	newObj.AddComponent<BoxCollider>();

			//}
			createdItems.Add(newObj);
		}

    }	
	 void CreatePowerUps(LevelItem level)
	{
		int[] count = level.powerupsIndex;
		int index;
		for (int i = 0; i < count.Length; i++)
        {
			index = Random.Range(0, powerupItems.Length);
			GameObject newObj = Instantiate(powerupItems[index], allPoses[count[i]].position, powerupItems[index].transform.rotation, transform);
			if(newObj.transform.GetComponent<Coin_Diemonds>().diemondType== Coin_Diemonds.DiemondType.coin)
            {
				newObj.transform.localScale = Vector3.one;
				newObj.transform.position=   new Vector3(newObj.transform.position.x, newObj.transform.position.y + 5, newObj.transform.position.z);
			}
            else
            {
				newObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			}			
			newObj.tag = Game.powerupTag;
			createdItems.Add(newObj);
		}
    }
	
	 void CreateBlasts(LevelItem level)
	{
		int[] count = level.blastIndex;
		int index;
		for (int i = 0; i < count.Length; i++)
		{
			index = Random.Range(0, blastItems.Length);
			GameObject newObj = Instantiate(blastItems[index], allPoses[count[i]].position, blastItems[index].transform.rotation, transform);
			//newObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			newObj.tag = Game.blastTag;
			createdItems.Add(newObj);
		}

	}

	
}
