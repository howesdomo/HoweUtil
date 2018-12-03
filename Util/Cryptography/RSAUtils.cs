using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Cryptography
{
    /// <summary>
    /// V 1.0.0.1 - 2018-12-3 16:00:58
    /// 加密内容过长, 提示 "不正确的长度" 报错, 计划使用 分段 的方式进行 加密/解密
    /// 
    /// V 1.0.0.0 - 2018-12-3 16:00:58
    /// RSAUtils 创建
    /// </summary>
    public class RSAUtils
    {
        private static string sPublicKey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private static string sPrivatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent><P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P><Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q><DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP><DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ><InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ><D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D></RSAKeyValue>";

        public static void Set_sPublicKey(string key)
        {
            sPublicKey = key;
        }

        public static void Set_sPrivatekey(string key)
        {
            sPrivatekey = key;
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string content, string publickey = "")
        {
            if (publickey.IsNullOrEmpty())
            {
                publickey = sPublicKey;
            }

            byte[] byteArr_r = null; // 加密结果

            System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);

            int keySize = rsa.KeySize / 8;
            int bufferSize = keySize - 11;
            byte[] buffer = new byte[bufferSize];

            byte[] byteArr_Content = Encoding.UTF8.GetBytes(content);
            using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(byteArr_Content))
            {
                using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
                {
                    int readLen = msInput.Read(buffer, 0, bufferSize);
                    while (readLen > 0)
                    {
                        byte[] dataToEncrypt = new byte[readLen];
                        Array.Copy
                        (
                            sourceArray: buffer,
                            sourceIndex: 0,
                            destinationArray: dataToEncrypt,
                            destinationIndex: 0,
                            length: readLen
                        );

                        byte[] encrypted = rsa.Encrypt(rgb: dataToEncrypt, fOAEP: false);
                        msOutput.Write(encrypted, 0, encrypted.Length);
                        readLen = msInput.Read(buffer, 0, bufferSize);
                    }

                    byteArr_r = msOutput.ToArray(); // 获得全部加密结果
                    rsa.Clear();
                }
            }

            //string r = Convert.ToBase64String(byteArr_r);
            //return r;
            return byteArr_r;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] Decrypt(string content, string privatekey = "")
        {
            if (privatekey.IsNullOrEmpty())
            {
                privatekey = sPrivatekey;
            }

            byte[] byteArr_r = null; // 解密结果

            System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(privatekey);

            int keySize = rsa.KeySize / 8;
            byte[] buffer = new byte[keySize];

            byte[] byteArr_Content = Encoding.UTF8.GetBytes(content);
            using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(byteArr_Content))
            {
                using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
                {
                    int readLen = msInput.Read(buffer, 0, keySize);
                    while (readLen > 0)
                    {
                        byte[] dataToDecrypt = new byte[readLen];

                        Array.Copy
                        (
                            sourceArray: buffer,
                            sourceIndex: 0,
                            destinationArray: dataToDecrypt,
                            destinationIndex: 0,
                            length: readLen
                        );

                        byte[] decrypted = rsa.Decrypt(rgb: dataToDecrypt, fOAEP: false);
                        msOutput.Write(decrypted, 0, decrypted.Length);
                        readLen = msInput.Read(buffer, 0, keySize);
                    }

                    byteArr_r = msOutput.ToArray(); // 获得全部加密结果
                    rsa.Clear();
                }
            }

            // string r = Convert.ToBase64String(byteArr_r);
            return byteArr_r;
        }
    }
}
