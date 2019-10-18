using System.Collections;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 8f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 300f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        Transform playerGraphics;
        Transform firePoint;
        public Transform PunchEffectPrefab;

        AudioManager audioManager;
		[SerializeField]
		string whooshSound = "Whoosh";

		public float camShakeAmt = 0.05f;
		public float camShakeLength = 0.1f;
		CameraShake camShake;


		private float attackTimer = 0;
        private float attackCd = .3f;
        private bool attacking = false;

        public Collider2D attackTrigger;

		 private void Start()
		{
			camShake = GameMaster.gm.GetComponent<CameraShake>();
			if (camShake == null)
			{
				Debug.LogError("No camera shake script found on GM object.");
			}
		}

        private void Awake()
        {
            // Setting up references.
            firePoint = transform.Find("FirePoint");
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            playerGraphics = transform.Find("PlayerBodyParts").Find("Graphics");
            if (playerGraphics == null)
            {
                Debug.LogError("No Graphics object as a child of player");
            }

            attackTrigger.enabled = false;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

			audioManager = AudioManager.instance;
			if (audioManager == null)
			{
				Debug.LogError("No audiomanager found!");
			}

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }

        public void Punch()
        {
            if (!attacking)
            {
                attacking = true;

                if (!m_Grounded)
                {
                    m_Anim.SetBool("FlyingPunch", attacking);

                    StartCoroutine(DisableFlyingPunch());
                    attackTrigger.enabled = true;
                    StartCoroutine(PunchEffect());
                }
                else
                {
                    m_Anim.SetBool("Punch", attacking);
                    StartCoroutine(DisablePunch());
                    attackTrigger.enabled = true;
                    StartCoroutine(PunchEffect());
                }
            }

        }

        public void Kick()
        {
            if (!attacking)
            {
                attacking = true;

                if (!m_Grounded && !m_Anim.GetBool("FlyingKick"))
                {
                    m_Anim.SetBool("FlyingKick", attacking);
                    StartCoroutine(DisableFlyingKick());
                    attackTrigger.enabled = true;
                    StartCoroutine(PunchEffect());
                }
                else
                {
                    m_Anim.SetBool("Kick", attacking);
                    StartCoroutine(DisableKick());
                    attackTrigger.enabled = true;
                    StartCoroutine(PunchEffect());
                }
            }
            
        }

        IEnumerator PunchEffect()
        {
            yield return new WaitForSeconds(.05f);
			audioManager.PlaySound(whooshSound);
			Transform clone = Instantiate(PunchEffectPrefab, firePoint.position, firePoint.rotation) as Transform;
            clone.parent = firePoint;
            float size = Random.Range(0.9f, 1.2f);
            clone.localScale = new Vector3(-size, size, size);
            Destroy(clone.gameObject, 0.03f);
			camShake.Shake(camShakeAmt, camShakeLength);
		}

        IEnumerator DisableFlyingKick()
        {
            yield return new WaitForSeconds(.25f);
            attacking = false;
            attackTrigger.enabled = false;
            m_Anim.SetBool("FlyingKick", false);
        }

        IEnumerator DisableFlyingPunch()
        {
            yield return new WaitForSeconds(.25f);
            attacking = false;
            attackTrigger.enabled = false;
            m_Anim.SetBool("FlyingPunch", false);
        }

        IEnumerator DisablePunch()
        {
            yield return new WaitForSeconds(.07f);
            attacking = false;
            attackTrigger.enabled = false;
            m_Anim.SetBool("Punch", false);
        }

        IEnumerator DisableKick()
        {
            yield return new WaitForSeconds(.09f);
            attacking = false;
            attackTrigger.enabled = false;
            m_Anim.SetBool("Kick", false);
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);


            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
