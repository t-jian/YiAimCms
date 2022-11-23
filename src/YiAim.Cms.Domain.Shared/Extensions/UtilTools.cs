using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace YiAim.Cms.Extensions;

public static class UtilTools
{
    /// <summary>
    /// The string time format is converted to DateTime
    /// </summary>
    /// <param name="time"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string time, DateTime defaultValue = default)
    {
        if (time.IsNullOrEmpty())
            return defaultValue;

        return DateTime.TryParse(time, out var dateTime) ? dateTime : defaultValue;
    }





    /// <summary>
    /// Check the ip address
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static bool IsIp(this string ip)
    {
        var regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

        return regex.IsMatch(ip);
    }

    /// <summary>
    /// Convert <paramref name="dic"/> to query string
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static string ToQueryString(this Dictionary<string, string> dic)
    {
        return dic.Select(x => $"{x.Key}={x.Value}").JoinAsString("&");
    }



    /// <summary>
    /// Generate random code
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateRandomCode(this int length)
    {
        int rand;
        char code;
        var randomcode = string.Empty;
        var random = new Random();

        for (int i = 0; i < length; i++)
        {
            rand = random.Next();

            if (rand % 3 == 0)
            {
                code = (char)('A' + (char)(rand % 26));
            }
            else
            {
                code = (char)('0' + (char)(rand % 10));
            }

            randomcode += code.ToString();
        }
        return randomcode;
    }


    /// <summary>
    /// Md5加密
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Md5(this string text)
    {
        return Md5(Encoding.UTF8.GetBytes(text));
    }

    public static string Md5(this byte[] by)
    {
        //MD5加密
        var md5 = MD5.Create();
        var bs = md5.ComputeHash(by);
        var sb = new StringBuilder();
        foreach (byte b in bs)
            sb.Append(b.ToString("x2"));
        //所有字符转为大写
        return sb.ToString().ToUpper();
    }

    /// <summary>
    /// HMAC-SHA1加密算法
    /// </summary>
    /// <param name="str">加密字符串</param>
    /// <returns></returns>
    public static string Sha1Encrypt(this string str)
    {
        var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.Default.GetBytes(str));
        //return BitConverter.ToString(hash).Replace("-", "");
        string byte2String = null;
        for (int i = 0; i < hash.Length; i++)
        {
            byte2String += hash[i].ToString("x2");
        }
        return byte2String;
    }
    /// <summary>
    /// AES加密 
    /// </summary>
    /// <param name="text">加密字符</param>
    /// <param name="encodingAESKey">加密的密码</param>
    /// <param name="appid">appId</param>
    /// <returns></returns>
    public static string AESEncrypt(string text, string encodingAESKey, string appid)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        byte[] key;
        key = Convert.FromBase64String(encodingAESKey + "=");
        byte[] iv = new byte[16];
        Array.Copy(key, iv, 16);

        #region 生成随机值
        string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
        string[] arr = codeSerial.Split(',');
        string code = "";
        int randValue = -1;
        Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
        for (int i = 0; i < 16; i++)
        {
            randValue = rand.Next(0, arr.Length - 1);
            code += arr[randValue];
        }
        #endregion

        byte[] bRand = Encoding.UTF8.GetBytes(code);
        byte[] bAppid = Encoding.UTF8.GetBytes(appid);
        byte[] btmpMsg = Encoding.UTF8.GetBytes(text);

        int outval = 0, inval = btmpMsg.Length;
        for (int i = 0; i < 4; i++)
            outval = (outval << 8) + ((inval >> (i * 8)) & 255);

        byte[] bMsgLen = BitConverter.GetBytes(outval);
        byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppid.Length + btmpMsg.Length];

        Array.Copy(bRand, bMsg, bRand.Length);
        Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
        Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
        Array.Copy(bAppid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bAppid.Length);

        var aes = new RijndaelManaged();
        //秘钥的大小，以位为单位
        aes.KeySize = 256;
        //支持的块大小
        aes.BlockSize = 128;
        //填充模式
        //aes.Padding = PaddingMode.PKCS7;
        aes.Padding = PaddingMode.None;
        aes.Mode = CipherMode.CBC;
        aes.Key = key;
        aes.IV = iv;
        var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] xBuff = null;
        byte[] msg = new byte[bMsg.Length + 32 - bMsg.Length % 32];
        Array.Copy(bMsg, msg, bMsg.Length);

        #region 自己进行PKCS7补位，用系统自己带的不行，微信加密要使用这个
        int block_size = 32;
        // 计算需要填充的位数
        int amount_to_pad = block_size - (bMsg.Length % block_size);
        if (amount_to_pad == 0)
        {
            amount_to_pad = block_size;
        }
        // 获得补位所用的字符
        char pad_chr = (char)(byte)(amount_to_pad & 0xFF);
        string tmp = "";
        for (int index = 0; index < amount_to_pad; index++)
        {
            tmp += pad_chr;
        }
        byte[] pad = Encoding.UTF8.GetBytes(tmp);

        Array.Copy(pad, 0, msg, bMsg.Length, pad.Length);

        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
            {
                cs.Write(msg, 0, msg.Length);
            }
            xBuff = ms.ToArray();
        }
        #endregion

        #region 注释的也是一种方法，效果一样，微信加密不能使用这个！！！！
        //ICryptoTransform transform = aes.CreateEncryptor();
        //xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);
        #endregion

        string output = Convert.ToBase64String(xBuff);
        return output;
    }
    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="encryptText">密文</param>
    /// <param name="encodingAESKey">秘钥</param>
    /// <param name="appid"></param>
    /// <returns></returns>
    public static string AESDecrypt(string encryptText, string encodingAESKey, out string appid)
    {
        if (string.IsNullOrEmpty(encryptText))
        {
            appid = "";
            return encryptText;
        }

        byte[] key;
        key = Convert.FromBase64String(encodingAESKey + "=");
        byte[] iv = new byte[16];
        Array.Copy(key, iv, 16);
        byte[] btmpMsg = null;

        RijndaelManaged aes = new RijndaelManaged();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.None;
        aes.Key = key;
        aes.IV = iv;
        var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Convert.FromBase64String(encryptText);
                byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                Array.Copy(xXml, msg, xXml.Length);
                cs.Write(xXml, 0, xXml.Length);
            }
            var decrypted = ms.ToArray();
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            btmpMsg = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, btmpMsg, 0, decrypted.Length - pad);
        }

        int len = BitConverter.ToInt32(btmpMsg, 16);
        len = IPAddress.NetworkToHostOrder(len);


        byte[] bMsg = new byte[len];
        byte[] bAppid = new byte[btmpMsg.Length - 20 - len];
        Array.Copy(btmpMsg, 20, bMsg, 0, len);
        Array.Copy(btmpMsg, 20 + len, bAppid, 0, btmpMsg.Length - 20 - len);
        string oriMsg = Encoding.UTF8.GetString(bMsg);
        appid = Encoding.UTF8.GetString(bAppid);
        return oriMsg;
    }
    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="encryptText">密文</param>
    /// <param name="encodingAESKey">秘钥</param>
    /// <param name="appid"></param>
    /// <returns></returns>
    public static string AESDecrypt(string encryptText, string encodingAESKey)
    {
        return AESDecrypt(encryptText, encodingAESKey, out _);
    }
}
