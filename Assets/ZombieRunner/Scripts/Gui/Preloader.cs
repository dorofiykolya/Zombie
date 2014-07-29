using UnityEngine;
using System.Collections;

public class Preloader : MonoBehaviour 
{
	public GUIStyle preloder;
	public GUIStyle back;
	public GUIStyle front;
	private AsyncOperation loadOp;
	private string[] hints = new string[]{"Чтобы пополнить запас мозгов, догоните любого человека",
										"С помощью менеджера, можно собрать группу до 6 зомби",
										"Домохозяйка увеличивает текущий множитель очков",
										"У толстяка ограниченное время жизни, помните это",
										"Бонусы встречаются редко, если в группе нет профессора",
										"Сержант служит щитом для вашего отряда",
										"Выполнив 3 миссии, вы увеличите множитель очков на 1",
										"Толстяк удваивает собираемые мозги"};
	private int hintId;

	void Awake()
	{
		preloder.fontSize = (int)(0.05f * Screen.width);
		preloder.padding.left = preloder.padding.right = (int)(Screen.height * 0.75f - Screen.width) / 2;
		hintId = Random.Range (0, hints.Length - 1);

		loadOp = Application.LoadLevelAsync("Game");
	}

	void OnGUI()
	{
		GUI.Box (new Rect ((Screen.width - Screen.height * 0.75f) / 2, 0, Screen.height * 0.75f, Screen.height), hints[hintId], preloder);
		GUI.Box (new Rect (Screen.width * 0.2f, Screen.height * 0.85f, Screen.width * 0.6f, Screen.height * 0.05f), "", back);
		if(loadOp != null) GUI.Box (new Rect (Screen.width * 0.2f, Screen.height * 0.85f, (Screen.width * 0.6f) * loadOp.progress, Screen.height * 0.05f), "", front);
	}
}
