using UnityEngine;
using System.Collections;

namespace Runner
{
	public class CurrencyPickUp : ComponentManager 
	{
		void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Currency"))
			{
				CurrencyManager.goldCount += (1 + Player.GetGoldBonus());
				other.transform.GetComponent<ObstacleCurrency>().isPickUp = true;
				other.gameObject.collider.enabled = false;
			}
		}
	}
}
