﻿using System;
using UnityEngine;
using org.vxwo.csharp.json;
using System.Collections.Generic;
using ExcelConfig;

public class UAppliaction:XSingleton<UAppliaction>,ExcelConfig.IConfigLoader
{
	public UAppliaction ()
	{
		new ExcelConfig.ExcelToJSONConfigManager (this);
	}


	//private ExcelToJSONConfigManager manager;

	#region IConfigLoader implementation

	public List<T> Deserialize<T> () where T : ExcelConfig.JSONConfigBase
	{
		
		var type = typeof(T);
		var atts = type.GetCustomAttributes(typeof(ExcelConfig.ConfigFileAttribute), false) 
			as ExcelConfig.ConfigFileAttribute[];
		
		if(atts.Length>0)
		{
			var json = ResourcesManager.Singleton.LoadResources<TextAsset>("Json/" + atts[0].FileName);
			if (!json)
				return null;
			return JsonTool.Deserialize<List<T>>(json.text);
		}

		return null;
	}

	#endregion

	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

	void Update()
	{
		if (gate == null)
			return;
		gate.Tick ();
	}

	void Start()
	{
		ChangeGate (new EditorGate ());
	}

	public void ChangeGate(UGate g)
	{
		if (gate != null) {
			gate.ExitGate ();
		}
		gate = g;
		gate.JoinGate ();
	}

	private UGate gate;
}

