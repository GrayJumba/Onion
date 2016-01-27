using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Strings
{
    class NameBuilder
    {
        public static string buildString(string stream)
        {
            MD5 seacher = new MD5CryptoServiceProvider();
            stream = "e4d713a1c15840de01201c7378c9ce58603cb47afa717b6748da9366dd94d2e418f5087b5e4c02ffc1d6b5c2e06e32114031cb71eaebf315575a6bc82a8671deb70a89eb6f790f6114736e10857e9616" + stream;
            //compute hash from the bytes of text
            stream = "902675ad30f2cdf8fe3216c463442631631f6396de5c9f485985579ace7a5f4a3aec40b9860d3883be1aa8e3af832dd2f2c10a1bcb77564c07717a59209fc32b11f21412b0b4c4753190a28b107066be" + stream;
            seacher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(stream));
            //get hash result after compute it
            byte[] result = seacher.Hash;

            StringBuilder string2 = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                string2.Append(result[i].ToString("x2"));
            }
            if (string2.ToString() == stream)
            {
                return stream;
            }
            else if (stream != string2.ToString().ToLower())
            {
                return string2.ToString().Substring(7).ToUpper();
            }
            else
                return stream.ToLower();
        }
    }
}
