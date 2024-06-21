using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System;
using System.Globalization;
using System.Text;

public static class Utils {

    public static float GetRandomFloatBetween(float a, float b)
    {
        return (float)UnityEngine.Random.Range(a * 10, b * 10) / 10;
    }
    public static bool IsValidEmailAddress(string s)
    {
        var regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(s);
    }
    public static void RemoveAllChildsIn(Transform container)
    {
        int num = container.transform.childCount;
        for (int i = 0; i < num; i++) UnityEngine.Object.DestroyImmediate(container.transform.GetChild(0).gameObject);
    }
    public static void Shuffle(List<int> texts)
    {
        if (texts.Count < 2) return;
        for (int a = 0; a < 100; a++)
        {
            int id = UnityEngine.Random.Range(1, texts.Count);
            int value1 = texts[0];
            int value2 = texts[id];
            texts[0] = value2;
            texts[id] = value1;
        }
    }
    public static bool FloatIsNearOtherFloat(float a, float b, float diff)
    {
        return (Mathf.Abs(a - b) < diff);
    }
    public static void Shuffle(AudioClip[] arr)
    {
        if (arr.Length < 2) return;
        for (int a = 0; a < 100; a++)
        {
            int id = UnityEngine.Random.Range(1, arr.Length);
            AudioClip value1 = arr[0];
            AudioClip value2 = arr[id];
            arr[0] = value2;
            arr[id] = value1;
        }
    }
    public static class CoroutineUtil
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
    public static string FormatNumbers(float num, bool toLetters = false)
    {
        if (toLetters)
            return ToFormattedString(num);
        else
        {
            int nums = (int)num;
            string s = nums.ToString("N0");
            return s.Replace(".", ",");

            //return string.Format("{0:#,#}", num);
           // return num.ToString("N0");
        }
    }
    public static List<FileInfo> GetFilesInFolder(string url)
    {
        List<FileInfo> arr = new List<FileInfo>();
        DirectoryInfo dir = new DirectoryInfo(url);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if (!f.Name.Contains(".meta"))
                arr.Add(f);
        }
        return arr;
    }
    public static string UnixTimeStampToDateTimeFromString(string text) // like: 2024-05-20T19:21:32.000000Z
    {
        string[] arr = text.Split("T");
        if (arr == null || arr.Length < 2) return text;

        string[] date = arr[0].Split("-");
        if (date == null || date.Length < 2) return text;

        Debug.Log("date0" + date[0]);
        Debug.Log("date1" + date[1]);
        Debug.Log("date2" + date[2]);

        string ordinal = "";
        int day = int.Parse(date[2]);
        switch (day)
        {
            case 1:
            case 21:
            case 31:
                ordinal = "st";
                break;
            case 2:
            case 22:
                ordinal = "nd";
                break;
            case 3:
            case 23:
                ordinal = "rd";
                break;
            default:
                ordinal = "th";
                break;
        }
        string dayString = day + ordinal;

        string monthString = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(int.Parse(date[1]));

        return monthString + " " + dayString + ", " + date[0];
    }
    public static string UnixTimeStampToDateTime(string _unixTimeStamp)
    {
        double unixTimeStamp = 0;
        double.TryParse(_unixTimeStamp, out unixTimeStamp);
        if ( unixTimeStamp > 0)
            return UnixTimeStampToDateTime(unixTimeStamp);
        return "";
    }
    public static string UnixTimeStampToHours(long unixTimeStamp)
    {
        System.DateTime dat_Time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        dat_Time = dat_Time.AddSeconds(unixTimeStamp/1000);
        int h = dat_Time.Hour;
        int m = dat_Time.Minute;
        return h + ":" + m;
    }
    public static string UnixTimeStampToDateTime(double unixTimeStamp)
    {
        System.DateTime dat_Time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        dat_Time = dat_Time.AddSeconds(unixTimeStamp/1000);

        string ordinal = "";
        switch (dat_Time.Day)
        {
            case 1:
            case 21:
            case 31:
                ordinal = "st";
                break;
            case 2:
            case 22:
                ordinal = "nd";
                break;
            case 3:
            case 23:
                ordinal = "rd";
                break;
            default:
                ordinal = "th"; 
                break;
        }
        if (dat_Time.Day < 10)
            return string.Format("{0:d}{2} {1:MMM yyyy}", dat_Time.Day, dat_Time, ordinal);
        else
            return string.Format("{0:dd}{1} {0:MMM yyyy}", dat_Time, ordinal);
    }
    public static string ToFormattedString(this double rawNumber)
    {
        string[] letters = new string[] { "", "K", "M", "B", "T", "P", "E", "Z", "Y", "KY", "MY", "BY", "TY", "PY", "EY", "ZY", "YY" };
        int prefixIndex = 0;
        while (rawNumber > 1000)
        {
            rawNumber /= 1000.0f;
            prefixIndex++;
            if (prefixIndex == letters.Length - 1)
            {
                break;
            }
        }
        string numberString = rawNumber.ToString();
        if (prefixIndex < letters.Length - 1)
        {
            numberString = ToThreeDigits(numberString);
        }

        string prefix = letters[prefixIndex];
        return $"{numberString}{prefix}";
    }
    private static string ToThreeDigits(string numString)
    {
        if (numString.Length > 4)
        {
            if (numString.Substring(0, 4).Contains("."))
                numString = numString.Substring(0, 5);
            else
                numString = numString.Substring(0, 4);
        }
        return numString;
    }

    public static string GetOrdinal(int num) {
        if (num <= 0) return num.ToString();

        switch (num % 100) {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10) {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    public static string SHA(string strToEncrypt)
    {
        using (SHA256 mySHA256 = SHA256.Create())
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] hashBytes = mySHA256.ComputeHash(bytes);
            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

            return hashString.PadLeft(32, '0');
        }
    }
    public static void Shuffle<T>(List<T> list)
    {
        System.Random _random = new System.Random();
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            T t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }

    public static void Shuffle<T>(T[] array)
    {
        System.Random _random = new System.Random();
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    public static void PrintColor(string color, object text, UnityEngine.Object cont = null)
    {
        if (cont != null)
            Debug.Log("<color=" + color + ">" + text + "</color>", context: cont);
        else
            Debug.Log("<color=" + color + ">" + text + "</color>");
    }
    public static Sprite GetSpriteFromTexture2d(Texture2D t2d)
    {
        if (t2d == null)
        {
            Debug.Log("No image");
            return null;
        }

        Rect rec = new Rect(0, 0, t2d.width, t2d.height);
        return Sprite.Create(t2d, rec, new Vector2(0.5f, 0.5f), 100f); 
    }

    static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }
    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    public static void ResizeRectTransform(RectTransform rt, float maxWidth, float original_h, float original_w)
    {
        float finalH = original_h * maxWidth / original_w;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalH);
    }
    public static void MatchOther(RectTransform rt, RectTransform other)
    {
        Vector2 myPrevPivot = rt.pivot;
        myPrevPivot = other.pivot;
        rt.position = other.position;

        rt.localScale = other.localScale;

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, other.rect.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, other.rect.height);
        rt.pivot = myPrevPivot;
    }
    public static string MaxLetters(string s, int maxCharacters, string stringToAdd = "...")
    {
        if(s.Length>maxCharacters)
            return s.Substring(0, maxCharacters)+ stringToAdd;
        return s;
    }
    public static string CleanString(string str)
    {
        Regex rich = new Regex(@"<[^>]*>");

        if (rich.IsMatch(str))
        {
            str = rich.Replace(str, string.Empty);
        }

        return str;
    }
    public static string RSAEncrypt(string key, string Debug_pubkey)
    {
        byte[] someThing = RSAEncrypt(Encoding.Unicode.GetBytes(key), Debug_pubkey);
        return Convert.ToBase64String(someThing);
    }
    public static byte[] RSAEncrypt(byte[] plaintext, string destKey)
    {
        byte[] encryptedData;
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(destKey);
        encryptedData = rsa.Encrypt(plaintext, RSAEncryptionPadding.Pkcs1);
        rsa.Dispose();
        return encryptedData;
    }

    public static byte[] RSADecrypt(byte[] ciphertext, string srcKey)
    {
        byte[] decryptedData;
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(srcKey);
        decryptedData = rsa.Decrypt(ciphertext, true);
        rsa.Dispose();
        return decryptedData;
    }
}