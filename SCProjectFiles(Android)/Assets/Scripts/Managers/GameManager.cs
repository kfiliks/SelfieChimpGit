﻿using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	AudioSource _musicSource;
    bool _gpgsLogInButtonTapped , _profilePicEnabled = false;
    //Dictionary<string , object> _highScoresData;
    //Dictionary<string , string> _scores = null;
    //float _highScore;
	Image _adsAcceptButtonImage , _adsCancelButtonImage , _adsMenuImage , _backToLandLoseMenuImage , _backToLandWinMenuImage , _backToLandWithSuperMenuImage , _chimpionshipBeltImage , _continueButtonImage , _exitButtonImage , _gpgsLeaderboardConfirmMenuImage , _gpgsLeaderboardOKButtonImage , _gpgsLeaderboardSuccessOKButtonImage;
    Image _gpgsLeaderboardUpdateAcceptButtonImage , _gpgsLeaderboardUpdateCancelButtonImage , _muteButtonImage , _pauseButtonImage , _pauseMenuImage , _playButtonImage , _quitButtonImage , _quitAcceptButtonImage , _quitCancelButtonImage , _quitMenuImage , _restartButtonImage;
    Image _restartAcceptButtonImage , _restartCancelButtonImage , _restartMenuImage , _resumeButtonImage , _unmuteButtonImage;
    LandChimp _landChimp;
	SoundManager _soundManager;
    string _applinkURL , _leaderboardID = "CgkInMKFu8wYEAIQAQ";
	Text _adsText , _backToLandLoseText , _backToLandWinText , _backToLandWithSuperText , _gpgsLeaderboardUpdateText , _highScoreDisplayText , _highScoreValueText , _noInternetText , _quitText , _restartText;

	[SerializeField] bool /*_isFBInviteTestMode , */_isFBShareTestMode , _isGPGsLeaderboardTestMode , _isGPGsLogInTestMode , _selfieFlashEnabled;
    [SerializeField] GameObject /*_fbChallengeInviteSuccessMenuObj , */_fbShareMenuObj , _fbShareSuccessMenuObj , _fbShareTestMenuObj , _fbLoggedInObj , _fbLoggedOutObj , _gpgsLeaderboardTestMenuObj , _gpgsLoggedInObj , _gpgsLoggedOutObj;
    [SerializeField] Image _facebookButtonImage , _fallingLevelImage , /*_fbChallengeInviteButtonImage , */_gpgsLogInButtonImage , _landLevelImage , _leaderboardButtonImage , _profilePicImage , _rateButtonImage , _shareButtonImage , _waterLevelImage;
    [SerializeField] Text /*_fbChallengeInviteTestText, _fbScoreText, */_gpgsLogInTestText , _gpgsLeaderboardLogInCheckText , _gpgsLeaderboardTestText , _gpgsLeaderboardUpdateSuccessText , _memoryLeakTestText , _usernameText;

    public static bool m_isMemoryLeakTestingMode , m_isTestingUnityEditor;
    public static Image m_selfieButtonImage , m_selfiePanelImage;
    public static int m_currentScene , m_playerMutedSounds;

    void Start()
	{
        FBInit();
        GPGsInit();
        //m_isMemoryLeakTestingMode = true; //TODO Remove this for Live Version
        Invoke("FBLogInCheck" , 0.2f);
        Invoke("GPGsLogInCheck" , 0.2f);
        GetBhanuObjects();
    }

    public void Ads()
    {
        _adsMenuImage.enabled = true;
        _adsAcceptButtonImage.enabled = true;
        _adsCancelButtonImage.enabled = true;
        _adsText.enabled = true;
        _chimpionshipBeltImage.enabled = false;
        
        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(false);
        }

        _muteButtonImage.enabled = false;
        _pauseButtonImage.enabled = false;
		m_selfieButtonImage.enabled = false;
        _unmuteButtonImage.enabled = false;
		Time.timeScale = 0;
    }

    public void AdsAcceptButton()
    {
        _adsMenuImage.enabled = false;
        _adsAcceptButtonImage.enabled = false;
        _adsCancelButtonImage.enabled = false;
        _adsText.enabled = false;
        
        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(false);
        }

        AdsShow();
    }

    public void AdsCancelButton()
    {
        BhanuPrefs.DeleteAll();
		ScoreManager.m_supersCount = ScoreManager.m_defaultSupersCount;
		BhanuPrefs.SetSupers(ScoreManager.m_supersCount);
        SceneManager.LoadScene(m_currentScene);
    }

    void AdResult(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            //Debug.Log("Video completed - Offer a reward to the player");
            ScoreManager.m_scoreValue *= 0.25f;
		    ScoreManager.m_scoreValue = Mathf.Round(ScoreManager.m_scoreValue);
            BhanuPrefs.SetHighScore(ScoreManager.m_scoreValue);
            Time.timeScale = 1;
            SceneManager.LoadScene(m_currentScene);
        }

        else if(result == ShowResult.Skipped)
        {
            //Debug.LogWarning("Video was skipped - Do NOT reward the player");
            BhanuPrefs.DeleteAll();
        }

        else if(result == ShowResult.Failed)
        {
            //Debug.LogError("Video failed to show");
            BhanuPrefs.DeleteAll();
        }
    }

    void AdsShow()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = AdResult;
        Advertisement.Show("rewardedVideo" , options);
    }

    public void BackToLandLoseMenu()
    {
        _backToLandLoseMenuImage.enabled = true;
        _backToLandLoseText.enabled = true;
        _chimpionshipBeltImage.enabled = false;
		_continueButtonImage.enabled = true;
		_highScoreDisplayText.enabled = false;
		_highScoreValueText.enabled = false;
        _muteButtonImage.enabled = false;
        _pauseButtonImage.enabled = false;
        _unmuteButtonImage.enabled = false;
        Screen.orientation = ScreenOrientation.Landscape;
        Time.timeScale = 0;
    }

    public void BackToLandWinMenu()
    {
        _backToLandWinMenuImage.enabled = true;
        _backToLandWinText.enabled = true;
        _chimpionshipBeltImage.enabled = false;
		_continueButtonImage.enabled = true;
		_highScoreDisplayText.enabled = false;
		_highScoreValueText.enabled = false;
        _muteButtonImage.enabled = false;
        _pauseButtonImage.enabled = false;
        _unmuteButtonImage.enabled = false;
        Screen.orientation = ScreenOrientation.Landscape;
        Time.timeScale = 0;
    }

    public void BackToLandWithSuperMenu()
    {
        _backToLandWithSuperMenuImage.enabled = true;
        _backToLandWithSuperText.enabled = true;
        _continueButtonImage.enabled = true;
        _chimpionshipBeltImage.enabled = false;
		_highScoreDisplayText.enabled = false;
		_highScoreValueText.enabled = false;
        _muteButtonImage.enabled = false;
        _pauseButtonImage.enabled = false;
        _unmuteButtonImage.enabled = false;
        Screen.orientation = ScreenOrientation.Landscape;
        Time.timeScale = 0;
    }

    public void ContinueButton()
    {
		BhanuPrefs.SetSupers(ScoreManager.m_supersCount);
		Screen.orientation = ScreenOrientation.Landscape;
        SceneManager.LoadScene("LandRunner");
    }

    void EndFlash()
	{
		m_selfiePanelImage.enabled = false;
	}

    public void ExitButton()
	{
        SceneManager.LoadScene("MainMenu");
    }

    void FBAppLinkURL(IAppLinkResult applinkResult) //Not sure how to use this yet so not using for now
    {
        if(!string.IsNullOrEmpty(applinkResult.Url))
        {
            _applinkURL = "" + applinkResult.Url + "";
            Debug.Log(_applinkURL);
        }
        else
        {
            _applinkURL = "http://uabhanu.wixsite.com/portfolio";
        }
    }

    public void FBChallengePlayers()
    {
        FB.AppRequest
        (
            "Come and try to be the Chimpion :) " ,
            null ,
            new List<object> { "app_users" } ,
            null ,
            null ,
            null ,
            null ,
            FBChallengePlayersCallback
        );

        Screen.orientation = ScreenOrientation.Portrait;
    }

    void FBChallengePlayersCallback(IAppRequestResult appRequestResult)
    {
        if(appRequestResult.Cancelled)
        {
            //Debug.LogWarning("Sir Bhanu, You have cancelled the invite");
            Screen.orientation = ScreenOrientation.Landscape;
        }

        else if(!string.IsNullOrEmpty(appRequestResult.Error))
        {
            //Debug.LogError("Sir Bhanu, There is a problem : " + appRequestResult.Error);
            Screen.orientation = ScreenOrientation.Landscape;
        }

        else if(!string.IsNullOrEmpty(appRequestResult.RawResult))
        {
            //Debug.LogWarning("Sir Bhanu, Your invitation : " + appRequestResult.RawResult);

            Screen.orientation = ScreenOrientation.Landscape;

            if(!_isFBShareTestMode)
            {
                ScoreManager.m_supersCount++;
                BhanuPrefs.SetSupers(ScoreManager.m_supersCount);
            }

            //_fbChallengeInviteSuccessMenuObj.SetActive(true);
        }
    }

    void FBInit()
    {
        if(!FB.IsInitialized)
        {
            HideUnityDelegate FBOnHideUnity = null;
            InitDelegate FBSetInit = null;
            FB.Init(FBSetInit , FBOnHideUnity);
        }
    }

    void FBLoggedIn()
    {
        FB.API("/me?fields=first_name" , HttpMethod.GET , FBUsernameDisplay);
        FB.API("/me/picture?type=square&height=480&width=480" , HttpMethod.GET , FBProfilePicDisplay);

        if(_fbLoggedInObj != null && _fbLoggedOutObj != null)
        {
            _fbLoggedInObj.SetActive(true);
            _fbLoggedOutObj.SetActive(false);
        }
        else
        {
            Debug.LogError("Sir Bhanu, FB Logged In & Out Objs no longer exist but you can ignore them :)");
        }

        Screen.orientation = ScreenOrientation.Landscape;
    }

    void FBLoggedOut()
	{
		if(_fbLoggedInObj != null && _fbLoggedOutObj != null)
        {
            _fbLoggedInObj.SetActive(false);
		    _fbLoggedOutObj.SetActive(true);		
        }
        else
        {
            Debug.LogError("Sir Bhanu, FB Logged In & Out Objs no longer exist but you can ignore them :)");
        }

        Screen.orientation = ScreenOrientation.Landscape;
	}

    void FBLogIn()
    {
        if(FB.IsInitialized)
        {
            _noInternetText.enabled = false;
            List<string> permissions = new List<string>();
            FB.LogInWithReadPermissions(permissions , FBLogInCallBack);
        }
    }

    void FBLoginButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        FBLogIn();
        Invoke("FBLoggedIn" , 0.2f);
    }

    void FBLogInCallBack(IResult logInResult)
    {
        if(logInResult.Cancelled)
        {
            Debug.LogWarning("Sir Bhanu, You have cancelled the LogIn");
            FBLoggedOut();
        }

        else if(!string.IsNullOrEmpty(logInResult.Error))
        {
            Debug.LogWarning("Sir Bhanu, You have pressed Error Button");
            FBLoggedOut();
        }

        else if(!string.IsNullOrEmpty(logInResult.RawResult))
        {
            Debug.LogWarning("Sir Bhanu, Your LogIn : " + logInResult.RawResult);
            FBLoggedIn();
        }
    }

    void FBLogInCheck()
    {
        if(FB.IsLoggedIn)
        {
            FBLoggedIn();
        }
        else
        {
            FBLoggedOut();
            Invoke("FBLogInCheck" , 0.2f);
        }
    }

    void FBOnHideUnity(bool isGameShown)
	{
		if(!isGameShown) 
		{
			Time.timeScale = 0;
		} 
		else 
		{
			Time.timeScale = 1;	
		}
	}

    void FBProfilePicDisplay(IGraphResult graphicResult)
    {
        try
        {
            if(graphicResult.Texture != null && graphicResult.Error == null)
            {
                _profilePicImage.sprite = Sprite.Create(graphicResult.Texture , new Rect(0 , 0 , graphicResult.Texture.width , graphicResult.Texture.height) , new Vector2());
                _profilePicEnabled = true; //This is used to check if sprite created properly and display only if it is, or else, _profilePicImage won't be enabled
                _profilePicImage.enabled = true;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    void FBSetInit()
    {
        if(FB.IsLoggedIn)
        {
            FBLoggedIn();
        }
        else
        {
            FBLoggedOut();
        }
    }

    public void FBShareButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        FB.ShareLink //TODO Do this if player has the relevant permission
        (
            contentTitle: "Fourth Lion Studios Message" ,
            contentURL: new Uri("http://uabhanu.wixsite.com/portfolio") , //TODO Game URL here when Live
            contentDescription: "We really hope you love the game" ,
            callback: FBShareCallback
        );
    }

    void FBShareCallback(IShareResult shareResult)
    {
        if(shareResult.Cancelled)
        {
            Debug.LogWarning("Sir Bhanu, you have cancelled the Share :)");
            Screen.orientation = ScreenOrientation.Landscape;
        }

        else if(!string.IsNullOrEmpty(shareResult.Error))
        {
            Debug.LogError("Sir Bhanu, You have pressed error button");
        }

        else
        {
            Debug.LogWarning("Sir Bhanu, Your Share is a success :)");
            Screen.orientation = ScreenOrientation.Landscape;

            if(!_isFBShareTestMode)
            {
                ScoreManager.m_supersCount++;
                BhanuPrefs.SetSupers(ScoreManager.m_supersCount);
            }

            _fbShareSuccessMenuObj.SetActive(true);
            _gpgsLogInButtonImage.enabled = false;
            _gpgsLogInTestText.enabled = false;
            _rateButtonImage.enabled = false;

        }
    }

    void FBUsernameDisplay(IResult usernameResult)
    {
        if(usernameResult.Error == null && m_currentScene == 0)
        {
            _usernameText.text = "Hi " + usernameResult.ResultDictionary["first_name"];
        }
        else
        {
            FBLoggedOut();
        }
    }

    void GetBhanuObjects()
    {
        m_currentScene = SceneManager.GetActiveScene().buildIndex;

        //if(_isFBInviteTestMode)
        //{
        //    _fbChallengeInviteTestText.enabled = true;
        //}

        if(_isFBShareTestMode)
        {
            _fbShareTestMenuObj.SetActive(true);
        }

        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(true);
        }

        if(_isGPGsLogInTestMode)
        {
            _gpgsLogInTestText.enabled = true;
        }

        if(m_isMemoryLeakTestingMode)
        {
            if(m_currentScene == 1)
            {
                _fallingLevelImage.enabled = true;
                _memoryLeakTestText.enabled = true;
                _waterLevelImage.enabled = true;
            }

            if(m_currentScene == 2)
            {
                _fallingLevelImage.enabled = true;
                _landLevelImage.enabled = true;
                _memoryLeakTestText.enabled = true;
            }

            if(m_currentScene == 3)
            {
                _landLevelImage.enabled = true;
                _memoryLeakTestText.enabled = true;
                _waterLevelImage.enabled = true;
            }
        }

        m_playerMutedSounds = BhanuPrefs.GetSoundsStatus();

        if(m_currentScene == 0)
        {
            _noInternetText = GameObject.Find("NoInternetText").GetComponent<Text>();
            _playButtonImage = GameObject.Find("PlayButton").GetComponent<Image>();
            _quitText = GameObject.Find("QuitText").GetComponent<Text>();
            _quitButtonImage = GameObject.Find("QuitButton").GetComponent<Image>();
            _quitAcceptButtonImage = GameObject.Find("QuitAcceptButton").GetComponent<Image>();
            _quitCancelButtonImage = GameObject.Find("QuitCancelButton").GetComponent<Image>();
            _quitMenuImage = GameObject.Find("QuitMenu").GetComponent<Image>();
        }

        else if(m_currentScene == 1)
        {
            _adsText = GameObject.Find("AdsText").GetComponent<Text>();
            _adsAcceptButtonImage = GameObject.Find("AdsAcceptButton").GetComponent<Image>();
            _adsCancelButtonImage = GameObject.Find("AdsCancelButton").GetComponent<Image>();
            _adsMenuImage = GameObject.Find("AdsMenu").GetComponent<Image>();
            _chimpionshipBeltImage = GameObject.Find("ChimpionshipBelt").GetComponent<Image>();
            _exitButtonImage = GameObject.Find("ExitButton").GetComponent<Image>();
            _gpgsLeaderboardConfirmMenuImage = GameObject.Find("GPGsLeaderboardConfirmMenu").GetComponent<Image>();
            _gpgsLeaderboardOKButtonImage = GameObject.Find("OKButton").GetComponent<Image>();
            _gpgsLeaderboardSuccessOKButtonImage = GameObject.Find("SuccessOKButton").GetComponent<Image>();
            _gpgsLeaderboardUpdateAcceptButtonImage = GameObject.Find("UpdateAcceptButton").GetComponent<Image>();
            _gpgsLeaderboardUpdateCancelButtonImage = GameObject.Find("UpdateCancelButton").GetComponent<Image>();
            _gpgsLeaderboardUpdateText = GameObject.Find("UpdateText").GetComponent<Text>();
            _highScoreDisplayText = GameObject.Find("HighScoreTextDisplay").GetComponent<Text>();
			_highScoreValueText = GameObject.Find("HighScoreValueDisplay").GetComponent<Text>();
            _landChimp = GameObject.Find("LandChimp").GetComponent<LandChimp>();
            _muteButtonImage = GameObject.Find("MuteButton").GetComponent<Image>();
			_pauseButtonImage = GameObject.Find("PauseButton").GetComponent<Image>();
			_pauseMenuImage = GameObject.Find("PauseMenu").GetComponent<Image>();
            _restartText = GameObject.Find("RestartText").GetComponent<Text>();
			_resumeButtonImage = GameObject.Find("ResumeButton").GetComponent<Image>();
            _restartButtonImage = GameObject.Find("RestartButton").GetComponent<Image>();
            _restartAcceptButtonImage = GameObject.Find("RestartAcceptButton").GetComponent<Image>();
            _restartCancelButtonImage = GameObject.Find("RestartCancelButton").GetComponent<Image>();
            _restartMenuImage = GameObject.Find("RestartMenu").GetComponent<Image>();
			m_selfieButtonImage = GameObject.Find("SelfieButton").GetComponent<Image>();
			m_selfiePanelImage = GameObject.Find("SelfiePanel").GetComponent<Image>();
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            _unmuteButtonImage = GameObject.Find("UnmuteButton").GetComponent<Image>();

            if(MusicManager.m_musicSource != null)
            {
                if(!MusicManager.m_musicSource.isPlaying && m_playerMutedSounds == 0)
                {
                    MusicManager.m_musicSource.Play();
                    _muteButtonImage.enabled = true;
                    _soundManager.m_soundsSource.enabled = true;
                }

                else if(MusicManager.m_musicSource.isPlaying && m_playerMutedSounds == 0)
                {
                    _muteButtonImage.enabled = true;
                    _soundManager.m_soundsSource.enabled = true;
                }

                else if(MusicManager.m_musicSource.isPlaying && m_playerMutedSounds == 1)
                {
                    MusicManager.m_musicSource.Pause();
                    _soundManager.m_soundsSource.enabled = false;
                    _unmuteButtonImage.enabled = true;
                }

                else
                {
                    _soundManager.m_soundsSource.enabled = false;
                    _unmuteButtonImage.enabled = true;
                }
            }
        }

		else
		{
			_backToLandLoseMenuImage = GameObject.Find("BackToLandLoseMenu").GetComponent<Image>();
			_backToLandLoseText = GameObject.Find("BackToLandLose").GetComponent<Text>();
			_backToLandWinMenuImage = GameObject.Find("BackToLandWinMenu").GetComponent<Image>();
			_backToLandWinText = GameObject.Find("BackToLandWin").GetComponent<Text>();
            _backToLandWithSuperMenuImage = GameObject.Find("BackToLandWithSuperMenu").GetComponent<Image>();
			_backToLandWithSuperText = GameObject.Find("BackToLandWithSuper").GetComponent<Text>();
            _chimpionshipBeltImage = GameObject.Find("ChimpionshipBelt").GetComponent<Image>();
			_continueButtonImage = GameObject.Find("ContinueButton").GetComponent<Image>();
			_highScoreDisplayText = GameObject.Find("HighScoreTextDisplay").GetComponent<Text>();
			_highScoreValueText = GameObject.Find("HighScoreValueDisplay").GetComponent<Text>();
            _muteButtonImage = GameObject.Find("MuteButton").GetComponent<Image>();
			_pauseButtonImage = GameObject.Find("PauseButton").GetComponent<Image>();
			_pauseMenuImage = GameObject.Find("PauseMenu").GetComponent<Image>();
			_resumeButtonImage = GameObject.Find("ResumeButton").GetComponent<Image>();
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            _unmuteButtonImage = GameObject.Find("UnmuteButton").GetComponent<Image>();

            if(MusicManager.m_musicSource != null)
            {
                if(!MusicManager.m_musicSource.isPlaying && m_playerMutedSounds == 0)
                {
                    MusicManager.m_musicSource.Play();
                    _muteButtonImage.enabled = true;
                    _soundManager.m_soundsSource.enabled = true;
                }

                else if(MusicManager.m_musicSource.isPlaying && m_playerMutedSounds == 0)
                {
                    _muteButtonImage.enabled = true;
                    _soundManager.m_soundsSource.enabled = true;
                }

                else if(MusicManager.m_musicSource.isPlaying && m_playerMutedSounds == 1)
                {
                    MusicManager.m_musicSource.Pause();
                    _soundManager.m_soundsSource.enabled = false;
                    _unmuteButtonImage.enabled = true;
                }

                else
                {
                    _soundManager.m_soundsSource.enabled = false;
                    _unmuteButtonImage.enabled = true;
                }
            }
		}

        Time.timeScale = 1;
    }

    public void GoToFallingLevelButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("FallingDown");
    }

    public void GoToLandLevelButton()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        SceneManager.LoadScene("LandRunner");
    }

    public void GoToWaterLevelButton()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        SceneManager.LoadScene("WaterSwimmer");
    }

    void GPGsInit()
    {
        _gpgsLogInButtonTapped = false;
        PlayGamesClientConfiguration clientConfig = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(clientConfig);
        PlayGamesPlatform.Activate();
    }

    public void GPGsLeaderboardButton()
    {
        if(!_adsMenuImage.enabled && !_gpgsLeaderboardLogInCheckText.enabled)
        {
            _gpgsLeaderboardUpdateAcceptButtonImage.enabled = true;
            _gpgsLeaderboardUpdateCancelButtonImage.enabled = true;
            _gpgsLeaderboardConfirmMenuImage.enabled = true;
            _gpgsLeaderboardUpdateText.enabled = true;
            
            if(_isGPGsLeaderboardTestMode)
            {
                _gpgsLeaderboardTestMenuObj.SetActive(false);
            }

            _pauseButtonImage.enabled = false;
            Time.timeScale = 0;
        }
    }

    public void GPGsLeaderboardOKButton()
    {
        _gpgsLeaderboardConfirmMenuImage.enabled = false;
        _gpgsLeaderboardLogInCheckText.enabled = false;
        _gpgsLeaderboardOKButtonImage.enabled = false;
        Time.timeScale = 1;
    }

    public void GPGsLeaderboardScoreGet()
    {
        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestText.text = ScoreManager.m_myScores;
        }
    }

    public void GPGsLeaderboardScoreSet()
    {
        if(_isGPGsLeaderboardTestMode)
        {
            PlayGamesPlatform.Instance.ReportScore((long)ScoreManager.m_scoreValue , _leaderboardID , (bool success) =>
            {
                _gpgsLeaderboardTestText.text = "Score Update : " + success;
            });
        }
    }

    public void GPGsLeaderboardSuccessOKButton()
    {
        _gpgsLeaderboardConfirmMenuImage.enabled = false;
        _gpgsLeaderboardSuccessOKButtonImage.enabled = false;
        _gpgsLeaderboardUpdateSuccessText.enabled = false;
        Time.timeScale = 1;
    }

    public void GPGsLeaderboardTestMenuAppear()
    {
        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(true);   
        }
    }

    public void GPGsLeaderboardTestMenuDisappear()
    {
        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(false);
        }
    }

    public void GPGsLeaderboardUpdateAcceptButton()
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated) 
        {
            PlayGamesPlatform.Instance.ReportScore((long)ScoreManager.m_scoreValue , _leaderboardID , (bool success) =>
            {
                if(success)
                {
                    _gpgsLeaderboardLogInCheckText.text = "Score Update : " + success;
                    _gpgsLeaderboardConfirmMenuImage.enabled = true;
                    _gpgsLeaderboardSuccessOKButtonImage.enabled = true;
                    _gpgsLeaderboardUpdateSuccessText.enabled = true;
                }
                else
                {
                    Debug.LogError("Sir Bhanu, You may want to check if you are logged in :)");
                    _gpgsLeaderboardConfirmMenuImage.enabled = true;
                    _gpgsLeaderboardSuccessOKButtonImage.enabled = true;
                    _gpgsLeaderboardUpdateSuccessText.text = "Leaderboard could not be updated, please try again later :)";
                    _gpgsLeaderboardUpdateSuccessText.enabled = true;
                }
            });

            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else 
        {
          //Debug.LogError("Sir Bhanu, Please make sure you are logged in first :) ");
          _gpgsLeaderboardUpdateAcceptButtonImage.enabled = false;
          _gpgsLeaderboardUpdateCancelButtonImage.enabled = false;
          _gpgsLeaderboardUpdateText.enabled = false;
          _gpgsLeaderboardOKButtonImage.enabled = true;
          _gpgsLeaderboardLogInCheckText.enabled = true;
            //Time.timeScale = 1;
        }
    }

    public void GPGsLeaderboardUpdateCancelButton()
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated) 
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }

        _gpgsLeaderboardUpdateAcceptButtonImage.enabled = false;
        _gpgsLeaderboardUpdateCancelButtonImage.enabled = false;
        _gpgsLeaderboardConfirmMenuImage.enabled = false;
        _gpgsLeaderboardUpdateText.enabled = false;
        _gpgsLeaderboardLogInCheckText.enabled = false;
        
        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(true);
        }

        _pauseButtonImage.enabled = true;
        Time.timeScale = 1;
    }

    void GPGsLoggedIn()
    {
        if(_gpgsLoggedInObj != null && _gpgsLoggedOutObj != null)
        {
            _gpgsLoggedInObj.SetActive(true);
            _gpgsLoggedOutObj.SetActive(false);
        }
        else
        {
            Debug.LogError("Sir Bhanu, GPGs Logged In & Logged Out Objs no longer exist but you can ignore them :) ");
        }

        if(_isGPGsLogInTestMode)
        {
            _gpgsLogInTestText.text = "Log In Success :)";
        }
    }

    void GPGsLoggedOut()
    {
        if(_gpgsLoggedInObj != null && _gpgsLoggedOutObj != null)
        {
            _gpgsLoggedInObj.SetActive(false);
            _gpgsLoggedOutObj.SetActive(true);
        }
        else
        {
            Debug.LogError("Sir Bhanu, GPGs Logged In & Logged Out Objs no longer exist but you can ignore them :) ");
        }

        if(_gpgsLogInButtonTapped && _isGPGsLogInTestMode)
        {
            _gpgsLogInTestText.text = "Log In Failed :(";
            _gpgsLogInButtonTapped = false;
        }
    }

    void GPGsLogIn() 
    {
        if(!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate(GPGsLogInCallback);
        }
    }

    public void GPGsLogInButton()
    {
        _gpgsLogInButtonTapped = true;
        GPGsLogIn();
    }

    void GPGsLogInCallback(bool success) 
    {  
        if(success)
        {
            GPGsLoggedIn();
        }
        else
        {
            GPGsLoggedOut();
        } 
    }

    void GPGsLogInCheck()
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated)
        {
            GPGsLoggedIn();
        }
        else
        {
            GPGsLoggedOut();
            Invoke("GPGsLogInCheck" , 0.2f);
        }
    }

    public void GPRateButton()
    {
        Application.OpenURL("http://uabhanu.wixsite.com/portfolio"); //TODO Game Play Store URL here after it's live
    }

    public void MuteUnmuteButton()
    {
        if(MusicManager.m_musicSource != null)
        {
            if(_muteButtonImage.enabled)
            {
                _muteButtonImage.enabled = false;
                MusicManager.m_musicSource.Pause();
                m_playerMutedSounds = 1;
                BhanuPrefs.SetSoundsStatus(m_playerMutedSounds);
                _unmuteButtonImage.enabled = true;
                _soundManager.m_soundsSource.enabled = false;
            }

            else if(!_muteButtonImage.enabled)
            {
                _muteButtonImage.enabled = true;
                MusicManager.m_musicSource.Play();
                m_playerMutedSounds = 0;
                BhanuPrefs.SetSoundsStatus(m_playerMutedSounds);
                _unmuteButtonImage.enabled = false;
                _soundManager.m_soundsSource.enabled = true;
            }
        }
    }

    public void OKInviteButton()
    {
        //_fbChallengeInviteSuccessMenuObj.SetActive(false);
    }

    public void OKShareButton()
    {
        _fbShareSuccessMenuObj.SetActive(false);
        _gpgsLogInButtonImage.enabled = true;
        
        if(_isGPGsLogInTestMode)
        {
            _gpgsLogInTestText.enabled = true;
        }
        
        _rateButtonImage.enabled = true;
    }

    public void PauseButton()
	{
        if(MusicManager.m_musicSource != null)
        {
            if(MusicManager.m_musicSource.isPlaying)
            {
                MusicManager.m_musicSource.Pause();
                _muteButtonImage.enabled = false;
            }
            else
            {
                _unmuteButtonImage.enabled = false;
            }
        }

        _chimpionshipBeltImage.enabled = false;
        
        if(_exitButtonImage != null)
        {
            _exitButtonImage.enabled = true;
        }

        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(false);
        }

		_highScoreDisplayText.enabled = false;
		_highScoreValueText.enabled = false;
        _leaderboardButtonImage.enabled = false;
        
        if(_restartButtonImage != null)
        {
            _restartButtonImage.enabled = true;
        }
        
		_pauseButtonImage.enabled = false;
		_pauseMenuImage.enabled = true;
		_resumeButtonImage.enabled = true;

		if(m_selfiePanelImage != null)
		{
			m_selfieButtonImage.enabled = false;	
		}

		Time.timeScale = 0;
	}

	public void PlayButton()
	{
		SceneManager.LoadScene("LandRunner");
	}

	public void QuitButton()
	{
        _facebookButtonImage.enabled = false;
        _fbShareTestMenuObj.SetActive(false);
        _gpgsLogInButtonImage.enabled = false;
        _gpgsLogInTestText.enabled = false;

        if(_rateButtonImage != null)
        {
            _rateButtonImage.enabled = false;
        }
        
        _noInternetText.enabled = false;
        _playButtonImage.enabled = false;
        _profilePicImage.enabled = false;
		_quitButtonImage.enabled = false;
        _usernameText.enabled = false;

		_quitMenuImage.enabled = true;
		_quitAcceptButtonImage.enabled = true;
		_quitCancelButtonImage.enabled = true;
		_quitText.enabled = true;
	}

	public void QuitAcceptButton()
	{
		Debug.Log("Quit Game");
		Application.Quit();
	}

	public void QuitCancelButton()
	{
        _facebookButtonImage.enabled = true;

        if(_isFBShareTestMode)
        {
            _fbShareTestMenuObj.SetActive(true);
        }
        
        _gpgsLogInButtonImage.enabled = true;

        if(_isGPGsLogInTestMode)
        {
            _gpgsLogInTestText.enabled = true;
        }

        if(_rateButtonImage != null)
        {
            _rateButtonImage.enabled = true;
        }

		_playButtonImage.enabled = true;

        if(_profilePicEnabled)
        {
            _profilePicImage.enabled = true;
        }
        
		_quitButtonImage.enabled = true;
        _usernameText.enabled = true;

		_quitMenuImage.enabled = false;
		_quitAcceptButtonImage.enabled = false;
		_quitCancelButtonImage.enabled = false;
		_quitText.enabled = false;
	}

	public void RestartButton()
	{
		_exitButtonImage.enabled = false;
		_pauseMenuImage.enabled = false;
		_restartButtonImage.enabled = false;
		_resumeButtonImage.enabled = false;

		_restartMenuImage.enabled = true;
		_restartAcceptButtonImage.enabled = true;
		_restartCancelButtonImage.enabled = true;
		_restartText.enabled = true;
	}

	public void RestartAcceptButton()
	{
        if(MusicManager.m_musicSource != null)
        {
            MusicManager.m_musicSource.Play();
        }
        
        BhanuPrefs.DeleteAll();
		ScoreManager.m_supersCount = ScoreManager.m_defaultSupersCount;
		BhanuPrefs.SetSupers(ScoreManager.m_supersCount);
		SceneManager.LoadScene(m_currentScene);
	}

    public void RestartCancelButton()
	{
		_exitButtonImage.enabled = true;
		_pauseMenuImage.enabled = true;
		_restartButtonImage.enabled = true;
		_resumeButtonImage.enabled = true;

		_restartMenuImage.enabled = false;
		_restartAcceptButtonImage.enabled = false;
		_restartCancelButtonImage.enabled = false;
		_restartText.enabled = false;
	}

	public void ResumeButton()
	{
        if(MusicManager.m_musicSource != null)
        {
            if(m_playerMutedSounds == 0)
            {
                MusicManager.m_musicSource.Play();
                _muteButtonImage.enabled = true;
            }

            else if(m_playerMutedSounds == 1)
            {
                _unmuteButtonImage.enabled = true;
            }
        }

        _chimpionshipBeltImage.enabled = true;

        if(_exitButtonImage != null)
        {
            _exitButtonImage.enabled = false;
        }

        if(_isGPGsLeaderboardTestMode)
        {
            _gpgsLeaderboardTestMenuObj.SetActive(true);
        }

		_highScoreDisplayText.enabled = true;
		_highScoreValueText.enabled = true;
        _leaderboardButtonImage.enabled = true;
		_pauseButtonImage.enabled = true;
		_pauseMenuImage.enabled = false;

        if(_restartButtonImage != null)
        {
            _restartButtonImage.enabled = false;
        }

        _resumeButtonImage.enabled = false;

		Time.timeScale = 1;
	}

	public void SelfieButton()
	{
		_soundManager.m_soundsSource.clip = _soundManager.m_selfie;
		
        if(_soundManager.m_soundsSource.enabled)
        {
            _soundManager.m_soundsSource.Play();
        }

		m_selfieButtonImage.enabled = false;

		if(_selfieFlashEnabled)
		{
			m_selfiePanelImage.enabled = true;
			Invoke("EndFlash" , 0.25f);
		}

		if(_landChimp.m_isSlipping)
        {
            ScoreManager.m_scoreValue += 60;
        }

        else if(_landChimp.m_isSuper) 
		{
			ScoreManager.m_scoreValue += 200;
		} 

		else 
		{
			ScoreManager.m_scoreValue += 20;
		}

		BhanuPrefs.SetHighScore(ScoreManager.m_scoreValue);
        ScoreManager.m_scoreDisplay.text = ScoreManager.m_scoreValue.ToString();
	}
}
