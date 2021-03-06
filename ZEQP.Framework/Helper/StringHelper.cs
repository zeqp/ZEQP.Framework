﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZEQP.Framework
{
    public static partial class StringHelper
    {
        public static string GetRandomStr(int length)
        {
            char[] arrChar = new char[]{
               'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
               '0','1','2','3','4','5','6','7','8','9',
               'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
            };

            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        public static int ToInt(this string source)
        {
            return int.Parse(source);
        }
        public static DateTime ToDateTime(this string source)
        {
            return DateTime.Parse(source);
        }
    }
}
