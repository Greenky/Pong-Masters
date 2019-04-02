using UnityEngine.SceneManagement;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    [SerializeField] MenuSetup Menu;
	[SerializeField] private GameObject _menuPrefab;
	private GameManager _gameManager;
	private SteamVR_Controller.Device controllerDevice;
	[SerializeField] private GameObject _leftController;

    private int _state;
	private bool _touch = false;
	private bool _flagClick = false;

	private float _lastPosition = 0;


	private enum TouchPosition { Off, Up, Down, Left, Right };
	private TouchPosition _selectedPosition;

    private enum _menuTypes
    {
        MAIN,
        SOLO,
        ONLINE,
        SETTINGS,
        SETTINGS_MIC,
        SETTINGS_SOUND,
        SETTINGS_MUSIC,
        SERVER_LIST,
        PAUSE_SOLO,
        PAUSE_ONLINE
    }

    private _menuTypes _typeContentList;

    private void Start()
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_menuPrefab.SetActive(false);
		if (SceneManager.GetActiveScene().name == "GameWithCups" || SceneManager.GetActiveScene().name == "GameWithBot" || SceneManager.GetActiveScene().name == "AIScene")
		{
			_typeContentList = _menuTypes.PAUSE_SOLO;
			Menu.SetupPauseSolo();
		}
		else
		{
			_typeContentList = _menuTypes.MAIN;
			Menu.SetupMain();
		}

		_leftController.GetComponent<SteamVR_TrackedController>().PadTouched += OnPadTouched;
		_leftController.GetComponent<SteamVR_TrackedController>().PadUntouched += OnPadUnTouched;
		_leftController.GetComponent<SteamVR_TrackedController>().PadClicked += OnPadClicked;
		_leftController.GetComponent<SteamVR_TrackedController>().MenuButtonClicked += OnMenuButtonClicked;

	}

	private void OnMenuButtonClicked(object sender, ClickedEventArgs e)
	{
		if (!_flagClick)
		{
			if (SceneManager.GetActiveScene().name != "Lobby")
				Time.timeScale = 0;
			GameObject.Find("Racket").transform.parent = GameObject.Find("Controller (right)").transform;
			_flagClick = true;
			_menuPrefab.SetActive(true);
		}
		else
		{
			Time.timeScale = 1;
			GameObject.Find("Racket").transform.parent = GameObject.Find("_Dynamic").transform;
			_flagClick = false;
			_menuPrefab.SetActive(false);
		}
	}

	private void OnPadClicked(object sender, ClickedEventArgs e)
	{
		Debug.Log("CLICKED ONE");
		if (Time.timeScale == 0 || SceneManager.GetActiveScene().name == "Lobby")
			{
			Debug.Log("CLICKED TWO");
			switch (CurrentTouchPosition())
			{
				case TouchPosition.Up:
					if (_typeContentList == _menuTypes.MAIN)
					{
						Menu.SetupSolo();
						_typeContentList = _menuTypes.SOLO;
					}
					else if (_typeContentList == _menuTypes.SOLO)
					{
						_gameManager.OnLoadGameCups();
						_typeContentList = _menuTypes.PAUSE_SOLO;
					}
					else if (_typeContentList == _menuTypes.SETTINGS)
					{
						Menu.SetupSettingsSound();
						_typeContentList = _menuTypes.SETTINGS_SOUND;
					}
					else if (_typeContentList == _menuTypes.SETTINGS_SOUND)
					{
						Menu.SetupSettingsMusic();
						_typeContentList = _menuTypes.SETTINGS_MUSIC;
					}
					else if (_typeContentList == _menuTypes.PAUSE_SOLO)
					{
						_gameManager.OnLoadRestart();
						_typeContentList = _menuTypes.PAUSE_SOLO;
					}
					break;
				case TouchPosition.Down:
					if (_typeContentList == _menuTypes.MAIN)
					{
						Menu.SetupOnline();
						_typeContentList = _menuTypes.ONLINE;

					}
					else if (_typeContentList == _menuTypes.SOLO)
					{
						_gameManager.OnLoadGameBot();
						_typeContentList = _menuTypes.PAUSE_SOLO;
					}
					else if (_typeContentList == _menuTypes.SETTINGS)
					{
						if (Menu.bottomButtonVariant.GetComponent<ArchSetup>().text == "MIC_OFF")
						{
							Menu.bottomButton.GetComponent<ArchSetup>().text = "MIC_OFF";
							Menu.bottomButtonVariant.GetComponent<ArchSetup>().text = "MIC_ON";
						}
						else
						{
							Menu.bottomButton.GetComponent<ArchSetup>().text = "MIC_ON";
							Menu.bottomButtonVariant.GetComponent<ArchSetup>().text = "MIC_OFF";
						}
					}
					break;
				case TouchPosition.Left:
					if (_typeContentList == _menuTypes.SOLO)
					{
						Menu.SetupMain();
						_typeContentList = _menuTypes.MAIN;
					}
					else if (_typeContentList == _menuTypes.ONLINE)
					{
						Menu.SetupMain();
						_typeContentList = _menuTypes.MAIN;
					}
					else if (_typeContentList == _menuTypes.SETTINGS)
					{
						Menu.SetupMain();
						_typeContentList = _menuTypes.MAIN;
					}
					else if (_typeContentList == _menuTypes.SETTINGS_SOUND)
					{
						Menu.SetupSettings();
						_typeContentList = _menuTypes.SETTINGS;
					}
					else if (_typeContentList == _menuTypes.PAUSE_SOLO)
					{
						_gameManager.OnLoadGameMenu();
						_typeContentList = _menuTypes.MAIN;
					}
					break;
				case TouchPosition.Right:
					if (_typeContentList == _menuTypes.SOLO)
					{
						_gameManager.OnLoadAI();
						_typeContentList = _menuTypes.PAUSE_SOLO;
					}
					else if (_typeContentList == _menuTypes.MAIN)
					{
						Menu.SetupSettings();
						_typeContentList = _menuTypes.SETTINGS;
					}
					else if (_typeContentList == _menuTypes.SETTINGS_SOUND)
					{
						Menu.SetupSettingsMic();
						_typeContentList = _menuTypes.SETTINGS_MIC;
					}
					else if (_typeContentList == _menuTypes.SETTINGS)
					{
						if (Menu.rightButton.GetComponent<ArchSetup>().text == "RU")
						{
							Menu.rightButton.GetComponent<ArchSetup>().text = "EN";
							Menu.rightButtonVariant.GetComponent<ArchSetup>().text = "RU";
						}
						else
						{
							Menu.rightButton.GetComponent<ArchSetup>().text = "RU";
							Menu.rightButtonVariant.GetComponent<ArchSetup>().text = "EN";
						}
					}
					else if (_typeContentList == _menuTypes.PAUSE_SOLO)
					{
						
						
					}
					break;
			}
			Debug.Log(_typeContentList);
		}
	}

	private void OnPadTouched(object sender, ClickedEventArgs e)
	{
		_touch = true;
	}

	private void OnPadUnTouched(object sender, ClickedEventArgs e)
	{
		_touch = false;
	}

	private void Update()
    {
		if (GameObject.Find("Controller (left)"))
			controllerDevice = SteamVR_Controller.Input((int)GameObject.Find("Controller (left)").GetComponent<SteamVR_TrackedObject>().index);
		//CurrentTouchPosition();
		//if (_touch)
		//{
		//	CurrentTouchPosition();
		//	if (_selectedPosition == TouchPosition.Up)
		//	{
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//	}
		//	else if (_selectedPosition == TouchPosition.Down)
		//	{
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//	}
		//	else if (_selectedPosition == TouchPosition.Left)
		//	{
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//	}
		//	else if (_selectedPosition == TouchPosition.Right)
		//	{
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
		//	}
		//}
		//else
		//{
		//	CurrentTouchPosition();
		//	if (_selectedPosition == TouchPosition.Up)
		//	{
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//	}
		//	else if (_selectedPosition == TouchPosition.Down)
		//	{
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//	}
		//	else if (_selectedPosition == TouchPosition.Left)
		//	{
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//	}
		//	else if (_selectedPosition == TouchPosition.Right)
		//	{
		//		Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
		//		Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//		Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
		//	}
		//}

		if (Input.GetKeyDown("w"))
        {
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
        }
        else if(Input.GetKeyUp("w"))
        {
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");


			if (_typeContentList == _menuTypes.MAIN)
			{
				Menu.SetupSolo();
				_typeContentList = _menuTypes.SOLO;
			}
			else if (_typeContentList == _menuTypes.SOLO)
			{
				_gameManager.OnLoadGameCups();
				_typeContentList = _menuTypes.PAUSE_SOLO;
			}
			else if (_typeContentList == _menuTypes.SETTINGS)
			{
				Menu.SetupSettingsSound();
				_typeContentList = _menuTypes.SETTINGS_SOUND;
			}
			else if (_typeContentList == _menuTypes.SETTINGS_SOUND)
			{
				Menu.SetupSettingsMusic();
				_typeContentList = _menuTypes.SETTINGS_MUSIC;
			}

		}


        if (Input.GetKeyDown("s"))
        {
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
        }
        else if (Input.GetKeyUp("s"))
        {
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
			if (_typeContentList == _menuTypes.MAIN)
			{
				Menu.SetupOnline();
				_typeContentList = _menuTypes.ONLINE;

			}
			else if (_typeContentList == _menuTypes.SOLO)
			{
				_gameManager.OnLoadGameBot();
				_typeContentList = _menuTypes.PAUSE_SOLO;
			}
			else if (_typeContentList == _menuTypes.SETTINGS)
			{
				if (Menu.bottomButtonVariant.GetComponent<ArchSetup>().text == "MIC_OFF")
				{
					Menu.bottomButton.GetComponent<ArchSetup>().text = "MIC_OFF";
					Menu.bottomButtonVariant.GetComponent<ArchSetup>().text = "MIC_ON";
				}
				else
				{
					Menu.bottomButton.GetComponent<ArchSetup>().text = "MIC_ON";
					Menu.bottomButtonVariant.GetComponent<ArchSetup>().text = "MIC_OFF";
				}
				//_typeContentList = _menuTypes.PAUSE_SOLO;
			}
		}


        if (Input.GetKeyDown("a"))
        {
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
        }

        else if (Input.GetKeyUp("a"))
        {
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
			Debug.Log(_typeContentList);
			if (_typeContentList == _menuTypes.SOLO)
			{
				Menu.SetupMain();
				_typeContentList = _menuTypes.MAIN;
			}
			else if (_typeContentList == _menuTypes.ONLINE)
			{
				Menu.SetupMain();
				_typeContentList = _menuTypes.MAIN;
			}
			else if (_typeContentList == _menuTypes.SETTINGS)
			{
				Menu.SetupMain();
				_typeContentList = _menuTypes.MAIN;
			}
			else if (_typeContentList == _menuTypes.SETTINGS_SOUND)
			{
				Menu.SetupSettings();
				_typeContentList = _menuTypes.SETTINGS;
			}
			//else if (_typeContentList == _menuTypes.SETTINGS_MIC)
			//{
			//	Menu.SetupSettings();

			//	_typeContentList = _menuTypes.SETTINGS;
			//}
			//else if (_typeContentList == _menuTypes.SETTINGS_MUSIC)
			//{
			//	Menu.SetupSettingsSound();
			//	_typeContentList = _menuTypes.SETTINGS_SOUND;
			//}
		}

        if (Input.GetKeyDown("d"))
        {
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonEnterIn");
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitIn");

			
        }
        else if (Input.GetKeyUp("d"))
        {
            Menu.rightButton.GetComponent<Animator>().SetTrigger("ButtonEnterOut");
            Menu.topButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.bottomButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");
            Menu.leftButton.GetComponent<Animator>().SetTrigger("ButtonExitOut");

			if (_typeContentList == _menuTypes.SOLO)
			{
				_gameManager.OnLoadAI();
				_typeContentList = _menuTypes.PAUSE_SOLO;
			}
			else if (_typeContentList == _menuTypes.MAIN)
			{
				Menu.SetupSettings();
				_typeContentList = _menuTypes.SETTINGS;
			}
			else if (_typeContentList == _menuTypes.SETTINGS_SOUND)
			{
				Menu.SetupSettingsMic();
				_typeContentList = _menuTypes.SETTINGS_MIC;
			}
			else if (_typeContentList == _menuTypes.SETTINGS)
			{
				if (Menu.rightButton.GetComponent<ArchSetup>().text == "RU")
				{
					Menu.rightButton.GetComponent<ArchSetup>().text = "EN";
					Menu.rightButtonVariant.GetComponent<ArchSetup>().text = "RU";
				}
				else
				{
					Menu.rightButton.GetComponent<ArchSetup>().text = "RU";
					Menu.rightButtonVariant.GetComponent<ArchSetup>().text = "EN";
				}
				//_typeContentList = _menuTypes.PAUSE_SOLO;
			}
		}
	}


    private TouchPosition CurrentTouchPosition()
    {
        Vector2 pos = controllerDevice.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

        bool isTop = pos.y >= 0;
        bool isRight = pos.x >= 0;
		

		if (isTop && isRight)
		_selectedPosition = pos.y > pos.x ? TouchPosition.Up : TouchPosition.Right;
        else if(isTop && !isRight)
			_selectedPosition = pos.y > -pos.x ? TouchPosition.Up : TouchPosition.Left;
        else if(!isTop && isRight)
			_selectedPosition =  - pos.y > pos.x ? TouchPosition.Down : TouchPosition.Right;
        else if(!isTop && !isRight)
			_selectedPosition = - pos.y > -pos.x ? TouchPosition.Down : TouchPosition.Left;

		Debug.Log("Position: " + _selectedPosition);
		return _selectedPosition;

	}
}
	
	
