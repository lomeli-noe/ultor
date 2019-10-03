using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
		private bool m_Jump;


        public Joystick joystick;
        float horizontalMove = 0f;
		float verticalMove = 0f;
        
		public float runSpeed = 1.2f;
		public float jumpSpeed = .5f;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {

            bool crouch = Input.GetKey(KeyCode.LeftControl);
			horizontalMove = joystick.Horizontal;
			verticalMove = joystick.Vertical;

			if (joystick.Horizontal >= .2f)
			{
				horizontalMove = runSpeed;
			}
            else if(joystick.Horizontal <= -.2f)
			{
				horizontalMove = -runSpeed;
			}
			else
			{
				horizontalMove = 0f;
			}

            if(joystick.Vertical >= .5f)
			{
				m_Jump = true;
				verticalMove = jumpSpeed;
			}

			// Pass all parameters to the character control script.
			m_Character.Move(horizontalMove,
                             crouch,
                             m_Jump
                             
                             );
            m_Jump = false;
            
        }
    }
}
