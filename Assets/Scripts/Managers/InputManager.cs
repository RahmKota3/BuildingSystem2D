using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

	public Vector2 MousePosition { get; private set; }
	public Vector2Int RoundedMousePosition { get; private set; }

	public System.Action OnLeftMouseClicked;
	public System.Action OnRightMouseClicked;

	void CheckForMouseInput()
    {
		MousePosition = ReferenceManager.Instance.CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
		RoundedMousePosition = new Vector2Int(Mathf.RoundToInt(MousePosition.x), Mathf.RoundToInt(MousePosition.y));

		if (Input.GetMouseButtonDown(0))
			OnLeftMouseClicked?.Invoke();

		if (Input.GetMouseButtonDown(1))
			OnRightMouseClicked?.Invoke();
	}

	void CheckForKeyboardInput()
    {

    }

	void CheckForInput()
    {
		CheckForMouseInput();
		CheckForKeyboardInput();
    }

	void Awake()
	{
		Instance = this;
	}

    private void Update()
    {
		CheckForInput();
    }
}
