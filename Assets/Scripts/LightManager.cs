using UnityEngine;


//[ExecuteAlways]
public class LightManager : MonoBehaviour
{

	//References
	[SerializeField] private UnityEngine.Rendering.Universal.Light2D directionalLight;
	[SerializeField] private Gradient AmbientColor;
	//Variables
	[SerializeField, Range(0, 24)] private float timeOfDay;

	public int initialTime;
	public float speed = 600f;
	//Tiempo actual y maximo en segundos
	public string currentTimeText;
	private float currentTime;
	private float maxTime = 86400.0f;
	//Valores separados en horas y minutos para poder mostrarlo en texto
	private float currentHour;
	private float currentMinute;
	bool isNight = true;

	public AudioSource audioSource;
	private void Start()
	{
		currentTime = ((initialTime * 60) * 60);
	}


	private void Update()
	{

		//if (Application.isPlaying)
		//{
			TiempoAvanza();
			UpdateLighting(timeOfDay);
			UpdateRelojCanvas();
		//}
		//else
		//{
		//	UpdateLighting(timeOfDay / 24);
		//}
	}
	private void UpdateLighting(float timePercent)
	{

		if (directionalLight != null)
		{
			directionalLight.color = AmbientColor.Evaluate(timePercent);
		}

		if(currentHour > 8 && currentHour < 18 && isNight)
        {
			Debug.Log("FinalNoche");
			isNight = false;
			audioSource.Play();
			GameManager.Instance.transitionDay(isNight);
		}
		else if(currentHour > 18 && !isNight)
        {
			isNight = true;
			GameManager.Instance.transitionDay(isNight);
		}
	}


	void TiempoAvanza() //Controla frame por frame el tiempo
	{
		//float rot = (360 * (1 / maxTime)) * Time.deltaTime * speed;
		//directionalLight.transform.Rotate(rot, 0, 0);
		if (currentTime > maxTime)
		{
			currentTime -= maxTime;
		}
		currentTime += Time.deltaTime * speed;
		timeOfDay = (((currentTime * 100) / maxTime) / 100);
		SetTimeString();
	}

	void SetTimeString()
	{
		float sec2 = currentTime;
		currentTimeText = "";

		currentHour = Mathf.FloorToInt(sec2 / 3600);
		sec2 -= currentHour * 3600;

		currentMinute = Mathf.FloorToInt(sec2 / 60);
		sec2 -= currentMinute * 60;

		int seconds = Mathf.FloorToInt(sec2);
		sec2 -= currentHour;

		currentTimeText += currentHour.ToString() + ":" + currentMinute.ToString() + ":" + seconds.ToString();
	}

	void UpdateRelojCanvas()
    {
		GameManager.Instance.canvas.SetReloj(currentTime);
    }
}