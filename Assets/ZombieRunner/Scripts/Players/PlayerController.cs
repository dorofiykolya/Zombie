using UnityEngine;
using System.Collections;
namespace Runner
{
    public class PlayerController : MonoBehaviour
    {

        private Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            set
            {
                targetPosition.x = value.x + offset.x;
				targetPosition.y = value.y + temp;
                targetPosition.z = value.z + offset.y;
            }
        }

        public bool isPatientZero = true;

        public BoxCollider collider;

        public int ID = 0;
		public int gameID = 0;
		private int temp;

        public bool isJumping;
        public bool isSliding;
		private bool isHalfJump;
        private bool jumpPending;
        private bool slidePending;

        private AnimationManager am;

        private Vector2 offset = Vector2.zero;

        public float sideScrollSpeed;
        public float jumpHeight;
        public float jumpForce;
		public float jumpAcceleration;
        public float slideDuration;

		private float jumpSpeed;
		private float bornTime;
		
		public float Distance {get{return PlayerManager.Distance;}}

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

            isJumping = false;
            jumpPending = false;

            TargetPosition = transform.position;
            targetPosition.x = WaypointManager.wayPoints[WaypointManager.currentWP].position.x + offset.x;
            transform.position = targetPosition;

            am = transform.GetComponent<AnimationManager>();

			if(ID == 2)
			{
				bornTime = Time.timeSinceLevelLoad;
			}
        }

        // Update is called once per frame
        void Update()
        {
			if(ID == 2 && Time.timeSinceLevelLoad - bornTime > PlayerValues.player_3_prefs[PlayerValues.levels[2]])
			{
				onDeath();
			}

            if (transform.position.x != targetPosition.x)
            {
                targetPosition.z = transform.position.z;
				targetPosition.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, sideScrollSpeed * Time.deltaTime);
            }
			if(isJumping)
			{
				targetPosition.y = jumpHeight;
				if (transform.position.y != targetPosition.y && !isHalfJump)
            	{
					jumpSpeed += jumpAcceleration;
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, (jumpForce + jumpSpeed) * Time.deltaTime);
				}
				else
				{
					if(jumpSpeed >= 0 && ! isHalfJump)
					{
						jumpSpeed -= jumpAcceleration * 2;
					}
					else
					{
						isHalfJump = true;
						
						targetPosition.y = temp;
						if (transform.position.y != targetPosition.y)
	            		{
							jumpSpeed += jumpAcceleration;
							transform.position = Vector3.MoveTowards(transform.position, targetPosition, (jumpForce + jumpSpeed) * Time.deltaTime);
						}
						else
						{
							am.run();
							isJumping = false;
							isHalfJump = false;
						}
					}
				}
			}
			else if(isSliding)
			{
				targetPosition.y = temp;
				if (transform.position.y != targetPosition.y)
            	{
					transform.position = Vector3.MoveTowards(transform.position, targetPosition, (isSliding ? jumpForce * 5 : jumpForce) * Time.deltaTime);
				}
			}
			am.updateSpeed();
        }

        public void doJump()
        {
            if (!isJumping)
            {
                isJumping = true;
                if (isPatientZero)
                {
                    am.jump();
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(jumpDelay());
                }
            }
            else
            {
                jumpPending = true;
            }
        }

        public void doSlide()
        {
            if (isJumping)
            {
                isJumping = false;
            }
            if (!isSliding)
            {
                isSliding = true;
                if (isPatientZero)
                {
                    am.slide();
					
					Vector3 height = collider.size;
					height.y /= 2;
                    Vector3 center = collider.center;
					center.y = height.y;
					collider.size = height;
                    collider.center = center;

                    StartCoroutine(slidingDuration());
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(slideDelay());
                }
            }
            else
            {
                slidePending = true;
            }
        }

        IEnumerator slidingDuration()
        {
            yield return new WaitForSeconds(slideDuration);

            isSliding = false;
			
			Vector3 height = collider.size;
            height.y *= 2;
            collider.size = height;
            Vector3 center = collider.center;
            center.y = collider.size.y;
            collider.center = center;

            am.run();
        }

        IEnumerator jumpDelay()
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            am.jump();
        }

        IEnumerator slideDelay()
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            StartCoroutine(slidingDuration());
            am.slide();
        }

        public void changeTarget(Vector3 position, bool right)
        {
            if (isPatientZero)
            {
                TargetPosition = position;
				if(right) StartCoroutine(am.slideRight()); else StartCoroutine(am.slideLeft());
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
			if(right) StartCoroutine(am.slideRight()); else StartCoroutine(am.slideLeft());
        }

		void onDeath()
		{
			if(isPatientZero)
			{
				if(PlayerManager.currentList.Count > 1)
				{
					PlayerManager.currentList.RemoveAt(0);
					
					for(int i = 0; i < PlayerManager.currentList.Count; i++)
					{
						PlayerManager.currentList[i].gameID = i;
					}
					
					PlayerManager.currentList[0].isPatientZero = true;
					
					Camera.main.transform.parent = null;
					
					Camera.main.transform.parent = PlayerManager.currentList[0].gameObject.transform;
					
					Camera.main.transform.localPosition = new Vector3(0, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
					
					Destroy(gameObject);
				}
				else
				{
					am.death();

					StateManager.Current = State.LOSE;
					PlayerManager.isStop = true;
				}
			}
			else
			{
				PlayerManager.currentList.RemoveAt(gameID);
				
				for(int i = ID; i < PlayerManager.currentList.Count; i++)
				{
					PlayerManager.currentList[i].gameID = i;
				}
				
				Destroy(gameObject);
			}
		}
		
		void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
				onDeath();
            }
            else if (other.gameObject.CompareTag("Currency"))
            {
				CurrencyManager.goldCount += (1 + PlayerManager.GetGoldBonus());
				other.transform.localScale = Vector3.zero;
            }
			else if (other.gameObject.CompareTag("Human"))
            {
				other.gameObject.collider.enabled = false;
				other.GetComponent<ObstacleHuman>().movement.speed = 0;
				
				if(PlayerManager.currentList.Count < PlayerManager.GetMaxPlayers())
				{
					PlayerManager.currentList.Add((Runner.PlayerController)GameObject.Instantiate(PlayerManager.GetById(other.GetComponent<ObstacleHuman>().ID)));
					PlayerManager.currentList[PlayerManager.currentList.Count - 1].isPatientZero = false;
					PlayerManager.currentList[PlayerManager.currentList.Count - 1].Initialize();
					PlayerManager.currentList[PlayerManager.currentList.Count - 1].gameID = PlayerManager.currentList.Count - 1;
					
					var game = GameObject.FindGameObjectWithTag("Player");
					PlayerManager.currentList[PlayerManager.currentList.Count - 1].gameObject.transform.parent = game.transform;
				}
			}
        }
    }
}
