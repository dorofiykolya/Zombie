using UnityEngine;
using System.Collections;

public class Preloader : MonoBehaviour 
{
	public GUIStyle preloder;
	private AsyncOperation loadOp;
	private string[] hints = new string[]{"Чтобы пополнить запас мозгов, догоните любого человека",
										"С помощью Валеры, можно собрать группу до 6 зомби",
										"Тоня увеличивает текущий множитель очков",
										"У Сани ограниченное время жизни, помните это",
										"Бонусы встречаются редко, если в группе нет доктора Белова",
										"Сержант Петров служит щитом для вашего отряда",
										"Выполнив 3 миссии, вы увеличите множитель очков на 1"};
	private int hintId;

	void Awake()
	{
		preloder.fontSize = (int)(0.05f * Screen.width);
		preloder.padding.left = preloder.padding.right = (int)(Screen.height * 0.75f - Screen.width) / 2;
		hintId = Random.Range (0, hints.Length - 1);

		StartCoroutine (LoadLevel());
	}

	private IEnumerator LoadLevel()
	{
		yield return new WaitForSeconds (3);
		loadOp = Application.LoadLevelAsync("Game");
	}

	void OnGUI()
	{
		GUI.Box (new Rect ((Screen.width - Screen.height * 0.75f) / 2, 0, Screen.height * 0.75f, Screen.height), hints[hintId], preloder);
	}
}
