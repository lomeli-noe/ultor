using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
	private bool m_Jump;


    protected Joystick joystick;
    protected Button punchButton;
    protected Button kickButton;
    float horizontalMove = 0f;
	float verticalMove = 0f;
        
	public float runSpeed = 1.2f;
	public float jumpSpeed = .5f;


    void Start()
	{
		joystick = FindObjectOfType<Joystick>();
        punchButton = GameObject.Find("Punch").GetComponent<Button>();
        punchButton.onClick.AddListener(m_Character.Punch);
        kickButton = GameObject.Find("Kick").GetComponent<Button>();
        kickButton.onClick.AddListener(m_Character.Kick);
    }

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
