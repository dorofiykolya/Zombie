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

        public int ID = 0;
		public int gameID = 0;

		private float tCurrentAngle = 0.0f;
		private float fJumpForwardFactor = 0.0f;
		private float fCurrentUpwardVelocity = 0.0f;
		private float fCurrentHeight = 0.0f;
		private float fContactPointY = 0.0f;

		public float fJumpPush = 185;
		public float acceleration = 500;

		public bool bInAir = false;
		private bool bJumpFlag = false;
		private bool bInJump = false;
		public bool bInDuck = false;
		private bool bDiveFlag = false;
		private bool bExecuteLand = false;

        private AnimationManager am;

        private Vector2 offset = Vector2.zero;

		private Vector3 rotateVector = Vector3.zero;

        public float sideScrollSpeed;
        public float slideDuration;
		
		private float bornTime;
		private int soldireLife;

        public float Distance { get { return Player.Distance; } }

		private Transform playerRotate;
		private ParticleSystem particle;

        public void Initialize()
        {
			offset.x = 0;
			offset.y = 0;

            if (!isPatientZero)
            {
				while(offset.x < 1.5f && offset.x > -1.5f)
					offset.x = Random.Range(-2f, 2f);
				while(offset.y < 1.5f && offset.y > -1.5f)
					offset.y = Random.Range(-2f, 2f);
            }

			bInAir = false;
			bJumpFlag = false;
			bInJump = false;
			bInDuck = false;
			bDiveFlag = false;
			bExecuteLand = false;

            TargetPosition = transform.position;
            targetPosition.x = WaypointManager.wayPoints[WaypointManager.currentWP].position.x + offset.x;
            transform.position = targetPosition;

			am = transform.FindChild ("Player").GetComponent<AnimationManager>();

			if(ID == 2)
			{
				bornTime = Time.timeSinceLevelLoad;
			}
			if(ID == 4)
			{
				soldireLife = PlayerValues.levels[ID];
			}

			playerRotate = transform.FindChild ("Player").transform;
			particle = transform.FindChild ("Smoke").GetComponent<ParticleSystem> ();
			particle.Stop ();
        }

        // Update is called once per frame
        void Update()
        {
			if(States.Current == State.LOSE)
			{
				return;
			}

            var speed = Player.Speed / Player.MinimumSpeed;

			if(ID == 2 && Time.timeSinceLevelLoad - bornTime > PlayerValues.player_3_prefs[PlayerValues.levels[2]])
			{
				onDeath();
			}

            if (transform.position.x != targetPosition.x)
            {
                targetPosition.z = transform.position.z;
				targetPosition.y = transform.position.y;
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, sideScrollSpeed * Time.deltaTime * speed);

				rotateVector.y = Mathf.Sign(targetPosition.x - transform.position.x) * 45 * ((8 - Vector3.Distance(transform.position, targetPosition)) / 8);

				playerRotate.localEulerAngles = rotateVector;
            }
			else
			{
				if(playerRotate.localEulerAngles.y != 0)
				{
					playerRotate.localEulerAngles = Vector3.RotateTowards(playerRotate.position, playerRotate.position, sideScrollSpeed * Time.deltaTime * 0.01f * speed, 0f);
				}
			}

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

			if(fContactPointY != 0)
			{
				fContactPointY = 0;
				bInAir = true;
			}

			am.updateSpeed();
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

				if(bInJump)
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
					
					Camera.main.transform.parent = null;

                    Camera.main.transform.parent = Player.currentList[0].gameObject.transform;
					
					Camera.main.transform.localPosition = new Vector3(0, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
					
					Destroy(gameObject);
				}
				else
				{
					am.death();

					States.Current = State.LOSE;
                    Player.isStop = true;
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
		
		void OnCollisionEnter(Collision other)
        {
			Debug.Log (other.gameObject.name);
            if (other.gameObject.CompareTag("Obstacle"))
            {
				Debug.Log(other.collider.bounds.size.y + " " + other.contacts[0].point.y);
				if(other.collider.bounds.center.y < 10)
				{
					if(other.collider.bounds.size.y - other.collider.bounds.center.y < other.contacts[0].point.y)
					{
						fContactPointY = other.contacts[0].point.y;
						return;
					}
				}
				if(ID == 4)
				{
					other.gameObject.GetComponent<MeshExploder>().Explode();
					other.transform.localPosition = Vector3.zero;
				}

				if(soldireLife == 0)
					onDeath();
				else
					soldireLife--;
            }
            else if (other.gameObject.CompareTag("Currency"))
            {
                CurrencyManager.goldCount += (1 + Player.GetGoldBonus());
				other.transform.localScale = new Vector3(0,-1000,0);
            }
			else if (other.gameObject.CompareTag("Human"))
			{
				if(isPatientZero)
				{
					StartCoroutine(am.eat());
					particle.Emit(50);
				}

				other.gameObject.collider.enabled = false;
				other.gameObject.GetComponent<ObstacleHuman>().movement.speed = 0;

                if (Player.currentList.Count < Player.GetMaxPlayers())
				{
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
