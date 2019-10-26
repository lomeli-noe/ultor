using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
	private bool m_Jump;


    protected Button punchButton;
    protected Button kickButton;
	protected Button jumpButton;
	protected Button leftButton;
	protected Button rightButton;
	float horizontalMove = 0f;


    void Start()
	{
        punchButton = GameObject.Find("Punch").GetComponent<Button>();
        punchButton.onClick.AddListener(m_Character.Punch);

        kickButton = GameObject.Find("Kick").GetComponent<Button>();
        kickButton.onClick.AddListener(m_Character.Kick);

		jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
		jumpButton.onClick.AddListener(m_Character.Jump);

		leftButton = GameObject.Find("LeftButton").GetComponent<Button>();

		rightButton = GameObject.Find("RightButton").GetComponent<Button>();
	}

	private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
	}

    public void MoveLeft()
	{
		horizontalMove = -1;
	}

	public void MoveRight()
	{
		horizontalMove = 1;
	}

	public void StopPlayer()
	{
		horizontalMove = 0;
	}

	public void EnableJump()
	{
		m_Jump = true;
	}

	public void DisableJump()
	{
		m_Jump = false;
	}

	private void Update()
    {

        bool crouch = Input.GetKey(KeyCode.LeftControl);
	
		// Pass all parameters to the character control script.
		m_Character.Move(horizontalMove, crouch);
            
    }
}
