﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class LandChimp : MonoBehaviour
{
    Animator m_chimpAnim;
    bool m_isJumping , m_isSliding;
	BoxCollider2D m_blockerBottomCollider2D , m_chimpCollider2D;
    float m_defaultGravityScale , m_defaultXPos;
	GameManager m_gameManager;
	LevelCreator m_levelCreator;
    Rigidbody2D m_chimpBody2D;
    RockSpawner m_rockSpawner;
	SoundManager m_soundManager;

    [SerializeField] bool m_grounded;
    [SerializeField] float m_jumpHeight;
    [SerializeField] Transform m_raycastOrigin;

    [HideInInspector] public bool m_isSlipping , m_isSuper;

	void Reset()
	{
		m_jumpHeight = 15.5f;
		m_isSlipping = false;
		m_isSuper = false;
	}

	void Start()
    {
        m_blockerBottomCollider2D = GameObject.Find("BlockerBottom").GetComponent<BoxCollider2D>();
        m_chimpAnim = GetComponent<Animator>();
        m_chimpBody2D = GetComponent<Rigidbody2D>();
        m_chimpCollider2D = GetComponent<BoxCollider2D>();
        m_defaultGravityScale = m_chimpBody2D.gravityScale;
        m_defaultXPos = transform.position.x;
		m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		m_levelCreator = GameObject.Find("LevelCreator").GetComponent<LevelCreator>();
        LevelCreator.m_middleCounter = 0;
        m_rockSpawner = GameObject.Find("RockSpawner").GetComponent<RockSpawner>();
		m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if(m_isSuper && transform.position.x < m_defaultXPos) //TODO this is temp fix and find better way if necessary
        {
            transform.position = new Vector2(m_defaultXPos , transform.position.y);
        }

        BhanuInput();
        Grounded();

    }

    void BhanuInput()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
            if(Input.GetMouseButtonDown(0)) //TODO This ia for testing only
            {
                Jump();
            }

            if(Input.GetMouseButtonDown(1)) //TODO This ia for testing only
            {
                Slide();
            }
        #endif

        #if UNITY_ANDROID || UNITY_IPHONE
            if(SwipeManager.Instance.IsSwiping(SwipeDirection.UP))
            {
                Jump();
            }

            if(SwipeManager.Instance.IsSwiping(SwipeDirection.DOWN))
            {
                Slide();
            }
        #endif
    }

    void CheatDeath()
    {
        m_gameManager.Ads();
        StopAllCoroutines();
    }

    void Grounded()
    {
        if(!m_isSuper)
        {
            //Debug.DrawLine(new Vector2(transform.position.x , transform.position.y - 0.7f) , new Vector2(transform.position.x , transform.position.y - 0.95f) , Color.green);
            RaycastHit2D hit2D = Physics2D.Raycast(m_raycastOrigin.position , -Vector2.down);

            if(hit2D)
            {
                if(!hit2D.collider.isTrigger)
                {
                    //Debug.Log("Grounded");
                    m_grounded = true;
                }
                else
                {
                    //Debug.Log("Not Grounded because fell in the hole");
                    m_grounded = false;
                }
            }
        }

        else if(m_isSuper)
        {
            LevelCreator.m_middleCounter = 0.5f;
        }
    }
	
	public void Jump()
    {
		if(m_grounded && !m_isJumping && !m_isSliding)
        {
            m_chimpAnim.SetBool("Jump" , true);
            m_chimpBody2D.velocity = new Vector2(m_chimpBody2D.velocity.x , m_jumpHeight);
            m_chimpCollider2D.isTrigger = true;
            SelfieAppear();
            m_isJumping = true;
            Invoke("JumpFinished" , 0.55f);
			m_soundManager.m_soundsSource.clip = m_soundManager.m_jump;
			m_soundManager.m_soundsSource.Play();
        }

        if(m_isSuper)
        {
            m_chimpBody2D.velocity = new Vector2(m_chimpBody2D.velocity.x , m_jumpHeight);
        }
    }

    void JumpFinished()
    {
        m_chimpAnim.SetBool("Jump" , false);
        m_chimpCollider2D.isTrigger = false;
        m_isJumping = false;      
        
        if(!m_isSuper)
        {
            SelfieDisappear();
        }

        else if(m_isSuper)
        {
            Invoke("SelfieDisappear" , 0.75f);
        }
    }

    void OnTriggerEnter2D(Collider2D tri2D)
    {
        if(tri2D.gameObject.tag.Equals("Fall"))
        {
			m_soundManager.m_soundsSource.clip = m_soundManager.m_fallDeath;
			m_soundManager.m_soundsSource.Play();
            CheatDeath();
        }

        if(tri2D.gameObject.tag.Equals("Hurdle"))
        {
			m_soundManager.m_soundsSource.clip = m_soundManager.m_hurdleDeath;
			m_soundManager.m_soundsSource.Play();
            CheatDeath();
        }

		if(tri2D.gameObject.tag.Equals("Portal"))
		{
            int levelToLoadAtRandom = Random.Range(0 , 4);

			switch(levelToLoadAtRandom)
			{
				case 0:
                    SceneManager.LoadScene("WaterSwimmer");
				break;

				case 1:
					SceneManager.LoadScene("WaterSwimmer");
				break;

				case 2:
					Screen.orientation = ScreenOrientation.Portrait;
					SceneManager.LoadScene("FallingDown");
				break;

				case 3:
					Screen.orientation = ScreenOrientation.Portrait;
					SceneManager.LoadScene("FallingDown");
				break;
			}
		}

        if(tri2D.gameObject.tag.Equals("Skin"))
        {
            Slip();
        }

		if(tri2D.gameObject.tag.Equals("Super"))
		{
			Super();
		}
    }

    void SelfieAppear()
    {
        GameManager.m_selfieButtonImage.enabled = true;
    }

    void SelfieDisappear()
    {
        GameManager.m_selfieButtonImage.enabled = false;
    }

    public void Slide()
    {
		if(m_grounded && !m_isJumping)
		{
			m_chimpAnim.SetBool("Jog" , false);
			m_chimpAnim.SetBool("Slide" , true);
			m_chimpBody2D.gravityScale = 0;
			m_chimpCollider2D.enabled = false;
			SelfieAppear();
			m_isSliding = true;
			Invoke("SlideFinished" , 0.75f);
		}
    }

    void SlideFinished()
    {
        m_chimpAnim.SetBool("Slide" , false);
        m_chimpBody2D.gravityScale = m_defaultGravityScale;
        m_chimpCollider2D.enabled = true;
		m_isSliding = false;
        SelfieDisappear();
    }

    void Slip()
    {
        m_chimpAnim.SetBool("Slip" , true);
        
        if(m_levelCreator.m_gameSpeed < 12f)
        {
            m_levelCreator.m_gameSpeed = 12f;
        }
        
        m_isSlipping = true;
        Invoke("SlipFinished" , 5.15f);
    }

    void SlipFinished()
    {
		m_chimpAnim.SetBool("Slip" , false);
        m_levelCreator.m_gameSpeed = 6.0f;
        m_isSlipping = false;
    }

	void Super()
	{
        m_blockerBottomCollider2D.enabled = true;
        m_chimpAnim.SetBool("Super" , true);
        m_chimpBody2D.gravityScale /= 2.5f;
		SelfieAppear();
        m_grounded = false;
        m_levelCreator.m_gameSpeed = 6.0f;
        m_isSuper = true;
        m_rockSpawner.StartSpawnRoutine();
		Invoke("SuperFinished" , 30.25f);
	}

    void SuperFinished()
    {
        m_blockerBottomCollider2D.enabled = false;
        m_chimpAnim.SetBool("Super" , false);
        m_chimpBody2D.gravityScale = m_defaultGravityScale;
        m_isSuper = false;	
        LevelCreator.m_middleCounter = 0;
    }
}