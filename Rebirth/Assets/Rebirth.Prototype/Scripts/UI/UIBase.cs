using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

	public void Toggle(bool state)
	{
		gameObject.SetActive(state);
	}

	public void Toggle()
	{
		gameObject.SetActive(!State());
	}

	public bool State()
	{
		return gameObject.activeSelf;
	}
}
