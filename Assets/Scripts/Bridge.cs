using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using LightDev;
    public class NativeAPI
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void sendMessageToMobileApp(string message);
#endif
    }


    [System.Serializable]
    public class Attributes
    {
        public string id;
        public int level;
    }

    [System.Serializable]
    public class Asset
    {
        public string id;
        public Attributes[] attributes;

        public Asset(string peteId)
        {
            id = peteId;
            attributes = new Attributes[3];

            attributes[0] = new Attributes();
            attributes[0].id = peteId + "-health";
            attributes[0].level = 1;


            attributes[1] = new Attributes();
            attributes[1].id = peteId + "-bullet";
            attributes[1].level = 1;


            attributes[2] = new Attributes();
            attributes[2].id = peteId + "-combo";
            attributes[2].level = 1;
        }
    }

    [System.Serializable]
    public class Data
    {
        public List<Asset> assets;
        public string saveData;
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public int coins;
        public bool volumeBg = true;
        public bool volumeSfx = true;
        public int highScore = 0;
        public Data data;
        public static PlayerInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayerInfo>(jsonString);
        }
    }


    public class Bridge : MonoBehaviour
    {
        public PlayerInfo thisPlayerInfo;
        private static Bridge instance;
        public int coinsCollected = 0;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void setScore(int score);

         
        [DllImport("__Internal")]
        private static extern void buyAsset(string assetId);

        [DllImport("__Internal")]
        private static extern void updateCoins(int coinsChange);

        [DllImport("__Internal")]
        private static extern void updateExp(int expChange);
        
        [DllImport("__Internal")]
        private static extern void upgradeAsset(string assetID, string attributeID, int level);

        [DllImport("__Internal")]
        private static extern void load();

        [DllImport("__Internal")]
        private static extern void restart();

        [DllImport("__Internal")]
        private static extern void vibrate(bool isLong);
#endif

        public static Bridge GetInstance()
        {
            return instance;
        }

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
        
#endif
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                Debug.Log("Loaded");
#if UNITY_WEBGL && !UNITY_EDITOR
            load();
#endif

            }
            else
                Destroy(this);


        }


        public void AddExp(int exp)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            updateExp(exp);
#endif
        }

        public void GameLoaded()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            load();
#endif
        }

        public void ButtonPressed()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.azesmwayreactnativeunity.ReactNativeUnityViewManager"))
                {
                    jc.CallStatic("sendMessageToMobileApp", "The button has been tapped!");
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IOS && !UNITY_EDITOR
                NativeAPI.sendMessageToMobileApp("The button has been tapped!");
#endif
            }
        }

        public void SendScore(int score)
        {

            Debug.Log(coinsCollected + "sent coin");
#if UNITY_WEBGL && !UNITY_EDITOR
            updateCoins(coinsCollected);
#endif
#if UNITY_WEBGL && !UNITY_EDITOR

            setScore(score);
#elif UNITY_EDITOR
            Debug.Log("sendingscore" + score);
#endif
        }

        public void Mute()
        {
            SoundManager.instance.Mute();
        }

        public void Unmute()
        {
            SoundManager.instance.Unmute();
        }

        [ContextMenu("replay")]
        public void Replay()
        {
            GameManager.Instance.Restart();
            //SceneManager.LoadScene(0);
            //coinsCollected = 0;
        }

        public void SendInitialData(string json)
        {
            thisPlayerInfo = PlayerInfo.CreateFromJSON(json);
            Debug.Log(json);
            if (thisPlayerInfo.data.assets.Count == 0)
            {
                Debug.Log("buying default cannon");
                BuyPete("pete-1");
            }

            if (thisPlayerInfo.volumeSfx)
            {
                Silence("false");
            }
            else
            {
                Silence("true");

            }
            //Replay();
            //Events.CoinsCountChanged.Call();
        }

        public void AddCoin()
        {
            thisPlayerInfo.coins++;
        }

        public void UpdateCoins(int value)
        {
            thisPlayerInfo.coins += value;
            //coinsCollected += value;
            if (value > 0)
            {
                Debug.Log(value);
#if UNITY_WEBGL && !UNITY_EDITOR
            updateCoins(coinsCollected);
#endif
            }
        }

        public void CollectCoins(int value)
        {
            thisPlayerInfo.coins += value;
            coinsCollected += value;
            //Debug.Log(value);

        }


        public void BuyPete(string peteID)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
                    buyAsset(peteID);
#endif
            AddPete(peteID);
        }

        public void AddPete(string peteID)
        {
            Asset addedPete = new Asset(peteID);
            addedPete.id = peteID;



            Debug.Log("added nnew cannon " + addedPete.id);

            thisPlayerInfo.data.assets.Add(addedPete);
        }

        [ContextMenu("Do Something")]
        public void SendTextData()
        {
            //SendInitialData("{\"coins\": 123,\"playerData\": {\"shootPower\":25,\"shootSpeed\":20}}");
            SendInitialData("{\"coins\":3400,\"data\":{\"pete\":[{\"id\":\"pete-1\",\"attributes\":[{\"health\":8,\"bullet\":8,\"combo\":8}]}]}}");
            //SendInitialData("{\"coins\":34,\"data\":{\"petes\":[{\"id\":\"health-1\",\"attributes\":[{\"id\":\"bullet-1-speed\",\"level\":91},{\"id\":\"compo-1-power\",\"level\":92}]},{\"id\":\"bvb-cannon-2\",\"attributes\":[{\"id\":\"bvb-cannon-2-speed\",\"level\":3},{\"id\":\"bvb-cannon-2-power\",\"level\":2}]}]}}");
            //SendInitialData("{\"coins\": 123,\"data\": null}");
            //Debug.Log(JsonUtility.ToJson( thisPlayerInfo.data));
            //Debug.Log( thisPlayerInfo.data);

        }

        public int GetCoins()
        {
            return thisPlayerInfo.coins;
        }


        public void Silence(string silence)
        {
            if (silence == "true")
                AudioListener.pause = true;

            if (silence == "false")
                AudioListener.pause = false;

            System.Console.WriteLine("called silence " + silence);

        }
    }
