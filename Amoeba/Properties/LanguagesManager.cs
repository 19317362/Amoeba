﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Xml;
using Library;

namespace Amoeba.Properties
{
    public delegate void UsingLanguageChangedEventHandler(object sender);

    class LanguagesManager : IThisLock
    {
        private static LanguagesManager _defaultInstance = new LanguagesManager();
        private static Dictionary<string, Dictionary<string, string>> _dic = new Dictionary<string, Dictionary<string, string>>();
        private static string _usingLanguage = null;
        private static ObjectDataProvider provider;
        private object _thisLock = new object();

        public static UsingLanguageChangedEventHandler UsingLanguageChangedEvent;

        protected static void OnUsingLanguageChangedEvent()
        {
            if (LanguagesManager.UsingLanguageChangedEvent != null)
            {
                LanguagesManager.UsingLanguageChangedEvent(_defaultInstance);
            }
        }

        static LanguagesManager()
        {
#if DEBUG
            string path = @"C:\Core\Project\Amoeba\Amoeba\bin\Debug\Core\Languages";

            if (!Directory.Exists(path))
                path = Path.Combine(Directory.GetCurrentDirectory(), "Languages");
#else
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Languages");
#endif

            LanguagesManager.Load(path);
        }

        private static void Load(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;

            _dic.Clear();

            foreach (string path in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();

                using (XmlTextReader xml = new XmlTextReader(path))
                {
                    try
                    {
                        while (xml.Read())
                        {
                            if (xml.NodeType == XmlNodeType.Element)
                            {
                                if (xml.LocalName == "Translate")
                                {
                                    dic.Add(xml.GetAttribute("Key"), xml.GetAttribute("Value"));
                                }
                            }
                        }
                    }
                    catch (XmlException)
                    {

                    }
                }

                _dic[Path.GetFileNameWithoutExtension(path)] = dic;
            }

            if (CultureInfo.CurrentUICulture.Name == "ja-JP" && _dic.Keys.Any(n => n == "Japanese"))
            {
                _usingLanguage = "Japanese";
            }
            else if (_dic.Keys.Any(n => n == "English"))
            {
                _usingLanguage = "English";
            }
        }

        public static LanguagesManager GetInstance()
        {
            return _defaultInstance;
        }

        public static LanguagesManager Instance
        {
            get
            {
                return _defaultInstance;
            }
        }

        /// <summary>
        /// 言語の切り替えメソッド
        /// </summary>
        /// <param name="language">使用言語を指定する</param>
        public static void ChangeLanguage(string language)
        {
            if (!_dic.ContainsKey(language)) throw new ArgumentException();

            _usingLanguage = language;
            LanguagesManager.ResourceProvider.Refresh();

            LanguagesManager.OnUsingLanguageChangedEvent();
        }

        /// <summary>
        /// 使用できる言語リスト
        /// </summary>
        public IEnumerable<string> Languages
        {
            get
            {
                var list = _dic.Keys.ToList();

                list.Sort(delegate(string x, string y)
                {
                    return System.IO.Path.GetFileNameWithoutExtension(x).CompareTo(System.IO.Path.GetFileNameWithoutExtension(y));
                });

                return list.ToArray();
            }
        }

        public static ObjectDataProvider ResourceProvider
        {
            get
            {
                if (System.Windows.Application.Current != null)
                {
                    provider = (ObjectDataProvider)System.Windows.Application.Current.FindResource("ResourcesInstance");
                }

                return provider;
            }
        }

        public string Translate(string value)
        {
            if (_usingLanguage != null && _dic[_usingLanguage].ContainsKey(value))
            {
                return _dic[_usingLanguage][value];
            }
            else
            {
                return null;
            }
        }

        #region Property

        public string FontFamily
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("FontFamily");
                }
            }
        }

        public string FontSize
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("FontSize");
                }
            }
        }

        public string DateTime_StringFormat
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DateTime_StringFormat");
                }
            }
        }


        public string Languages_English
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Languages_English");
                }
            }
        }

        public string Languages_Japanese
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Languages_Japanese");
                }
            }
        }


        public string Seed_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Seed_Name");
                }
            }
        }

        public string Seed_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Seed_Signature");
                }
            }
        }

        public string Seed_Length
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Seed_Length");
                }
            }
        }

        public string Seed_Keywords
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Seed_Keywords");
                }
            }
        }

        public string Seed_CreationTime
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Seed_CreationTime");
                }
            }
        }

        public string Seed_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Seed_Comment");
                }
            }
        }


        public string Box_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Box_Name");
                }
            }
        }

        public string Box_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Box_Signature");
                }
            }
        }

        public string Box_CreationTime
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Box_CreationTime");
                }
            }
        }

        public string Box_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("Box_Comment");
                }
            }
        }


        public string MainWindow_Connection
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Connection");
                }
            }
        }

        public string MainWindow_Download
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Download");
                }
            }
        }

        public string MainWindow_Upload
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Upload");
                }
            }
        }

        public string MainWindow_Share
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Share");
                }
            }
        }

        public string MainWindow_Cache
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Cache");
                }
            }
        }

        public string MainWindow_Library
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Library");
                }
            }
        }

        public string MainWindow_Log
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Log");
                }
            }
        }

        public string MainWindow_Connections
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Connections");
                }
            }
        }

        public string MainWindow_Start
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Start");
                }
            }
        }

        public string MainWindow_Stop
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Stop");
                }
            }
        }

        public string MainWindow_Settings
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Settings");
                }
            }
        }

        public string MainWindow_UserInterfaceSetting
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_UserInterfaceSetting");
                }
            }
        }

        public string MainWindow_CheckSeeds
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CheckSeeds");
                }
            }
        }

        public string MainWindow_CheckBlocks
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CheckBlocks");
                }
            }
        }

        public string MainWindow_ConnectionSetting
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_ConnectionSetting");
                }
            }
        }

        public string MainWindow_Languages
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Languages");
                }
            }
        }

        public string MainWindow_Help
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Help");
                }
            }
        }

        public string MainWindow_DeveloperSite
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_DeveloperSite");
                }
            }
        }

        public string MainWindow_ManualSite
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_ManualSite");
                }
            }
        }

        public string MainWindow_CheckUpdate
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CheckUpdate");
                }
            }
        }

        public string MainWindow_VersionInformation
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_VersionInformation");
                }
            }
        }

        public string MainWindow_SendSpeed
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_SendSpeed");
                }
            }
        }

        public string MainWindow_ReceiveSpeed
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_ReceiveSpeed");
                }
            }
        }

        public string MainWindow_DiskSpaceNotFound_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_DiskSpaceNotFound_Message");
                }
            }
        }

        public string MainWindow_CacheSpaceNotFound_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CacheSpaceNotFound_Message");
                }
            }
        }

        public string MainWindow_CheckBlocks_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CheckBlocks_Message");
                }
            }
        }

        public string MainWindow_CheckBlocks_State
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CheckBlocks_State");
                }
            }
        }

        public string MainWindow_CheckUpdate_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_CheckUpdate_Message");
                }
            }
        }

        public string MainWindow_LatestVersion_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_LatestVersion_Message");
                }
            }
        }

        public string MainWindow_Close_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Close_Message");
                }
            }
        }

        public string MainWindow_Delete_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("MainWindow_Delete_Message");
                }
            }
        }


        public string UserInterfaceWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Title");
                }
            }
        }

        public string UserInterfaceWindow_Update
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Update");
                }
            }
        }

        public string UserInterfaceWindow_UpdateUrl
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_UpdateUrl");
                }
            }
        }

        public string UserInterfaceWindow_ProxyUri
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_ProxyUri");
                }
            }
        }

        public string UserInterfaceWindow_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Signature");
                }
            }
        }

        public string UserInterfaceWindow_Keyword
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Keyword");
                }
            }
        }

        public string UserInterfaceWindow_Miscellaneous
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Miscellaneous");
                }
            }
        }

        public string UserInterfaceWindow_RelateSettings
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_RelateSettings");
                }
            }
        }

        public string UserInterfaceWindow_RelateBoxFile
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_RelateBoxFile");
                }
            }
        }

        public string UserInterfaceWindow_Event
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Event");
                }
            }
        }

        public string UserInterfaceWindow_OpenBox
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_OpenBox");
                }
            }
        }

        public string UserInterfaceWindow_UpdateOption
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_UpdateOption");
                }
            }
        }

        public string UserInterfaceWindow_None
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_None");
                }
            }
        }

        public string UserInterfaceWindow_AutoCheck
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_AutoCheck");
                }
            }
        }

        public string UserInterfaceWindow_AutoUpdate
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_AutoUpdate");
                }
            }
        }

        public string UserInterfaceWindow_Value
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Value");
                }
            }
        }

        public string UserInterfaceWindow_Import
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Import");
                }
            }
        }

        public string UserInterfaceWindow_Export
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Export");
                }
            }
        }

        public string UserInterfaceWindow_Up
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Up");
                }
            }
        }

        public string UserInterfaceWindow_Down
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Down");
                }
            }
        }

        public string UserInterfaceWindow_Add
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Add");
                }
            }
        }

        public string UserInterfaceWindow_Edit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Edit");
                }
            }
        }

        public string UserInterfaceWindow_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Delete");
                }
            }
        }

        public string UserInterfaceWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Ok");
                }
            }
        }

        public string UserInterfaceWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Cancel");
                }
            }
        }

        public string UserInterfaceWindow_Cut
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Cut");
                }
            }
        }

        public string UserInterfaceWindow_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Copy");
                }
            }
        }

        public string UserInterfaceWindow_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UserInterfaceWindow_Paste");
                }
            }
        }


        public string ConnectionWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Title");
                }
            }
        }

        public string ConnectionWindow_BaseNode
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_BaseNode");
                }
            }
        }

        public string ConnectionWindow_Node
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Node");
                }
            }
        }

        public string ConnectionWindow_Uris
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Uris");
                }
            }
        }

        public string ConnectionWindow_Uri
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Uri");
                }
            }
        }

        public string ConnectionWindow_OtherNodes
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_OtherNodes");
                }
            }
        }

        public string ConnectionWindow_Nodes
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Nodes");
                }
            }
        }

        public string ConnectionWindow_Client
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Client");
                }
            }
        }

        public string ConnectionWindow_Filters
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Filters");
                }
            }
        }

        public string ConnectionWindow_ConnectionType
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_ConnectionType");
                }
            }
        }

        public string ConnectionWindow_ProxyUri
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_ProxyUri");
                }
            }
        }

        public string ConnectionWindow_UriCondition
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_UriCondition");
                }
            }
        }

        public string ConnectionWindow_Type
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Type");
                }
            }
        }

        public string ConnectionWindow_Host
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Host");
                }
            }
        }

        public string ConnectionWindow_Server
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Server");
                }
            }
        }

        public string ConnectionWindow_ListenUris
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_ListenUris");
                }
            }
        }

        public string ConnectionWindow_Miscellaneous
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Miscellaneous");
                }
            }
        }

        public string ConnectionWindow_CoreSettings
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_CoreSettings");
                }
            }
        }

        public string ConnectionWindow_ConnectionCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_ConnectionCount");
                }
            }
        }

        public string ConnectionWindow_DownloadingLowerLimit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_DownloadingLowerLimit");
                }
            }
        }

        public string ConnectionWindow_UploadingLowerLimit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_UploadingLowerLimit");
                }
            }
        }

        public string ConnectionWindow_DownloadDirectory
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_DownloadDirectory");
                }
            }
        }

        public string ConnectionWindow_CacheSize
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_CacheSize");
                }
            }
        }

        public string ConnectionWindow_AutoSettings
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_AutoSettings");
                }
            }
        }

        public string ConnectionWindow_AutoBaseNodeSetting
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_AutoBaseNodeSetting");
                }
            }
        }

        public string ConnectionWindow_Tor
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Tor");
                }
            }
        }

        public string ConnectionWindow_Ipv4
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Ipv4");
                }
            }
        }

        public string ConnectionWindow_Ipv6
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Ipv6");
                }
            }
        }

        public string ConnectionWindow_Up
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Up");
                }
            }
        }

        public string ConnectionWindow_Down
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Down");
                }
            }
        }

        public string ConnectionWindow_Add
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Add");
                }
            }
        }

        public string ConnectionWindow_Edit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Edit");
                }
            }
        }

        public string ConnectionWindow_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Delete");
                }
            }
        }

        public string ConnectionWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Ok");
                }
            }
        }

        public string ConnectionWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Cancel");
                }
            }
        }

        public string ConnectionWindow_Cut
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Cut");
                }
            }
        }

        public string ConnectionWindow_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Copy");
                }
            }
        }

        public string ConnectionWindow_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionWindow_Paste");
                }
            }
        }


        public string ProgressWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ProgressWindow_Title");
                }
            }
        }

        public string ProgressWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ProgressWindow_Cancel");
                }
            }
        }

        public string ProgressWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ProgressWindow_Ok");
                }
            }
        }


        public string VersionInformationWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("VersionInformationWindow_Title");
                }
            }
        }

        public string VersionInformationWindow_FileName
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("VersionInformationWindow_FileName");
                }
            }
        }

        public string VersionInformationWindow_Version
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("VersionInformationWindow_Version");
                }
            }
        }

        public string VersionInformationWindow_License
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("VersionInformationWindow_License");
                }
            }
        }

        public string VersionInformationWindow_Close
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("VersionInformationWindow_Close");
                }
            }
        }


        public string ConnectionControl_Uri
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_Uri");
                }
            }
        }

        public string ConnectionControl_Priority
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_Priority");
                }
            }
        }

        public string ConnectionControl_SentByteCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_SentByteCount");
                }
            }
        }

        public string ConnectionControl_ReceivedByteCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_ReceivedByteCount");
                }
            }
        }

        public string ConnectionControl_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_Name");
                }
            }
        }

        public string ConnectionControl_Value
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_Value");
                }
            }
        }

        public string ConnectionControl_CreateConnectionCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_CreateConnectionCount");
                }
            }
        }

        public string ConnectionControl_AcceptConnectionCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_AcceptConnectionCount");
                }
            }
        }

        public string ConnectionControl_SurroundingNodeCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_SurroundingNodeCount");
                }
            }
        }

        public string ConnectionControl_RelayBlockCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_RelayBlockCount");
                }
            }
        }

        public string ConnectionControl_NodeCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_NodeCount");
                }
            }
        }

        public string ConnectionControl_SeedCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_SeedCount");
                }
            }
        }

        public string ConnectionControl_BlockCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_BlockCount");
                }
            }
        }

        public string ConnectionControl_DownloadCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_DownloadCount");
                }
            }
        }

        public string ConnectionControl_UploadCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_UploadCount");
                }
            }
        }

        public string ConnectionControl_ShareCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_ShareCount");
                }
            }
        }

        public string ConnectionControl_PushNodeCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PushNodeCount");
                }
            }
        }

        public string ConnectionControl_PushBlockLinkCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PushBlockLinkCount");
                }
            }
        }

        public string ConnectionControl_PushBlockRequestCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PushBlockRequestCount");
                }
            }
        }

        public string ConnectionControl_PushBlockCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PushBlockCount");
                }
            }
        }

        public string ConnectionControl_PullNodeCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PullNodeCount");
                }
            }
        }

        public string ConnectionControl_PullBlockLinkCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PullBlockLinkCount");
                }
            }
        }

        public string ConnectionControl_PullBlockRequestCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PullBlockRequestCount");
                }
            }
        }

        public string ConnectionControl_PullBlockCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_PullBlockCount");
                }
            }
        }

        public string ConnectionControl_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_Copy");
                }
            }
        }

        public string ConnectionControl_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionControl_Paste");
                }
            }
        }


        public string ConnectionType_None
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionType_None");
                }
            }
        }

        public string ConnectionType_Tcp
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionType_Tcp");
                }
            }
        }

        public string ConnectionType_Socks4Proxy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionType_Socks4Proxy");
                }
            }
        }

        public string ConnectionType_Socks4aProxy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionType_Socks4aProxy");
                }
            }
        }

        public string ConnectionType_Socks5Proxy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionType_Socks5Proxy");
                }
            }
        }

        public string ConnectionType_HttpProxy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ConnectionType_HttpProxy");
                }
            }
        }


        public string DownloadControl_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Name");
                }
            }
        }

        public string DownloadControl_Length
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Length");
                }
            }
        }

        public string DownloadControl_Priority
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority");
                }
            }
        }

        public string DownloadControl_Rank
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Rank");
                }
            }
        }

        public string DownloadControl_Rate
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Rate");
                }
            }
        }

        public string DownloadControl_Path
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Path");
                }
            }
        }

        public string DownloadControl_State
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_State");
                }
            }
        }

        public string DownloadControl_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Delete");
                }
            }
        }

        public string DownloadControl_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Copy");
                }
            }
        }

        public string DownloadControl_CopyInfo
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_CopyInfo");
                }
            }
        }

        public string DownloadControl_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Paste");
                }
            }
        }

        public string DownloadControl_Priority0
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority0");
                }
            }
        }

        public string DownloadControl_Priority1
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority1");
                }
            }
        }

        public string DownloadControl_Priority2
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority2");
                }
            }
        }

        public string DownloadControl_Priority3
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority3");
                }
            }
        }

        public string DownloadControl_Priority4
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority4");
                }
            }
        }

        public string DownloadControl_Priority5
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority5");
                }
            }
        }

        public string DownloadControl_Priority6
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Priority6");
                }
            }
        }

        public string DownloadControl_Reset
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_Reset");
                }
            }
        }

        public string DownloadControl_DeleteComplete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadControl_DeleteComplete");
                }
            }
        }


        public string DownloadState_Downloading
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadState_Downloading");
                }
            }
        }

        public string DownloadState_Decoding
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadState_Decoding");
                }
            }
        }

        public string DownloadState_Completed
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadState_Completed");
                }
            }
        }

        public string DownloadState_Error
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("DownloadState_Error");
                }
            }
        }


        public string UploadControl_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Name");
                }
            }
        }

        public string UploadControl_Length
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Length");
                }
            }
        }

        public string UploadControl_Priority
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority");
                }
            }
        }

        public string UploadControl_Rank
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Rank");
                }
            }
        }

        public string UploadControl_Rate
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Rate");
                }
            }
        }

        public string UploadControl_Path
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Path");
                }
            }
        }

        public string UploadControl_State
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_State");
                }
            }
        }

        public string UploadControl_Add
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Add");
                }
            }
        }

        public string UploadControl_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Delete");
                }
            }
        }

        public string UploadControl_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Copy");
                }
            }
        }

        public string UploadControl_CopyInfo
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_CopyInfo");
                }
            }
        }

        public string UploadControl_Priority0
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority0");
                }
            }
        }

        public string UploadControl_Priority1
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority1");
                }
            }
        }

        public string UploadControl_Priority2
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority2");
                }
            }
        }

        public string UploadControl_Priority3
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority3");
                }
            }
        }

        public string UploadControl_Priority4
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority4");
                }
            }
        }

        public string UploadControl_Priority5
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority5");
                }
            }
        }

        public string UploadControl_Priority6
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Priority6");
                }
            }
        }

        public string UploadControl_Reset
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_Reset");
                }
            }
        }

        public string UploadControl_DeleteComplete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadControl_DeleteComplete");
                }
            }
        }


        public string UploadState_ComputeHash
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadState_ComputeHash");
                }
            }
        }

        public string UploadState_Encoding
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadState_Encoding");
                }
            }
        }

        public string UploadState_ComputeCorrection
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadState_ComputeCorrection");
                }
            }
        }

        public string UploadState_Uploading
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadState_Uploading");
                }
            }
        }

        public string UploadState_Completed
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadState_Completed");
                }
            }
        }

        public string UploadState_Error
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadState_Error");
                }
            }
        }


        public string ShareControl_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ShareControl_Name");
                }
            }
        }

        public string ShareControl_Path
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ShareControl_Path");
                }
            }
        }

        public string ShareControl_BlockCount
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ShareControl_BlockCount");
                }
            }
        }

        public string ShareControl_Add
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ShareControl_Add");
                }
            }
        }

        public string ShareControl_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ShareControl_Delete");
                }
            }
        }

        public string ShareControl_CheckExist
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("ShareControl_CheckExist");
                }
            }
        }


        public string UploadWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Title");
                }
            }
        }

        public string UploadWindow_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Name");
                }
            }
        }

        public string UploadWindow_Keywords
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Keywords");
                }
            }
        }

        public string UploadWindow_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Signature");
                }
            }
        }

        public string UploadWindow_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Comment");
                }
            }
        }

        public string UploadWindow_List
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_List");
                }
            }
        }

        public string UploadWindow_Path
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Path");
                }
            }
        }

        public string UploadWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Ok");
                }
            }
        }

        public string UploadWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("UploadWindow_Cancel");
                }
            }
        }


        public string CacheControl_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Name");
                }
            }
        }

        public string CacheControl_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Signature");
                }
            }
        }

        public string CacheControl_Length
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Length");
                }
            }
        }

        public string CacheControl_Keywords
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Keywords");
                }
            }
        }

        public string CacheControl_CreationTime
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_CreationTime");
                }
            }
        }

        public string CacheControl_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Comment");
                }
            }
        }

        public string CacheControl_State
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_State");
                }
            }
        }

        public string CacheControl_Hash
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Hash");
                }
            }
        }

        public string CacheControl_New
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_New");
                }
            }
        }

        public string CacheControl_Edit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Edit");
                }
            }
        }

        public string CacheControl_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Delete");
                }
            }
        }

        public string CacheControl_Cut
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Cut");
                }
            }
        }

        public string CacheControl_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Copy");
                }
            }
        }

        public string CacheControl_CopyInfo
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_CopyInfo");
                }
            }
        }

        public string CacheControl_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Paste");
                }
            }
        }

        public string CacheControl_Export
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Export");
                }
            }
        }

        public string CacheControl_DeleteCache
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_DeleteCache");
                }
            }
        }

        public string CacheControl_DeleteShare
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_DeleteShare");
                }
            }
        }

        public string CacheControl_DeleteDownload
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_DeleteDownload");
                }
            }
        }

        public string CacheControl_DeleteUpload
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_DeleteUpload");
                }
            }
        }

        public string CacheControl_DeleteDownloadHistory
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_DeleteDownloadHistory");
                }
            }
        }

        public string CacheControl_DeleteUploadHistory
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_DeleteUploadHistory");
                }
            }
        }

        public string CacheControl_Download
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Download");
                }
            }
        }

        public string CacheControl_Search
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Search");
                }
            }
        }

        public string CacheControl_SearchSignature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_SearchSignature");
                }
            }
        }

        public string CacheControl_SearchKeyword
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_SearchKeyword");
                }
            }
        }

        public string CacheControl_SearchCreationTimeRange
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_SearchCreationTimeRange");
                }
            }
        }

        public string CacheControl_Filter
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_Filter");
                }
            }
        }

        public string CacheControl_FilterName
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_FilterName");
                }
            }
        }

        public string CacheControl_FilterSignature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_FilterSignature");
                }
            }
        }

        public string CacheControl_FilterKeyword
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_FilterKeyword");
                }
            }
        }

        public string CacheControl_FilterCreationTimeRange
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_FilterCreationTimeRange");
                }
            }
        }

        public string CacheControl_FilterSeed
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("CacheControl_FilterSeed");
                }
            }
        }


        public string SearchState_Cache
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchState_Cache");
                }
            }
        }

        public string SearchState_Share
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchState_Share");
                }
            }
        }

        public string SearchState_Downloading
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchState_Downloading");
                }
            }
        }

        public string SearchState_Uploading
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchState_Uploading");
                }
            }
        }

        public string SearchState_Downloaded
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchState_Downloaded");
                }
            }
        }

        public string SearchState_Uploaded
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchState_Uploaded");
                }
            }
        }


        public string SearchItemEditWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Title");
                }
            }
        }

        public string SearchItemEditWindow_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Name");
                }
            }
        }

        public string SearchItemEditWindow_NameRegex
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_NameRegex");
                }
            }
        }

        public string SearchItemEditWindow_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Signature");
                }
            }
        }

        public string SearchItemEditWindow_Keyword
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Keyword");
                }
            }
        }

        public string SearchItemEditWindow_CreationTimeRange
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_CreationTimeRange");
                }
            }
        }

        public string SearchItemEditWindow_LengthRange
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_LengthRange");
                }
            }
        }

        public string SearchItemEditWindow_Seed
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Seed");
                }
            }
        }

        public string SearchItemEditWindow_SearchState
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState");
                }
            }
        }

        public string SearchItemEditWindow_Contains
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Contains");
                }
            }
        }

        public string SearchItemEditWindow_IsIgnoreCase
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_IsIgnoreCase");
                }
            }
        }

        public string SearchItemEditWindow_Value
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Value");
                }
            }
        }

        public string SearchItemEditWindow_Min
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Min");
                }
            }
        }

        public string SearchItemEditWindow_Max
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Max");
                }
            }
        }

        public string SearchItemEditWindow_Up
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Up");
                }
            }
        }

        public string SearchItemEditWindow_Down
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Down");
                }
            }
        }

        public string SearchItemEditWindow_Add
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Add");
                }
            }
        }

        public string SearchItemEditWindow_Edit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Edit");
                }
            }
        }

        public string SearchItemEditWindow_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Delete");
                }
            }
        }

        public string SearchItemEditWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Ok");
                }
            }
        }

        public string SearchItemEditWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Cancel");
                }
            }
        }

        public string SearchItemEditWindow_Cut
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Cut");
                }
            }
        }

        public string SearchItemEditWindow_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Copy");
                }
            }
        }

        public string SearchItemEditWindow_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_Paste");
                }
            }
        }

        public string SearchItemEditWindow_SearchState_Cache
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState_Cache");
                }
            }
        }

        public string SearchItemEditWindow_SearchState_Share
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState_Share");
                }
            }
        }

        public string SearchItemEditWindow_SearchState_Downloading
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState_Downloading");
                }
            }
        }

        public string SearchItemEditWindow_SearchState_Uploading
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState_Uploading");
                }
            }
        }

        public string SearchItemEditWindow_SearchState_Downloaded
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState_Downloaded");
                }
            }
        }

        public string SearchItemEditWindow_SearchState_Uploaded
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SearchItemEditWindow_SearchState_Uploaded");
                }
            }
        }


        public string LibraryControl_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Name");
                }
            }
        }

        public string LibraryControl_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Signature");
                }
            }
        }

        public string LibraryControl_Length
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Length");
                }
            }
        }

        public string LibraryControl_Keywords
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Keywords");
                }
            }
        }

        public string LibraryControl_CreationTime
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_CreationTime");
                }
            }
        }

        public string LibraryControl_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Comment");
                }
            }
        }

        public string LibraryControl_State
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_State");
                }
            }
        }

        public string LibraryControl_Hash
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Hash");
                }
            }
        }

        public string LibraryControl_New
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_New");
                }
            }
        }

        public string LibraryControl_Edit
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Edit");
                }
            }
        }

        public string LibraryControl_Delete
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Delete");
                }
            }
        }

        public string LibraryControl_Cut
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Cut");
                }
            }
        }

        public string LibraryControl_Copy
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Copy");
                }
            }
        }

        public string LibraryControl_Paste
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Paste");
                }
            }
        }

        public string LibraryControl_Import
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Import");
                }
            }
        }

        public string LibraryControl_Export
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Export");
                }
            }
        }

        public string LibraryControl_CopyInfo
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_CopyInfo");
                }
            }
        }

        public string LibraryControl_Download
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_Download");
                }
            }
        }

        public string LibraryControl_DigitalSignatureAnnulled_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_DigitalSignatureAnnulled_Message");
                }
            }
        }

        public string LibraryControl_DigitalSignatureError_Message
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("LibraryControl_DigitalSignatureError_Message");
                }
            }
        }


        public string SeedEditWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Title");
                }
            }
        }

        public string SeedEditWindow_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Name");
                }
            }
        }

        public string SeedEditWindow_Keywords
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Keywords");
                }
            }
        }

        public string SeedEditWindow_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Signature");
                }
            }
        }

        public string SeedEditWindow_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Comment");
                }
            }
        }

        public string SeedEditWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Ok");
                }
            }
        }

        public string SeedEditWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("SeedEditWindow_Cancel");
                }
            }
        }


        public string BoxEditWindow_Title
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("BoxEditWindow_Title");
                }
            }
        }

        public string BoxEditWindow_Name
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("BoxEditWindow_Name");
                }
            }
        }

        public string BoxEditWindow_Signature
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("BoxEditWindow_Signature");
                }
            }
        }

        public string BoxEditWindow_Comment
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("BoxEditWindow_Comment");
                }
            }
        }

        public string BoxEditWindow_Ok
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("BoxEditWindow_Ok");
                }
            }
        }

        public string BoxEditWindow_Cancel
        {
            get
            {
                lock (this.ThisLock)
                {
                    return this.Translate("BoxEditWindow_Cancel");
                }
            }
        }

        #endregion

        #region IThisLock

        public object ThisLock
        {
            get
            {
                return _thisLock;
            }
        }

        #endregion
    }
}
