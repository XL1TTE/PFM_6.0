using System;
using System.Collections.Generic;

[Serializable]
public class LocalizationData
{
    public List<LocalizationSheet> sheets = new List<LocalizationSheet>();
}

[Serializable]
public class LocalizationSheet
{
    public string name;
    public List<LocalizationEntry> entries = new List<LocalizationEntry>();
}

[Serializable]
public class LocalizationEntry
{
    public string key;
    public Dictionary<string, string> translations = new Dictionary<string, string>();
}

public enum Language
{
    English,
    Russian
}