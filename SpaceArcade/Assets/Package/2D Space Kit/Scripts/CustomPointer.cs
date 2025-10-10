using SpaceArcade.Input;
using UnityEngine;
public class CustomPointer : MonoBehaviour
{
	[SerializeField] InputReader inputReader;
	[SerializeField] Texture pointerTexture; //The image for the pointer, generally a crosshair or dot.
	
	[SerializeField] bool pointerReturnsToCenter = false; //Pointer will drift to the center of the screen (Use this for joysticks)
	[SerializeField] bool instantSnapping = false; //If the pointer returns to the center, this will make it return to the center instantly when input is idle. Only works for joysticks
	[SerializeField] float centerSpeed = 5f; //How fast the pointer returns to the center.

	[SerializeField] bool invertYAxis = false; //Inverts the y axis.
	
	[SerializeField] float mouseSensitivityModifier = 1f; //Speed multiplier for the mouse.
	
	public static Vector2 pointerPosition; //Position of the pointer in screen coordinates.
	Vector2 _pointerDelta;
	
	void Awake() 
	{	
		pointerPosition = new Vector2 (Screen.width / 2, Screen.height / 2);
	}

	void OnEnable()
	{
		inputReader.PointerDeltaUpdate += HandlePointerDelta;
	}

	void OnDisable()
	{
		inputReader.PointerDeltaUpdate -= HandlePointerDelta;
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update()
	{
		float xAxis = _pointerDelta.x;
		float yAxis = _pointerDelta.y;
		
		if (invertYAxis) yAxis = -yAxis;
			
		pointerPosition += new Vector2(xAxis * mouseSensitivityModifier, yAxis * mouseSensitivityModifier);
		
		if (pointerReturnsToCenter) 
		{
			//If there's no input and instant snapping is on...
			if (xAxis == 0 && yAxis == 0 && instantSnapping)
			{
				pointerPosition = new Vector2 (Screen.width / 2, Screen.height / 2);
			} 
			else 
			{
				//Move pointer to the center (Will stop when it hits the deadzone)
				pointerPosition.x = Mathf.Lerp (pointerPosition.x, Screen.width / 2, centerSpeed * Time.deltaTime);
				pointerPosition.y = Mathf.Lerp (pointerPosition.y, Screen.height / 2, centerSpeed * Time.deltaTime);
			}
		}
		pointerPosition.x = Mathf.Clamp (pointerPosition.x, 0, Screen.width);
		pointerPosition.y = Mathf.Clamp (pointerPosition.y, 0, Screen.height);
	}

	void HandlePointerDelta(Vector2 pointerDelta)
	{
		_pointerDelta = pointerDelta;
	}
	
	void OnGUI() 
	{
		//Draw the pointer texture.
		if (pointerTexture != null)
			GUI.DrawTexture(new Rect(pointerPosition.x - (pointerTexture.width / 2), Screen.height - pointerPosition.y - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);

	}
}
