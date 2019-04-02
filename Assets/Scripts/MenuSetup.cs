using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSetup : MonoBehaviour
{
    public GameObject topButton;
    public GameObject bottomButton;
    public GameObject leftButton;
    public GameObject rightButton;
	public GameObject bottomButtonVariant;
	public GameObject rightButtonVariant;
	public GameObject scrollListCenter;



	[SerializeField] private Sprite _exitIcon;
    [SerializeField] private Sprite _settingsIcon;
	[SerializeField] private Sprite _scrollIcon;
    



    public void SetupMain()
    {
        topButton.SetActive(true);
        bottomButton.SetActive(true);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
		scrollListCenter.SetActive(false);

		topButton.GetComponent<ArchSetup>().text = "SOLO";
        topButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

        bottomButton.GetComponent<ArchSetup>().text = "ONLINE";
        bottomButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

        leftButton.GetComponent<ArchSetup>().image = _exitIcon;
        leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

        rightButton.GetComponent<ArchSetup>().image = _settingsIcon;
        rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;
    }
    public void SetupSolo()
    {
        topButton.SetActive(true);
        bottomButton.SetActive(true);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
		scrollListCenter.SetActive(false);

		topButton.GetComponent<ArchSetup>().text = "CUPS";
        topButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

        bottomButton.GetComponent<ArchSetup>().text = "ROBOT";
        bottomButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

        leftButton.GetComponent<ArchSetup>().image = _exitIcon;
        leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

        rightButton.GetComponent<ArchSetup>().text = "AI";
        rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;
    }

    public void SetupOnline()
    {
        topButton.SetActive(true);
        bottomButton.SetActive(false);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
		scrollListCenter.SetActive(false);

		topButton.GetComponent<ArchSetup>().text = "CREATE";
		topButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		rightButton.GetComponent<ArchSetup>().text = "JOIN";
		rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;
	}

    public void SetupPauseSolo()
    {
        topButton.SetActive(true);
        bottomButton.SetActive(false);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
		scrollListCenter.SetActive(false);

		topButton.GetComponent<ArchSetup>().text = "RESTART";
		topButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		rightButton.GetComponent<ArchSetup>().image = _settingsIcon;
		rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;
	}

	public void SetupPauseOnline()
    {
        topButton.SetActive(false);
        bottomButton.SetActive(false);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
		scrollListCenter.SetActive(false);

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		rightButton.GetComponent<ArchSetup>().image = _settingsIcon;
		rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

	}

	public void SetupSettings()
	{
		topButton.SetActive(true);
		bottomButton.SetActive(true);
		leftButton.SetActive(true);
		rightButton.SetActive(true);
		scrollListCenter.SetActive(false);

		topButton.GetComponent<ArchSetup>().text = "SOUND";
		topButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		bottomButton.GetComponent<ArchSetup>().text = "MIC_ON";
		bottomButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		bottomButtonVariant.GetComponent<ArchSetup>().text = "MIC_OFF";
		bottomButtonVariant.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		rightButton.GetComponent<ArchSetup>().text = "RU";
		rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		rightButtonVariant.GetComponent<ArchSetup>().text = "EN";
		rightButtonVariant.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;
	}

	public void SetupSettingsSound()
    {
        topButton.SetActive(true);
        bottomButton.SetActive(false);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
		scrollListCenter.SetActive(false);


		topButton.GetComponent<ArchSetup>().text = "MUSIC";
		topButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		rightButton.GetComponent<ArchSetup>().text = "MIC";
		rightButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.TEXT;
	}

	public void SetupSettingsMusic()
	{
		topButton.SetActive(false);
		bottomButton.SetActive(false);
		leftButton.SetActive(true);
		rightButton.SetActive(false);
		scrollListCenter.SetActive(true);

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		scrollListCenter.GetComponent<ArchSetup>().image = _scrollIcon;
		scrollListCenter.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;


	}

	public void SetupSettingsMic()
	{
		topButton.SetActive(false);
		bottomButton.SetActive(false);
		leftButton.SetActive(true);
		rightButton.SetActive(false);
		scrollListCenter.SetActive(true);

		leftButton.GetComponent<ArchSetup>().image = _exitIcon;
		leftButton.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;

		scrollListCenter.GetComponent<ArchSetup>().image = _scrollIcon;
		scrollListCenter.GetComponent<ArchSetup>().typeContentList = ArchSetup.typeContentOption.ICON;
	}

}
