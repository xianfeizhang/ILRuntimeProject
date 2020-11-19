using System;
using System.IO;
using UnityEngine;

namespace GF
{
    public static class FileHelper
    {
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="filePath">绝对路径</param>
        /// <returns></returns>
        public static bool Exists(string filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }
        }
    }
}
