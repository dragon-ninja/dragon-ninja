using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class Encrypt
{
    //����AES���ܽ��ܲ���
    private static RijndaelManaged Setting()
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes("TestTestTestTest"), //������Կ,�Լ����ã����ȱ���Ϊ16�ֽڵı���
            IV = Encoding.UTF8.GetBytes("1234567812345678"),  //���ܵ�ivƫ����,���ȱ���Ϊ16�ֽڵı���
            Mode = CipherMode.CBC,       //����ģʽ��ECB��CBC��CFB��
            Padding = PaddingMode.PKCS7, //�����ܵ����ĳ��Ȳ���������ʱʹ�õ����ģʽ��PKCS7��python��Ĭ�ϵ����ģʽ
            BlockSize = 128              //���ܲ����Ŀ��С
        };
        return rijndaelCipher;
    }

    //�����ַ�����������Ϊbase64�����ʽ���ַ���
    public static string EncryptStr(string encryptStr)
    {
        string decryptStr = string.Empty;
        //try
        {
            //�������ܵ������ַ���תΪ����������ֽ������ʽ
            byte[] plainText = Encoding.UTF8.GetBytes(encryptStr);

            //�趨���ܲ���
            RijndaelManaged rijndaelCipher = Setting();

            //�����ַ���
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            //�����ܺ���ֽ�����תΪbase64��ʽ�ַ���
            decryptStr = Convert.ToBase64String(cipherBytes);
        }
       /* catch (Exception ex)
        {

        }*/
        return decryptStr;
    }

    //����base64�����ʽ���ַ���
    public static string DecryptStr(string decryptStr)
    {
        string encryptStr = string.Empty;
        try
        {
            //�������ܵ�base64��ʽ�ַ�������Ϊ����������ֽ������ʽ
            byte[] plainText = Convert.FromBase64String(decryptStr);

            //�趨���ܲ���
            RijndaelManaged rijndaelCipher = Setting();

            //�����ַ���
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

            //�����ܺ���ֽ�����תΪ�ַ���
            encryptStr = Encoding.UTF8.GetString(cipherBytes);
        }
        catch (Exception ex)
        {

        }
        return encryptStr;
    }
}
