﻿/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/04/29
 * Time: 20:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;

namespace namaichi.config {
/// <summary>
/// Description of config.
/// </summary>
public class config
{
	private Configuration cfg;
	public Dictionary<string, string> defaultConfig;
	public Dictionary<string, string> argConfig = new Dictionary<string, string>();
	public config()
	{
		cfg = getConfig();
		defaultMergeFile();
        
 	}
	public Configuration getConfig() {
		while (true) {
			try {
				var jarPath = util.getJarPath();
				var configFile = jarPath[0] + "\\" + jarPath[1] + ".config";
				//util.debugWriteLine(configFile);
		        var exeFileMap = new System.Configuration. ExeConfigurationFileMap { ExeConfigFilename = configFile };
		        var cfg     = ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);
		        return cfg;
			} catch (Exception e) {
				util.debugWriteLine("getconfig " + e.Message + " " + e.StackTrace + " " + e.TargetSite);
				Thread.Sleep(3000);
				continue;
			}
			
		}
	}
	public void set(string key, string value) {
		if (key.IndexOf("user_session") == -1 && 
				key.IndexOf("account") == -1)
			util.debugWriteLine("config set " + key + " " + value);
		else util.debugWriteLine("config set " + key);
		for (var i = 0; i < 100; i++) {
			cfg = getConfig();
			
			
			var keys = cfg.AppSettings.Settings.AllKeys;
			if (System.Array.IndexOf(keys, key) < 0)
				cfg.AppSettings.Settings.Add(key, value);
			else cfg.AppSettings.Settings[key].Value = value;
			try {
				cfg.Save();
				return;
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace);
				System.Threading.Thread.Sleep(500);
				continue;
			}
		}
	}
	public void set(List<KeyValuePair<string, string>> l) {
		foreach (var _l in l) {
			if (_l.Key.IndexOf("user_session") == -1 && 
					_l.Key.IndexOf("account") == -1)
				util.debugWriteLine("config set " + _l.Key + " " + _l.Value);
			else util.debugWriteLine("config set " + _l.Key);
		}
		for (var i = 0; i < 100; i++) {
			cfg = getConfig();
			
			var keys = cfg.AppSettings.Settings.AllKeys;
			
			foreach (var _l in l) {
				if (System.Array.IndexOf(keys, _l.Key) < 0)
					cfg.AppSettings.Settings.Add(_l.Key, _l.Value);
				else cfg.AppSettings.Settings[_l.Key].Value = _l.Value;
			}
			try {
				cfg.Save();
				return;
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace);
				System.Threading.Thread.Sleep(500);
				continue;
			}
		}
	}
	public string get(string key) {
		util.debugWriteLine("config get " + key);
		try {
			if (key.IndexOf("user_session") == -1 && 
			    	key.IndexOf("account") == -1) {
				util.debugWriteLine(key + " " + cfg.AppSettings.Settings[key].Value);
			} else util.debugWriteLine(key);
		} catch (Exception e) {
			util.debugWriteLine("config get exception " + key + " " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			return null;
		}
		try {
			if (argConfig.ContainsKey(key)) 
				return argConfig[key];
			return cfg.AppSettings.Settings[key].Value;
		} catch (Exception e) {
			return null;
		}
	}
	private void defaultMergeFile() {
		defaultConfig = new Dictionary<string, string>(){
			{"accountId",""},
			{"accountPass",""},
			{"user_session",""},
			{"user_session_secure",""},
			{"browserNum","1"},
			{"browserPath",""},
			{"issecondlogin","false"},
			
			{"IsdefaultBrowserPath","true"},
			{"appliAPath",""},
			{"appliBPath",""},
			{"appliCPath",""},
			{"appliDPath",""},
			{"appliEPath",""},
			{"appliFPath",""},
			{"appliGPath",""},
			{"appliHPath",""},
			{"appliIPath",""},
			{"appliJPath",""},
			
			{"appliAArgs",""},
			{"appliBArgs",""},
			{"appliCArgs",""},
			{"appliDArgs",""},
			{"appliEArgs",""},
			{"appliFArgs",""},
			{"appliGArgs",""},
			{"appliHArgs",""},
			{"appliIArgs",""},
			{"appliJArgs",""},
			
			{"appliAName",""},
			{"appliBName",""},
			{"appliCName",""},
			{"appliDName",""},
			{"appliEName",""},
			{"appliFName",""},
			{"appliGName",""},
			{"appliHName",""},
			{"appliIName",""},
			{"appliJName",""},
			{"log","false"},
			{"IsStartTimeAllCheck","false"},
			{"Ischeck30min","true"},
			{"IschangeIcon","false"},
			{"IstasktrayStart","false"},
			{"IsdragCom","false"},
			{"doublecmode","なにもしない"},
			
			
			{"IsbroadLog","false"},
			{"IsLogFile","false"},
			
			{"poploc","右下"},
			{"poptime","10"},
			{"Isclosepopup","true"},
			{"Isfixpopup","false"},
			{"Issmallpopup","false"},
			{"IsTopMostPopup","true"},
			{"rssUpdateInterval","15"},
			{"userNameUpdateInterval","15"},
			
			{"mailFrom",""},
			{"mailTo",""},
			{"mailSmtp",""},
			{"mailPort",""},
			{"mailUser",""},
			{"mailPass",""},
			{"IsmailSsl","false"},
			{"IsSoundDefault","true"},
			{"soundPath",""},
			{"soundVolume","50"},
			
			{"cookieFile",""},
			{"iscookie","false"},
			
			{"ShowComId","true"},
			{"ShowUserId","true"},
			{"ShowComName","true"},
			{"ShowUserName","true"},
			{"ShowKeyword","true"},
			{"ShowComFollow","true"},
			{"ShowUserFollow","true"},
			{"ShowLatestTime","true"},
			{"ShowRegistTime","true"},
			{"ShowPop","true"},
			{"ShowBalloon","true"},
			{"ShowWeb","true"},
			{"ShowMail","true"},
			{"ShowSound","true"},
			{"ShowAppA","true"},
			{"ShowAppB","true"},
			{"ShowAppC","true"},
			{"ShowAppD","false"},
			{"ShowAppE","false"},
			{"ShowAppF","false"},
			{"ShowAppG","false"},
			{"ShowAppH","false"},
			{"ShowAppI","false"},
			{"ShowAppJ","false"},
			{"ShowMemo","false"},
				
			{"ShowTaskStartDt","true"},
			{"ShowTaskLvid","true"},
			{"ShowTaskArgs","true"},
			{"ShowTaskAddDt","true"},
			{"ShowTaskStatus","true"},
			{"ShowTaskPopup","true"},
			{"ShowTaskBalloon","true"},
			{"ShowTaskWeb","true"},
			{"ShowTaskMail","true"},
			{"ShowTaskSound","true"},
			{"ShowTaskAppliA","true"},
			{"ShowTaskAppliB","true"},
			{"ShowTaskAppliC","true"},
			{"ShowTaskAppliD","false"},
			{"ShowTaskAppliE","false"},
			{"ShowTaskAppliF","false"},
			{"ShowTaskAppliG","false"},
			{"ShowTaskAppliH","false"},
			{"ShowTaskAppliI","false"},
			{"ShowTaskAppliJ","false"},
			{"ShowTaskDelete","true"},
			{"ShowTaskMemo","true"},
			
			{"OffPop", "false"},
	        {"OffBalloon", "false"},
	        {"OffWeb", "false"},
	        {"OffMail", "false"},
	        {"OffSound", "false"},
	        {"OffAppA", "false"},
	        {"OffAppB", "false"},
	        {"OffAppC", "false"},
	        {"OffAppD", "false"},
	        {"OffAppE", "false"},
	        {"OffAppF", "false"},
	        {"OffAppG", "false"},
	        {"OffAppH", "false"},
	        {"OffAppI", "false"},
	        {"OffAppJ", "false"},
	        
	        {"IsRss", "true"},
	        {"IsPush", "true"},
	        {"IsAppPush", "false"},
	        {"pushPri", ""},
	        {"pushPub", ""},
	        {"pushAuth", ""},
	        {"pushUa", ""},
	        {"pushChId", ""},
	        {"appPushId", ""},
	        {"appPushToken", ""},
			
			{"Height","400"},
			{"Width","715"},
			
		};

		var buf = new Dictionary<string,string>();
		foreach (var k in cfg.AppSettings.Settings.AllKeys) {
			buf.Add(k, cfg.AppSettings.Settings[k].Value);
		}
		
		cfg.AppSettings.Settings.Clear();
		foreach (var k in defaultConfig.Keys) {
			var v = (buf.ContainsKey(k)) ? buf[k] : defaultConfig[k];
			cfg.AppSettings.Settings.Add(k, v);
		}
		try {
			cfg.Save();
		} catch (Exception e) {
			util.debugWriteLine(e.Message + " " + e.StackTrace);
		}
		
		// Dictionary<string, string>
	}
	public void saveFromForm(Dictionary<string, string> formData) {
		cfg = getConfig();
		
		foreach (var k in formData.Keys) {
			cfg.AppSettings.Settings[k].Value = formData[k];
			//util.debugWriteLine(k + formData[k]);
		}		
		try {
			cfg.Save();
		} catch (Exception e) {
			util.debugWriteLine(e.Message + " " + e.StackTrace);
		}
	}
//	private string[] defaultConfig = {};
}

}