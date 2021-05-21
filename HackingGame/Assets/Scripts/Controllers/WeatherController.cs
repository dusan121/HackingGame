using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SimpleJSON;

public class WeatherController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TryToFindCountryUsingIP());
    }

    public static string TAG_CITY = "city";
    public static string TAG_LATITUDE = "lat";
    public static string TAG_LONGITUDE = "lon";
    public static string GEO_LOCATION_LINK = "http://ip-api.com/json";
    public static string LINK_FOR_WEATHER = "api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}";
    public static string WEATHER_API_KEY = "bc31ce2a91c4568ab7cd6f0ba946a81d";
    public static string myCity;
    public static string myLatitude;
    public static string myLongitude;
    public float myWeatherTemperature;


    private IEnumerator TryToFindCountryUsingIP()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(GEO_LOCATION_LINK))
        {
            yield return www.Send();

            if (!www.isNetworkError)
            {
                JSONNode mainJsonObject = JSON.Parse(www.downloadHandler.text);
                myCity = mainJsonObject[TAG_CITY].Value;
                myLatitude = mainJsonObject[TAG_LATITUDE].Value;
                myLongitude = mainJsonObject[TAG_LONGITUDE].Value;
            }
        }


        using (UnityWebRequest www = UnityWebRequest.Get(string.Format(LINK_FOR_WEATHER, myLatitude, myLongitude, WEATHER_API_KEY)))
        {
            yield return www.Send();

            if (!www.isNetworkError)
            {
                JSONNode mainJsonObject = JSON.Parse(www.downloadHandler.text);
                string temp = mainJsonObject["main"]["temp"].Value;
                float tempFloat = float.Parse(temp);
                myWeatherTemperature = Mathf.Round((tempFloat - 273.0f) * 10) / 10;
            }
        }
        float b_segment = 0.013333333333333334f * myWeatherTemperature + 0.5f;
        Color newBackgroundColor = new Color(0.5f, 0.5f, b_segment, 1);
        CameraController.instance.SetCameraBackgroundColor(newBackgroundColor);
        yield return null;
    }
}

