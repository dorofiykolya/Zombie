using UnityEngine;
using System.Collections;
namespace Runner
{
    public class PlayerController : ComponentManager
    {

        private Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            set
            {
                targetPosition.x = value.x + offset.x;
				targetPosition.y = value.y;
                targetPosition.z = value.z + offset.y;
            }
        }

        public bool isPatientZero = true;
        public BoxCollider collider;
		public SphereCollider magnet { get; private set; }

        public int ID = 0;
		public int gameID = 0;

		private float tCurrentAngle = 0.0f;
		private float fJumpForwardFactor = 0.0f;
		private float fCurrentUpwardVelocity = 0.0f;
		private float fCurrentHeight = 0.0f;
		private float fContactPointY = 0.0f;

		private Transform board;

		public float fJumpPush = 185;
		public float acceleration = 500;

		private float glideSpeed = 100;

		public bool bInAir;
		public bool glideEnable;
		private bool bJumpFlag;
		private bool bInJump;
		public bool bInDuck;
		private bool bDiveFlag;
		private bool bExecuteLand;
		private bool isBridge;
		private Vector3 bridgeHeight;
		private float brigeDistance;

        private AnimationManager am;

        private Vector2 offset = Vector2.zero;
		private Vector3 rotateVector = Vector3.zero;

        public float sideScrollSpeed;
        public float slideDuration;
		
		public float bornTime;
		private int soldierLife;
		private float reviveTime;

        public float Distance { get { return Player.Distance; } }
		private float speed;
		private float deathSpeed;

		private Transform playerRotate;
		private ParticleSystem particle;
		private bool blink;
		private float blinkTime;

        public void Initialize()
        {
			offset.x = 0;
			offset.y = 0;

			fContactPointY = 0;

            if (!isPatientZero)
            {
				if(ID == 4)
				{
					offset.y = 4f;
				}
				else if(ID == 2)
				{
					offset.y = -4f;
				}
				else
				{
					if(Player.currentList[0].ID == 4)
					{
						while(offset.y < 1.5f && offset.y > 0f)
							offset.y = Random.Range(0f, 2f);
					}
					else
					{
						while(offset.y < 1.5f && offset.y > -1.5f)
							offset.y = Random.Range(-2f, 2f);
					}
				}

				while(offset.x < 1.5f && offset.x > -1.5f)
					offset.x = Random.Range(-2f, 2f);
            }

			bInAir = false;
			bJumpFlag = false;
			glideEnable = false;
			bInJump = false;
			bInDuck = false;
			bDiveFlag = false;
			bExecuteLand = false;
			isBridge = false;
			bridgeHeight = Vector3.zero;

			magnet = GameObject.Find ("Player").collider as SphereCollider;
			board = transform.FindChild("Board");

            TargetPosition = transform.position;
			targetPosition.x = Waypoint.wayPoints[Waypoint.currentWP].position.x + offset.x;
            transform.position = targetPosition;

			am = transform.FindChild ("Player").GetComponent<AnimationManager>();

			if(ID == 2)
			{
				bornTime = Time.timeSinceLevelLoad;
			}
			if(ID == 4)
			{
				soldierLife = PlayerValues.player_5_prefs[PlayerValues.levels[ID]];
			}
			if(Player.isRevive)
			{
				reviveTime = Time.timeSinceLevelLoad;
			}

			playerRotate = transform.FindChild ("Player").transform;
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
					soldierLife = PlayerValues.player_5_prefs[PlayerValues.levels[ID]];
				}
				if(am != null);
					am.run();
			}
		}

        // Update is called once per frame
        void Update()
        {
			//falling down if player dies in air
			if(Player.isStop)
			{
				deathFall();
				return;
			}

			//update game speed
            speed = Player.Speed / Player.MinimumSpeed;

			//revive
			if(Player.isRevive)
			{
				if(Time.timeSinceLevelLoad - reviveTime > 3)
				{
					Player.isRevive = false;
					am.gameObject.SetActive(true);
				}

				if(Time.timeSinceLevelLoad - blinkTime > 0.4)
				{
					am.gameObject.SetActive(blink);
					blink = !blink;
					blinkTime = Time.timeSinceLevelLoad;
				}
			}

			//fatman dies
			if(ID == 2 && Time.timeSinceLevelLoad - bornTime > PlayerValues.player_3_prefs[PlayerValues.levels[2]] && !Player.isJumpPowerUp)
			{
				onDeath();
			}

			//board flight animation
			if(Player.isJumpPowerUp && isPatientZero)
			{
				am.idle();

				if(transform.position.y == 26)
				{
					glideEnable = false;
				}

				if(transform.position.y == 28)
				{
					glideSpeed = 5;
					glideEnable = true;
				}

				if(glideEnable)
				{
					targetPosition.y = 26;
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, glideSpeed * Time.deltaTime * speed);
				}
				else
				{
					targetPosition.y = 28;
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, glideSpeed * Time.deltaTime * speed);
				}

				Camera.main.transform.localPosition = new Vector3(Player.defaultCameraPosition.x, Player.defaultCameraPosition.y + 20 - transform.position.y, Player.defaultCameraPosition.z);
			}

			//change waypoint
            if (transform.position.x != targetPosition.x)
            {
				if(Mathf.Abs(transform.position.x - targetPosition.x) <= 4)
				{
					Waypoint.transitWP = Waypoint.currentWP;
				}

                targetPosition.z = transform.position.z;
				targetPosition.y = transform.position.y;
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, sideScrollSpeed * Time.deltaTime * speed);

				if(!Player.isJumpPowerUp)
				{
					rotateVector.y = Mathf.Sign(targetPosition.x - transform.position.x) * 45 * ((8 - Vector3.Distance(transform.position, targetPosition)) / 8);

					playerRotate.localEulerAngles = rotateVector;
				}
            }
			else
			{
				if(playerRotate.localEulerAngles.y != 0 && !Player.isJumpPowerUp)
				{
					playerRotate.localEulerAngles = Vector3.RotateTowards(playerRotate.position, transform.position, sideScrollSpeed * Time.deltaTime * 0.01f * speed, 0f);
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
				if(isPatientZero)               
					Camera.main.transform.localPosition = new Vector3(Player.defaultCameraPosition.x, Player.defaultCameraPosition.y - fCurrentHeight, Player.defaultCameraPosition.z);
			}

			if(bJumpFlag == true)
			{
				bJumpFlag = false;
				bExecuteLand = true;
				bInJump = true;
				bInAir = true;
				fCurrentUpwardVelocity = fJumpPush;
				fCurrentHeight = transform.position.y;
			}

			//step on bridge
			if(isBridge)
			{
				var length = Player.Distance - brigeDistance;
				if(length < 100)
				{
					targetPosition.y = 8;
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

					targetPosition.y = 0;
				}

				bridgeHeight = Vector3.MoveTowards(bridgeHeight, targetPosition, 4 * Time.deltaTime * speed);
				fContactPointY = bridgeHeight.y;

				if(!bInJump)
				{
					bridgeHeight.x = transform.position.x;
					bridgeHeight.z = transform.position.z;
					transform.position = bridgeHeight;
				}
			}

			am.updateSpeed();
        }

		public void Revive()
		{
			am.run ();

			reviveTime = Time.timeSinceLevelLoad;

			if(ID == 2)
			{
				bornTime = Time.timeSinceLevelLoad;
			}
			if(ID == 4)
			{
				soldierLife = PlayerValues.player_5_prefs[PlayerValues.levels[ID]];
			}
		}

		private void deathFall()
		{
			if(transform.position.y != 0)
			{
				targetPosition.y = 0;
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, sideScrollSpeed * Time.deltaTime * deathSpeed);
			}
		}

		private void setCurrentJumpHeight(float speed)
		{
			fCurrentUpwardVelocity -= Time.deltaTime * acceleration * speed;
			fCurrentUpwardVelocity = Mathf.Clamp(fCurrentUpwardVelocity, -fJumpPush, fJumpPush);
			fCurrentHeight += fCurrentUpwardVelocity * Time.deltaTime * speed;
			
			if(fCurrentHeight < fContactPointY)
			{
				fCurrentHeight = fContactPointY;
				bInAir = false;
				bInJump = false;
				
				if (bDiveFlag)
					return;

				am.run();
			}
		}

		private void setCurrentDiveHeight(float speed)
		{
			fCurrentUpwardVelocity -= Time.deltaTime * 2000 * speed;
			fCurrentUpwardVelocity = Mathf.Clamp(fCurrentUpwardVelocity, -fJumpPush, fJumpPush);
			fCurrentHeight += fCurrentUpwardVelocity * Time.deltaTime * speed;

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

			Vector3 height = collider.center;
			height.y /= 6;
			collider.center = height;

			StartCoroutine (slideDurationTimer ());
		}

		private IEnumerator slideDurationTimer ()
		{
			yield return new WaitForSeconds(slideDuration);

			bInDuck = false;

			Vector3 height = collider.center;
			height.y *= 6;
			collider.center = height;
			
			am.run();
		}

        public void doJump()
        {
			if(!bInAir)
			{
				bJumpFlag = true;
				bInDuck = false;
				am.jump();
			}
        }

        public void doSlide()
        {
            if(!bInAir && !bInDuck)
			{
				duckPlayer();
			}
			else if(bInAir && !bInDuck)
			{
				bDiveFlag = true;
			}
        }

        public void changeTarget(Vector3 position, bool right)
        {
            if (isPatientZero)
            {
                TargetPosition = position;

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
            else
            {
                StopAllCoroutines();
                StartCoroutine(changeDelay(position, right));
            }
        }

        IEnumerator changeDelay(Vector3 position, bool right)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            TargetPosition = position;

			if(!bInJump)
			{			
				if(right) 
				{
					StartCoroutine(am.slideRight()); 
				}
				else 
				{
					StartCoroutine(am.slideLeft());
				}
			}
		}
		
		void onDeath()
		{
			if(isPatientZero)
			{
                if (Player.currentList.Count > 1)
				{
                    Player.currentList.RemoveAt(gameID);

                    for (int i = 0; i < Player.currentList.Count; i++)
					{
                        Player.currentList[i].gameID = i;
					}

                    Player.currentList[0].isPatientZero = true;
					Player.currentList[0].offset = Vector2.zero;

                    Camera.main.transform.parent = Player.currentList[0].gameObject.transform;
					
					Camera.main.transform.localPosition = new Vector3(0, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
					
					Destroy(gameObject);
				}
				else
				{
					am.death();

					deathSpeed = Player.Speed / Player.MinimumSpeed;

					Game.GameStop();
				}
			}
			else
			{
                Player.currentList.RemoveAt(gameID);

                for (int i = ID; i < Player.currentList.Count; i++)
				{
                    Player.currentList[i].gameID = i;
				}
				
				Destroy(gameObject);
			}
		}

		public void OnPowerUpStarted()
		{
			board.gameObject.SetActive (true);
			Player.isJumpPowerUp = true;
			glideSpeed = 100;
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
            if (other.gameObject.CompareTag("Obstacle") && !Player.isJumpPowerUp && !Player.isRevive)
            {
				if(other.collider.bounds.center.y < 10)
				{
					if(other.collider.bounds.size.y * .9f < other.contacts[0].point.y)
					{
						fContactPointY = other.contacts[0].point.y;
						return;
					}
				}

				var targetX = Mathf.Round(other.transform.position.x);
				var playerX = Mathf.Round(transform.position.x);

				if(Waypoint.currentWP != Waypoint.transitWP)
				{
					Waypoint.changeWP(Waypoint.currentWP < Waypoint.transitWP);
					return;
				}

				if(ID == 4)
				{
					other.transform.localScale = Vector3.zero;
					other.gameObject.collider.enabled = false;
				}

				soldierLife--;

				if(soldierLife <= 0)
					onDeath();

            }
			else if (other.gameObject.CompareTag("Bridge"))
			{
				isBridge = true;
				brigeDistance = Player.Distance;
				bridgeHeight = Vector3.zero;
			}
			else if (other.gameObject.CompareTag("Currency"))
            {
                CurrencyManager.goldCount += (1 + Player.GetGoldBonus());
				other.transform.GetComponent<ObstacleCurrency>().isPickUp = true;
				other.gameObject.collider.enabled = false;
            }
			else if (other.gameObject.CompareTag("PowerUp"))
			{
				PowerUp.UseBonus(other.gameObject.GetComponent<ObstaclePowerUp>());
			}
			else if (other.gameObject.CompareTag("Human"))
			{
				other.gameObject.collider.enabled = false;
				other.gameObject.GetComponent<ObstacleHuman>().movement.speed = 0;

                if (Player.currentList.Count < Player.GetMaxPlayers())
				{
					particle.Emit(50);

                    Player.currentList.Add((Runner.PlayerController)GameObject.Instantiate(Player.GetById(other.gameObject.GetComponent<ObstacleHuman>().ID)));
                    Player.currentList[Player.currentList.Count - 1].isPatientZero = false;
                    Player.currentList[Player.currentList.Count - 1].Initialize();
                    Player.currentList[Player.currentList.Count - 1].gameID = Player.currentList.Count - 1;
					
					var game = GameObject.FindGameObjectWithTag("Player");
                    Player.currentList[Player.currentList.Count - 1].gameObject.transform.parent = game.transform;
				}
			}
        }
    }
}
