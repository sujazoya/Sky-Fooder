using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class Effects
{
	public GameObject tri_yellow_effect;
	public GameObject tri_blue_effect;
	public GameObject tri_blueDark_effect;
	public GameObject tri_pink_effect;
	public GameObject tri_red_effect;
	public GameObject tri_green_effect;

	public GameObject quad_yellow_effect;
	public GameObject quad_blue_effect;
	public GameObject quad_blueDark_effect;
	public GameObject quad_pink_effect;
	public GameObject quad_red_effect;
	public GameObject quad_green_effect;

	public GameObject coin_effect;

}
public class UndergroundCollision : MonoBehaviour
{
	[SerializeField] Effects effects;
	[SerializeField] GameObject bomb;
	[SerializeField] GameObject afterBlast;
	[SerializeField] GameObject eating;
	[SerializeField] GameObject[] disableOnBlast;

	[SerializeField] Text diemondText;
	[SerializeField] Text coinText;
	[SerializeField] GameObject floatingText;
	private void Awake()
    {
		bomb.SetActive(false);
		afterBlast.SetActive(false);
		eating.SetActive(false);
        for (int i = 0; i < disableOnBlast.Length; i++)
        {
			disableOnBlast[i].SetActive(true);

		}
		UpdateUIText();
	}
	void UpdateUIText()
    {
		diemondText.text = Game.TotalDiemonds.ToString();
		coinText.text    = Game.TotalCoins.ToString();
	}

    void OnTriggerEnter (Collider other)
	{
		//Object or Obstacle is at the bottom of the Hole

		if (!Game.isGameover) {
			string tag = other.tag;
			//------------------------ O B J E C T --------------------------
			if (tag.Equals (Game.itemTag)) { 
				Level.Instance.objectsInScene--;
				UIManager.Instance.UpdateLevelProgress ();
				StartCoroutine(ActEat());
				//Make sure to remove this object from Magnetic field
				Magnet.Instance.RemoveFromMagnetField (other.attachedRigidbody);

				Destroy (other.gameObject);
				Game.foodEaten++;
				//check if win
				if (Level.Instance.objectsInScene == 0) {
					//no more objects to collect (WIN)
					UIManager.Instance.ShowLevelCompletedUI ();
					Level.Instance.PlayWinFx ();

					//Load Next level after 2 seconds
					//Invoke ("NextLevel", 2f);
				}
			}
			if (tag.Equals(Game.powerupTag))
			{
				ActGems_Coin(other.transform);
			}
				//---------------------- O B S T A C L E -----------------------
				if (tag.Equals (Game.blastTag)) {
				Game.isGameover = true;
				bomb.SetActive(true);
				Destroy (other.gameObject);
				for (int i = 0; i < disableOnBlast.Length; i++)
				{
					disableOnBlast[i].SetActive(false);
				}
				afterBlast.SetActive(true);
				DoRandomRotation(afterBlast.transform);
				Game.gameStatus = Game.GameStatus.isGameover;
				//Shake the camera for 1 second
				Camera.main.transform
					.DOShakePosition (2f, .2f, 20, 90f)
					.OnComplete (() => {
					//restart level after shaking complet
					Level.Instance.RestartLevel ();
				});
			}
		}
	}
	void ShowFloatingText()
    {
		Vector3 pos = new Vector3(eating.transform.position.x, eating.transform.position.y+1, eating.transform.position.z);
		GameObject ft = Instantiate(floatingText, pos, Quaternion.identity);
		ft.GetComponent<TextMesh>().text = "1";
		UpdateUIText();
	}

	IEnumerator ActEat()
    {
		eating.SetActive(false);
		yield return new WaitForSeconds(0.02f);
		eating.SetActive(true);
		yield return new WaitForSeconds(1.2f);
		eating.SetActive(false);
	}
	void DoRandomRotation(Transform @object)
    {
		float rt = Random.Range(0, 180);
		@object.rotation =  Quaternion.Euler(90, 0, rt);
		//@object.rotation = Quaternion.AngleAxis(rt, Vector3.forward);		
		//Vector3 eulerRotation = transform.rotation.eulerAngles;
		//@object.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, rt);

	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
			Level.Instance.PlayWinFx();
		}
    }

    void NextLevel ()
	{
		Level.Instance.LoadNextLevel ();
	}
	public void ActGems_Coin(Transform gems)
    {
        if (gems.GetComponent<Coin_Diemonds>().diemondType == Coin_Diemonds.DiemondType.tri)
        {
			if(gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.yellow)
			{
				effects.tri_yellow_effect.SetActive(true);
            }
            else
				if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.red)
			{
				effects.tri_red_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.blue)
			{
				effects.tri_blue_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.blue_dark)
			{
				effects.tri_blueDark_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.pink)
			{
				effects.tri_pink_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.green)
			{
				effects.tri_green_effect.SetActive(true);
			}
			Game.TotalDiemonds += 1;
			ShowFloatingText();
		}
		if (gems.GetComponent<Coin_Diemonds>().diemondType == Coin_Diemonds.DiemondType.quad)
		{
			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.yellow)
			{
				effects.quad_yellow_effect.SetActive(true);
			}
			else
				if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.red)
			{
				effects.quad_red_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.blue)
			{
				effects.quad_blue_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.blue_dark)
			{
				effects.quad_blueDark_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.pink)
			{
				effects.quad_pink_effect.SetActive(true);
			}
			else

			if (gems.GetComponent<Coin_Diemonds>().diemondColour == Coin_Diemonds.DiemondColour.green)
			{
				effects.quad_green_effect.SetActive(true);
			}
			Game.TotalDiemonds += 1;
			ShowFloatingText();
		}
		if (gems.GetComponent<Coin_Diemonds>().diemondType == Coin_Diemonds.DiemondType.coin)
        {
			Game.TotalCoins += 1;
			effects.coin_effect.SetActive(true);
			ShowFloatingText();
		}
	}
		
}
