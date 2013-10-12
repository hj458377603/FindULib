using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FindULib.Common
{
    public class AppConfig
    {
        /// <summary>
        /// 当前应用的版本号
        /// </summary>
        public const string VERSION = "0.13.10.02";

        /// <summary>
        /// 当前应用的版本文件url
        /// </summary>
        public const string VERSION_CONFIG_URL = "http://files.cnblogs.com/fb-boy/config.xml";

        /// <summary>
        /// 独立存储目录名称
        /// </summary>
        public const string STORE_DIRECTORY_NAME = "FindULib";

        /// <summary>
        /// 独立存储图片目录名称
        /// </summary>
        public const string STORE_IMAGE_DIRECTORY_NAME = STORE_DIRECTORY_NAME + "\\Images";

        /// <summary>
        /// 独立存储收藏夹文件名称
        /// </summary>
        public const string STORE_FILE_NAME = "Store.xml";
    }
}
