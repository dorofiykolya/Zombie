using UnityEngine;
using System.Collections;
namespace Runner
{
    public class PlayerController : ComponentManager
    {

		public Vector3 targetPosition;
		private Vector3 transit;
        public Vector3 TargetPosition
        {
            get
            {
				transit.x = targetPosition.x + offset.x;
				transit.y = targetPosition.y;
				transit.z = targetPosition.z + offset.y;

				return transit;
            }
        }

        public bool isPatientZero = true;
        public BoxCollider colliderBox;
		public SphereCollider magnet { get; private set; }

        public int ID = 0;

		public int[] prices;
		public int[] prefs;

		private string intersectName = "";

		private float fCurrentUpwardVelocity = 0.0f;
		private float fCurrentHeight = 0.0f;
		private float fContactPointY = 0.0f;

		private Transform board;

		public float jumpHeight = 25;
		public float gravity = 500;
		public float speedCoeff = 4;

		private float glideSpeed = 2;

		[HideInInspector]
		public bool bInAir;
		[HideInInspector]
		public bool glideEnable;
		private bool bJumpFlag;
		private bool bInJump;
		[HideInInspector]
		public bool bInDuck;
		private bool bDiveFlag;
		private bool isBridge;
		private Vector3 bridgeHeight;
		private float brigeDistance;

        private AnimationManager am;

        private Vector2 offset = Vector2.zero;

        public float sideScrollSpeed;
        public float slideDuration;
        private float slideTime;

		[HideInInspector]
		public float bornTime;
		private int soldierLife;
		private float reviveTime;

        public float Distance { get { return Player.Distance; } }
		private float speed;
		private float deathSpeed;

		private ParticleSystem particle;
		private bool blink;
		private float blinkTime;

        private float tutorialZombieTime;

        public override void Initialize()
        {
			offset.x = 0;
			offset.y = 0;

            slideTime = 0;

			fContactPointY = 0;

            if (!isPatientZero)
            {
				if(ID == 4)
				{
                    offset.y = Mathf.RoundToInt(Player.currentList[0].transform.position.z) + (Player.GetMaxPlayers() - (Player.currentList.Count - 2)) * 3;
				}
				else if(ID == 2)
				{
					offset.y = Mathf.RoundToInt(Player.currentList[0].transform.position.z) + Player.GetMaxPlayers() * -3;
				}
				else
				{
					bool isPlaced;
					float posZ = Mathf.RoundToInt(Player.currentList[0].transform.position.z);
					for(float j = posZ; j >  posZ - (3 * Player.currentList.Count); j -= 3)
					{
						isPlaced = false;
						for(int i = 0; i < Player.currentList.Count; i++)
						{
							if(Mathf.RoundToInt(Player.currentList[i].transform.position.z) == j)
							{
								isPlaced = true;
								break;
							}
						}
						if(!isPlaced)
						{
							offset.y = j;
							break;
						}
					}
				}
				while(offset.x == 0)
					offset.x = (Random.value < .5? 2 : -2);
            }

			bInAir = false;
			bJumpFlag = false;
			glideEnable = false;
			bInJump = false;
			bInDuck = false;
			bDiveFlag = false;
			isBridge = false;
            tutorialZombieTime = 0f;
			bridgeHeight = Vector3.zero;

			magnet = GameObject.Find ("Player").collider as SphereCollider;
			board = transform.FindChild("Board");

            targetPosition = transform.position;
			targetPosition.x = Waypoint.wayPoints[Waypoint.currentWP].position.x;
            transform.position = TargetPosition;

			am = transform.FindChild ("Player").GetComponent<AnimationManager>();

			if(ID == 2)
			{
				bornTime = Time.timeSinceLevelLoad;
			}
			if(ID == 4)
			{
				soldierLife = prefs[PlayerManager.levels[ID]];
			}
			if(Player.isRevive)
			{
				reviveTime = Time.timeSinceLevelLoad;
			}

			particle = transform.FindChild ("Smoke").GetComponent<ParticleSystem> ();
			particle.Stop ();

			States.OnChanged += OnChanged;
        }

		public override void GameStop ()
		{
			States.OnChanged -= OnChanged;
		}

		
		public void OnChanged(State state)
		{
			if(state == State.GAME)
			{
				if(ID == 2)
				{
					bornTime = Time.timeSinceLevelLoad;
				}
				if(ID == 4)
				{
					soldierLife = prefs[PlayerManager.levels[ID]];
				}
				if(am != null)
					am.run();
			}
		}

        // Update is called once per frame
		void FixedUpdate()
        {
			//falling down if player dies in air
			if(Player.isStop)
			{
				deathFall();
				return;
			}

			//update game speed
            speed = Player.Speed / Player.MinimumSpeed;

            if (bInDuck && slideTime <= Time.timeSinceLevelLoad)
            {
                slideDurationTimer();
            }

			//revive
			if(Player.isRevive && isPatientZero)
			{
				if(Time.timeSinceLevelLoad - reviveTime > 3)
				{
					Player.isRevive = false;
					(am.gameObject.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer).enabled = true;
				}
				else if(Time.timeSinceLevelLoad - blinkTime > 0.1)
				{
					(am.gameObject.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer).enabled = blink;
					blink = !blink;
					blinkTime = Time.timeSinceLevelLoad;
				}
			}

            if (tutorialZombieTime < Time.timeSinceLevelLoad && Time.timeScale == 0.01f && isPatientZero)
            {
                Time.timeScale = 1f;
                TutorialAction.hideTutorial();
            }

			//fatman dies
			if(ID == 2 && Time.timeSinceLevelLoad - bornTime > prefs[PlayerManager.levels[ID]] && !Player.isJumpPowerUp)
			{
				Missions.Dispatch("diewithouttouching", 1);
				onDeath();
			}

			//board flight animation
			if(Player.isJumpPowerUp && isPatientZero)
			{
				am.skate();

				if(Mathf.RoundToInt(transform.position.y) == 3)
				{
					glideEnable = false;
				}

                if (Mathf.RoundToInt(transform.position.y) >= 5)
				{
					glideEnable = true;
				}

				if(glideEnable)
				{
					targetPosition.y = 3;
					transform.position = Vector3.MoveTowards(transform.position, TargetPosition, glideSpeed * Time.fixedDeltaTime * speed);
				}
				else
				{
					targetPosition.y = 5;
					transform.position = Vector3.MoveTowards(transform.position, TargetPosition, glideSpeed * Time.fixedDeltaTime * speed);
				}
			}

			//change waypoint
            if (transform.position.x != TargetPosition.x)
            {
				if(Mathf.Abs(transform.position.x - TargetPosition.x) <= 4)
				{
					Waypoint.transitWP = Waypoint.currentWP;
				}

				targetPosition.y = transform.position.y;
				transform.position = Vector3.MoveTowards(transform.position, TargetPosition, sideScrollSpeed * Time.fixedDeltaTime * speed);
				if(isPatientZero)
				{
					Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.position, 
				                                                          new Vector3(TargetPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z),
				                                                          sideScrollSpeed * Time.fixedDeltaTime * speed);
				}
            }

			//jump
			if(bInAir)
			{
				if(bDiveFlag)
				{
					setCurrentDiveHeight(speed);
				}
				else
				{
					setCurrentJumpHeight(speed);
				}

				targetPosition.y = fCurrentHeight;
				transform.position = new Vector3(transform.position.x, fCurrentHeight, transform.position.z);
			}

			if(bJumpFlag == true)
			{
				bJumpFlag = false;
				bInJump = true;
				bInAir = true;
				fCurrentUpwardVelocity = CalculateJumpVerticalSpeed(jumpHeight + fContactPointY);
				fCurrentHeight = transform.position.y;
			}

			//step on bridge
			if(isBridge)
			{
				var length = Player.Distance - brigeDistance;
				if(length < 100)
				{
					targetPosition.y = Player.isJumpPowerUp ? 12 : 8f;
				}
				else if(length > 100)
				{
					if(length > 400)
					{
						isBridge = false;
						fContactPointY = 0;
						bridgeHeight = Vector3.zero;
						return;
					}

                    targetPosition.y = Player.isJumpPowerUp ? 5 : 0f;
				}

                bridgeHeight = Vector3.MoveTowards(bridgeHeight, TargetPosition, (Player.isJumpPowerUp ? 8 : 5) * Time.fixedDeltaTime * speed);

				if(!bInJump)
				{
					bridgeHeight.x = transform.position.x;
					bridgeHeight.z = transform.position.z;
					transform.position = bridgeHeight;
				}
			}

			if(bInDuck && intersectName == "")
			{
				RaycastHit hit;
				Vector3 dir = transform.TransformDirection(Vector3.up);
				if (Physics.Raycast(transform.position, dir, out hit, 10))
					intersectName = hit.transform.name;
			}

			if(bInAir && intersectName == "")
			{
				RaycastHit hit;
				Vector3 dir = transform.TransformDirection(Vector3.down);
				if (Physics.Raycast(transform.position, dir, out hit, 10))
					intersectName = hit.transform.name;
			}
			
			am.updateSpeed();
        }

		public void Revive()
		{
			am.current = "";
			am.run ();

			reviveTime = Time.timeSinceLevelLoad;

			if(ID == 2)
			{
				bornTime = Time.timeSinceLevelLoad;
			}
			if(ID == 4)
			{
				soldierLife = prefs[PlayerManager.levels[ID]];
			}
		}

		private void deathFall()
		{
			if(transform.position.y != 0)
			{
				targetPosition.y = 0;
				transform.position = Vector3.MoveTowards(transform.position, TargetPosition, sideScrollSpeed * Time.fixedDeltaTime * deathSpeed);
			}
		}

		private void setCurrentJumpHeight(float speed)
		{
			fCurrentUpwardVelocity -= Time.deltaTime * gravity * speed;
			fCurrentHeight += fCurrentUpwardVelocity * Time.fixedDeltaTime * speed;
			
			if(fCurrentHeight < fContactPointY)
			{
				fCurrentHeight = fContactPointY;
				bInAir = false;
				bInJump = false;
				
				if (bDiveFlag)
					return;

				am.run();

				if (!isPatientZero)
					return;
				if(intersectName != "")
				{
					Missions.Dispatch("jumpoverobstacles", 1);
				}
				if(intersectName.ToLower().Contains("taxi"))
				{
					Missions.Dispatch("jumpovertaxi", 1);
				}
				if(intersectName.ToLower().Contains("cone"))
				{
					Missions.Dispatch("jumpovercone", 1);
				}
				if(intersectName.ToLower().Contains("red"))
				{
					Missions.Dispatch("jumpoverredcar", 1);
				}
				if(intersectName.ToLower().Contains("car"))
				{
					Missions.Dispatch("jumpovercars", 1);
				}
				if(intersectName.ToLower().Contains("barrel"))
				{
					Missions.Dispatch("jumpoverbarrel", 1);
				}
				if(intersectName.ToLower().Contains("police"))
				{
					Missions.Dispatch("jumpoverpolice", 1);
				}
			}
		}

		public float CalculateJumpVerticalSpeed(float jumpHeight)
		{
			return Mathf.Sqrt(speedCoeff * jumpHeight * gravity * speed);
		}

		private void setCurrentDiveHeight(float speed)
		{
			fCurrentUpwardVelocity -= Time.fixedDeltaTime * 2000 * speed;
			fCurrentHeight += fCurrentUpwardVelocity * Time.fixedDeltaTime * speed;

			if(fCurrentHeight <= fContactPointY)
			{
				fCurrentHeight = fContactPointY;
				
				bInAir = false;
				bInJump = false;
				
				duckPlayer();
				bDiveFlag = false;
			}
		}

		private void duckPlayer()
		{
			bInDuck = true;

			am.slide();

			Vector3 height = colliderBox.center;
			height.y /= 6;
			colliderBox.center = height;

            slideTime = Time.timeSinceLevelLoad + slideDuration;
		}

		private void slideDurationTimer ()
		{
			bInDuck = false;

			restoreAfterDuck ();
			
			am.run();
		}

		private void restoreAfterDuck()
		{
			Vector3 height = colliderBox.center;
			height.y *= 6;
			colliderBox.center = height;
			
			if (!isPatientZero)
				return;
			if(intersectName.ToLower().Contains("sign"))
			{
				Missions.Dispatch("slideundersign", 1);
			}
			if(intersectName.ToLower().Contains("bus"))
			{
				Missions.Dispatch("slideunderbus", 1);
			}
			if(intersectName.ToLower().Contains("sign") && ID == 2)
			{
				Missions.Dispatch("slideundersignbobby", 1);
			}
			if(intersectName.ToLower().Contains("barrel"))
			{
				Missions.Dispatch("slideunderbarrel", 1);
			}
		}

        public void doJump()
        {
			if(!bInAir)
			{
				StopAllCoroutines();
				intersectName = "";
				bJumpFlag = true;

				if(bInDuck)
				{
					bInDuck = false;
					restoreAfterDuck();
				}
				am.jump();
				if(isBridge && isPatientZero)
				{
					Missions.Dispatch("jumponbridge", 1);
				}
				if(ID == 1)
				{
					Missions.Dispatch("jumpwithjessy", 1);
				}
			}
        }

        public void doSlide()
        {
			if(!Player.currentList[0].bInAir && !Player.currentList[0].bInDuck)
			{
				intersectName = "";
				duckPlayer();
			}
			else if(bInAir && !bInDuck)
			{
				bDiveFlag = true;
			}
        }

        public void changeTarget(Vector3 position, bool right)
        {
			targetPosition = position;

			if(bInJump || Player.isJumpPowerUp)
				return;

			if(right) 
			{
				StartCoroutine(am.slideRight()); 
			}
			else 
			{
				StartCoroutine(am.slideLeft());
			}
        }
		
		public void onDeath()
		{
			if(isPatientZero)
			{
                if (Player.currentList.Count > 1)
				{
					Player.currentList.Remove(this);

                    Player.currentList[0].isPatientZero = true;
					Player.currentList[0].offset = Vector2.zero;
					
					Destroy(gameObject);
				}
				else
				{
					Currency.reviveCount *= 2;

					am.death();

					deathSpeed = Player.Speed / Player.MinimumSpeed;

					Game.GameStop();
				}
			}
			else
			{
				Missions.Dispatch("die5butnotmain", 1);

                Player.currentList.Remove(this);
				
				Destroy(gameObject);
			}

			Audio.PlaySound (6 + ID);
		}

		public void OnPowerUpStarted()
		{
			board.gameObject.SetActive (true);
			Player.isJumpPowerUp = true;
			am.skate ();
			for(int i = 1; i < Player.currentList.Count; i++)
			{
				Player.currentList[i].onDeath();
				i--;
			}
		}

		public void OnPowerUpEnded()
		{
			board.gameObject.SetActive (false);
			am.run ();
			fContactPointY = 0;
			fCurrentUpwardVelocity = 0;
			fCurrentHeight = transform.position.y;
			bInAir = true;
			Player.isJumpPowerUp = false;
		}

		void OnCollisionExit(Collision other)
		{
			if (other.gameObject.CompareTag("Obstacle"))
			{
				fContactPointY = 0;
				bInAir = true;
			}
		}

		void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Obstacle") && !Player.isRevive)
            {
				if(other.collider.bounds.center.y < 10 && fContactPointY == 0 && !Player.isJumpPowerUp)
				{
					if(other.collider.bounds.size.y * .9f < other.contacts[0].point.y)
					{
						if(fContactPointY == 0)
						{
							if (isPatientZero)
							{
								Missions.Dispatch("jumpoverobstacles", 1);
								if(other.gameObject.name.ToLower().Contains("orange"))
								{
									Missions.Dispatch("jumponorangecar", 1);
								}
								else if(other.gameObject.name.ToLower().Contains("bus"))
								{
									Missions.Dispatch("jumponbus", 1);
								}
							}
						}
						fContactPointY = transform.position.y;
						return;
					}
				}

                if (Waypoint.currentWP != Waypoint.transitWP && !Player.isJumpPowerUp)
				{
                    int objectWP = 0;
                    float objectX = other.transform.position.x;
                    if(objectX > 4)
                    {
                        objectWP = 2;
                    }
                    else if(objectX < -4)
                    {
                        objectWP = 0;
                    }
                    else
                    {
                        objectWP = 1;
                    }

                    if(objectWP != Waypoint.transitWP)
                    {
                        Waypoint.changeWP(Waypoint.currentWP < Waypoint.transitWP);
                        return;
                    }
				}

				if(ID == 4 || Player.isJumpPowerUp)
				{
					var go = Instantiate(PowerUp._boomPrefab, new Vector3(0, 10, 0), Quaternion.identity) as GameObject;
					go.transform.parent = other.transform;
					Audio.PlaySound (14);
					other.transform.localScale = Vector3.zero;
					other.gameObject.collider.enabled = false;

					if(other.gameObject.name.ToLower().Contains("taxi"))
					{
						Missions.Dispatch("destroytaximarine", 1);
					}
					if(other.gameObject.name.ToLower().Contains("red"))
					{
						Missions.Dispatch("destroyredcar", 1);
					}

					Missions.Dispatch("destroyobstaclesmarine", 1);
				}

				soldierLife--;

				if(soldierLife <= 0 && !Player.isJumpPowerUp)
				{
					if(other.gameObject.name.ToLower().Contains("taxi") && ID == 3)
					{
						Missions.Dispatch("diedrwhitetaxi", 1);
					}
					if(other.gameObject.name.ToLower().Contains("er"))
					{
						Missions.Dispatch("dieambulance", 1);
					}
					if(other.gameObject.name.ToLower().Contains("police"))
					{
						Missions.Dispatch("diefrompolice", 1);
					}
					if(other.gameObject.name.ToLower().Contains("cone"))
					{
						Missions.Dispatch("diecone", 1);
					}
					if(other.gameObject.name.ToLower().Contains("red"))
					{
						Missions.Dispatch("dieredcar", 1);
					}
					if(other.gameObject.name.ToLower().Contains("swat"))
					{
						Missions.Dispatch("dieswat", 1);
					}
					if(other.gameObject.name.ToLower().Contains("bus"))
					{
						Missions.Dispatch("diebus", 1);
					}
					if(other.gameObject.name.ToLower().Contains("swat") && ID == 4)
					{
						Missions.Dispatch("diemarinefromswat", 1);
					}
					Audio.PlaySound(1);
					onDeath();
				}

            }
			else if (other.gameObject.CompareTag("Bridge"))
			{
				isBridge = true;
				brigeDistance = Player.Distance;
				bridgeHeight = Vector3.zero;
			}
			else if (other.gameObject.CompareTag("PowerUp"))
			{
				PowerUp.UseBonus(other.gameObject.GetComponent<ObstaclePowerUp>());
			}
			else if (other.gameObject.CompareTag("Human"))
			{
				other.gameObject.collider.enabled = false;
				var human = other.gameObject.GetComponent<ObstacleHuman>();
				human.movement.Stop();
				human.GetComponent<ObstacleAnimation>().death();

				int price = 0;
				switch(human.ID)
				{
				case 0:
					price = 20;
					break;
				case 1:
					price = 15;
					break;
				case 2:
					price = 10;
					break;
				case 3:
					price = 50;
					break;
				case 4:
					price = 5;
					break;
				}

                price *= Player.GetGoldBonus();

				Missions.Dispatch("gatherbrain", price);
				if(ID == 1)
				{
					Missions.Dispatch("gatherbrainjessy", price);
				}
                PlayerData.SetBrains(price);
				Currency.goldCount += price;
				Currency.showEatBrains(price);

				Audio.PlaySound (18 + human.ID);
				particle.Emit(50);

                if(PlayerData.zombieTutorial[human.ID] == "0")
                {
                    TutorialAction.showTutorial("00" + (5 + human.ID));
                    Time.timeScale = 0.01f;
                    PlayerData.zombieTutorial[human.ID] = "1";
                    tutorialZombieTime = Time.timeSinceLevelLoad + 0.05f;
                    StorageManager.Save();
                }

				if (Player.currentList.Count < Player.GetMaxPlayers() && PlayerManager.levels[human.ID] != 0 && !Player.isJumpPowerUp)
				{
					for(int i = 0; i < Player.currentList.Count; i++)
					{
						if(Player.currentList[i].ID == 2 && human.ID == 2)
						{
							return;
						}
					}

					Player.currentList.Add((Runner.PlayerController)GameObject.Instantiate(Player.GetById(human.ID)));
                    Player.currentList[Player.currentList.Count - 1].isPatientZero = false;
                    Player.currentList[Player.currentList.Count - 1].Initialize();
					
					var game = GameObject.FindGameObjectWithTag("Player");
                    Player.currentList[Player.currentList.Count - 1].gameObject.transform.parent = game.transform;

					Missions.Dispatch ("infect", 1);

					if(human.ID == 3)
					{
						Missions.Dispatch("infectprofessor", 1);
					}
					else if(human.ID == 2)
					{
						Missions.Dispatch("infectfatso", 1);
					}
					else if(ID == 1 && human.ID == 1)
					{
						Missions.Dispatch("infecthousewifewithjessy", 1);
					}
					if(ID == 3)
					{
						Missions.Dispatch("infectbydrwhite", 1);
					}
				}
			}
        }
    }
}
