using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace H1Z1_Config_Manager
{
    public enum UserOptionsGroup : short
    {
        Default = 0,
        Display = 1,
        Rendering = 2,
        General = 3,
        Sound = 4,
        Voice = 5,
        Controls = 6,
        VideoStreamer = 7,
        AutoRefuse = 8,
        UI = 9,
        VoiceChat = 10,
        VoiceVolumes = 11
    };
    
    public class UserOptionsSetting
    {
        public string group, key, value;

        public UserOptionsSetting()
        {
            group = key = value = String.Empty;
        }

        public UserOptionsSetting(string group, string key, string value)
        {
            this.group = group;
            this.key = key;
            this.value = value;
        }

        public bool IsMatching(string group, string key)
        {
            if (string.Compare(this.group, group) == 0)
                if (string.Compare(this.key, key) == 0)
                    return true;

            return false;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(key);
        }
    }

    public class UserOptionsParser
    {
        private List<UserOptionsSetting> m_settings;
        public bool ReadOnly;
        public string GamePath;

        public UserOptionsParser(string path)
        {
            initialize();
            GamePath = path;
        }

        private void initialize()
        {
            m_settings = new List<UserOptionsSetting>();
            ReadOnly = false;
            GamePath = String.Empty;
        }

        public static short GroupToNumber(string group)
        {
            UserOptionsGroup g = UserOptionsGroup.Default;

            if (Enum.TryParse(group, out g))
                return (short)g;

            return (short)UserOptionsGroup.Default;
        }

        public bool ReadConfig()
        {
            if (string.IsNullOrWhiteSpace(GamePath))
                return false;

            FileInfo fileInfo = null;
            string[] lines = null;

            string filePath = GamePath + "\\" + Settings.UserOptionsFileName;

            try
            {
                fileInfo = new FileInfo(filePath);
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "Cannot create FileInfo of UserOptions file!");
                return false;
            }

            if (!fileInfo.Exists)
            {
                Logger.WriteLine(LogType.ERROR, "UserOptions file doesn't exists!");
                return false;
            }

            try
            {
                ReadOnly = fileInfo.IsReadOnly;
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "Cannot check IsReadOnly UserOptions file!");
                return false;
            }

            fileInfo = null;

            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "Cannot read from UserOptions file!");
                return false;
            }

            if (lines == null)
            {
                Logger.WriteLine(LogType.ERROR, "Lines from UserOptions file are null!");
                return false;
            }

            string group, key, value, comment;

            group = key = value = comment = String.Empty;
            
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                if (CheckIfLineIsKeyAndValue(line, out key, out value))
                {
                    Logger.WriteLine(LogType.DEBUG, "KEY: {0} ; VALUE: {1}", key, value);

                    if (string.IsNullOrEmpty(group))
                    {
                        Logger.WriteLine(LogType.WARNING, "KEY and VALUE outside of GROUP: {0}", line);
                        continue;
                    }

                    m_settings.Add(new UserOptionsSetting(group, key, value));
                    continue;
                }

                if (CheckIfLineIsGroupName(line, out group))
                {
                    Logger.WriteLine(LogType.DEBUG, "GROUP: {0}", group);
                    continue;
                }

                if (CheckIfLineIsComment(line, out comment))
                {
                    Logger.WriteLine(LogType.DEBUG, "Comment line ({0}): {1}", (i + 1).ToString(), comment);
                    continue;
                }

                Logger.WriteLine(LogType.WARNING, "Invalid line ({0}): {1}", (i + 1).ToString(), line);
            }

            Logger.WriteLine(LogType.INFO, "UserOptions file successfully loaded.");
            return true;
        }

        public bool SaveConfig()
        {
            if (string.IsNullOrWhiteSpace(GamePath))
                return false;

            string filePath = GamePath + "\\" + Settings.UserOptionsFileName;
            string filePathBackup = GamePath + "\\" + Settings.UserOptionsBackupFileName;

            if (File.Exists(filePathBackup))
            {
                try
                {
                    File.SetAttributes(filePathBackup, FileAttributes.Normal);
                    File.Delete(filePathBackup);
                }
                catch
                {
                    Logger.WriteLine(LogType.ERROR, "Cannot delete UserOptions backup file!");
                    return false;
                }
            }
            else
                Logger.WriteLine(LogType.WARNING, "UserOptions backup file doesn't exist!");

            if (File.Exists(filePath))
            {
                try
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Move(filePath, filePathBackup);
                }
                catch
                {
                    Logger.WriteLine(LogType.ERROR, "Cannot move UserOptions file!");
                    return false;
                }
            }
            else
                Logger.WriteLine(LogType.WARNING, "UserOptions file doesn't exist!");

            StreamWriter file = null;

            try
            {
                file = new StreamWriter(filePath);
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "Cannot create new UserOptions file!");
                return false;
            }

            string group = String.Empty;

            List<UserOptionsSetting> sortedList = m_settings
                .OrderBy(o => GroupToNumber(o.group))
                .ToList();

            foreach (UserOptionsSetting setting in sortedList)
            {
                if (!setting.group.Equals(group))
                {
                    if (!string.IsNullOrEmpty(group))
                    {
                        try
                        {
                            file.Write("\n");
                        }
                        catch
                        {
                            Logger.WriteLine(LogType.ERROR, "Cannot write to new UserOptions file!");
                            break;
                        }
                    }

                    group = setting.group;

                    try
                    {
                        file.Write("[{0}]\n", group);
                    }
                    catch
                    {
                        Logger.WriteLine(LogType.ERROR, "Cannot write to new UserOptions file!");
                        break;
                    }
                }

                try
                {
                    file.Write("{0}={1}\n", setting.key, setting.value);
                }
                catch
                {
                    Logger.WriteLine(LogType.ERROR, "Cannot write to new UserOptions file!");
                    break;
                }
            }

            try
            {
                file.Close();
                FileInfo fileInfo = new FileInfo(filePath);
                fileInfo.IsReadOnly = ReadOnly;
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "Error at closing new UserOptions file!");
                return false;
            }

            Logger.WriteLine(LogType.INFO, "UserOptions file successfully saved.");
            return true;
        }

        public bool SetReadOnlyFlag(bool readOnlyOrNot)
        {
            if (string.IsNullOrWhiteSpace(GamePath))
                return false;

            string filePath = GamePath + "\\" + Settings.UserOptionsFileName;

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.IsReadOnly = readOnlyOrNot;
                    ReadOnly = readOnlyOrNot;
                }
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "Error at setting UserOptions ReadOnly flag!");
                return false;
            }


            Logger.WriteLine(LogType.INFO, "UserOptions ReadOnly flag successfully set.");
            return true;
        }

        public static bool CheckIfLineIsComment(string line, out string comment)
        {
            Match m = Regex.Match(line, @"^(\s*)(\;)(\s*)(?<comment>\S*(\s+\S+)*)(\s*)$");

            if (m.Success)
            {
                comment = m.Groups["comment"].Value;
                return true;
            }

            comment = String.Empty;
            return false;
        }

        public static bool CheckIfLineIsGroupName(string line, out string group)
        {
            Match m = Regex.Match(line, @"^(\s*)(\[+)(?<group>.+)(\]+)(\s*)$");

            if (m.Success)
            {
                group = m.Groups["group"].Value;
                return true;
            }

            group = String.Empty;
            return false;
        }

        public static bool CheckIfLineIsKeyAndValue(string line, out string key, out string value)
        {
            Match m = Regex.Match(line, @"^(\s*)(?<key>.+)(\s*)(\=)(\s*)(?<value>.*)(\s*)$");

            if (m.Success)
            {
                key = m.Groups["key"].Value;
                value = m.Groups["value"].Value;
                return true;
            }

            key = value = String.Empty;
            return false;
        }

        public UserOptionsSetting FindSetting(string group, string key)
        {
            foreach (UserOptionsSetting setting in m_settings)
                if (setting.IsMatching(group, key))
                    return setting;

            return null;
        }

        public string GetStringValue(string group, string key, string defaultValue = "")
        {
            UserOptionsSetting setting = FindSetting(group, key);

            if (setting != null)
                return setting.value;

            return defaultValue;
        }
        
        public void SetStringValue(string group, string key, string value)
        {
            UserOptionsSetting setting = FindSetting(group, key);

            if (setting == null)
            {
                setting = new UserOptionsSetting(group, key, value);
                m_settings.Add(setting);

                Logger.WriteLine(LogType.INFO, "New setting added: GROUP: {0} ; KEY: {1} ; VALUE: {2}", group, key, value);
            }
            else
            {
                setting.value = value;
            }
        }

        public short GetShortValue(string group, string key, short defaultValue = 0)
        {
            return Tools.StrToShort(GetStringValue(group, key), defaultValue);
        }
        
        public void SetShortValue(string group, string key, short value)
        {
            SetStringValue(group, key, value.ToString());
        }

        public int GetIntValue(string group, string key, int defaultValue = 0)
        {
            return Tools.StrToInt(GetStringValue(group, key), defaultValue);
        }

        public void SetIntValue(string group, string key, int value)
        {
            SetStringValue(group, key, value.ToString());
        }

        public double GetDoubleValue(string group, string key, double defaultValue = 0.0)
        {
            return Tools.StrToDouble(GetStringValue(group, key), defaultValue);
        }

        public void SetDoubleValue(string group, string key, double value)
        {
            SetStringValue(group, key, value.ToString());
        }
    }
}
