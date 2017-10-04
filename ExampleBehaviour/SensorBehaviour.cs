using UnityEngine;
using System;
using UnityEngine.UI;

public class SensorBehaviour : MonoBehaviour
{
	private AndroidJavaObject plugin;

    /// <summary>
    /// Сенсор
    /// </summary>
    public SensorName Sensor;


	void Start ()
	{
#if UNITY_ANDROID
        //Подключаем плагин сенсора
		plugin = new AndroidJavaClass("jp.kshoji.unity.sensor.UnitySensorPlugin").CallStatic<AndroidJavaObject>("getInstance");
        //Задаем параметры обновления сенсора в милисекундах
		plugin.Call("setSamplingPeriod", 100 * 1000);
        //Активируем сенсор
        ActivateSensor(Sensor);
#endif
	}

    /// <summary>
    /// При выходе из приложения отключить сенсоры
    /// </summary>
	void OnApplicationQuit ()
	{
#if UNITY_ANDROID
		if (plugin != null) {
			plugin.Call("terminate");
			plugin = null;
		}
#endif
	}

	void Update ()
	{
	    string s = "";

	    foreach (float f in Get_Sensor(Sensor))
	    {
	        s += f + " :: ";
	    }
        Debug.Log(s);
	}

    /// <summary>
    /// Получить показания сенсора
    /// </summary>
    /// <param name="sensor">Тип сенсора</param>
    /// <returns>Массив значений показаний сенсора</returns>
    public float[] Get_Sensor(SensorName sensor)
    {
#if UNITY_ANDROID
        if (plugin != null)
        {
            return plugin.Call<float[]>("getSensorValues", Enum.GetName(typeof(SensorName), sensor));
        }
#endif
        return null;
    }

    /// <summary>
    /// Активация сенсора.
    /// </summary>
    /// <param name="sensor"></param>
    public void ActivateSensor(SensorName sensor)
    {
#if UNITY_ANDROID
        plugin.Call("startSensorListening", Enum.GetName(typeof(SensorName), sensor));
#endif
    }

    /// <summary>
    /// Тип сенсора
    /// </summary>
    public enum SensorName {
        accelerometer,
        ambienttemperature,
        gamerotationvector,
        geomagneticrotationvector,
        gravity,
        gyroscope,
        gyroscopeuncalibrated,
        heartrate,
        light,
        linearacceleration,
        magneticfield,
        magneticfielduncalibrated,
        pressure,
        proximity,
        relativehumidity,
        rotationvector,
        significantmotion,
        stepcounter,
        stepdetector,
    };
}
