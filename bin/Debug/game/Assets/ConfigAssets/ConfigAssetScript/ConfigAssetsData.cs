using UnityEngine;
/// <summary>
/// 获取配置的单例类
/// </summary>
public class ConfigAssetsData : MonoBehaviour
{
    static private ConfigAssetsData configAssetsDate;
    static public ConfigAssetsData instance
    { get { return configAssetsDate; } }
    static public ConfigAssetsData Instance()
    {
        if (configAssetsDate == null)
        {
            configAssetsDate = new GameObject("ConfigAssetsData").AddComponent<ConfigAssetsData>();
        }
        return configAssetsDate;
    }
    static public Enum_LanguageType languageType = 0;
    AssetBundle ab = null;
    private void Awake()
    {
        if (configAssetsDate == null) configAssetsDate = this;
        else
        {
            Debug.LogError(this.GetType().Name + "已经存在");
            Destroy(this.gameObject);
            return;
        }
    }
    private void OnDestroy()
    {
        ab?.Unload(true);
    }
    public void Init(AssetBundle configAB)
    {
        if (configAB != null) return;
        ab = configAB;
        _languageConfigAsset = GetConfigAsset<LanguageConfigAsset, LanguageConfigAsset.LanguageConfig>();
        _languageDataConfigAsset = GetConfigAsset<LanguageDataConfigAsset, LanguageDataConfigAsset.LanguageDataConfig>();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">配置容器类</typeparam>
    /// <typeparam name="V">配置类</typeparam>
    /// <returns></returns>
    T GetConfigAsset<T, V>() where T : ConfigAssetBase  where V : ConfigAssetBase.ConfigAsset
    {
#if UNITY_EDITOR
        string filePath = $"Assets/ConfigAssets/ConfigAssetBinary/{typeof(V).Name}.bytes";
        TextAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
#else
        TextAsset asset = ab.LoadAsset<TextAsset>(typeof(V).Name);
#endif
        T config = System.Activator.CreateInstance<T>();
        if (config != null)
        {
            config.ReadFromBytes(asset.bytes);
        }
        else
        {
            Debug.LogError($"{typeof(T).Name}创建失败");
        }
        return config;
    }
    public string GetLanguageText(string languageKey)
    {
        if (languageKey.IndexOf("language_") > -1)
        {
            if (languageDataConfigAsset == null) 
                Debug.LogError("LanguageData config is not init!");
            else return languageDataConfigAsset.GetLanguageText(languageKey);
        }
        else
        {
            if (languageConfigAsset == null) 
                Debug.LogError("Language config is not init!");
            else 
                return languageConfigAsset.GetLanguageText(languageKey);
        }
        Debug.LogError(languageKey + "is not in config!");
        return languageKey;
    }
    private LanguageConfigAsset _languageConfigAsset;
    public LanguageConfigAsset languageConfigAsset
    {
        get
        {
            if (_languageConfigAsset == null)
            {
                _languageConfigAsset = GetConfigAsset<LanguageConfigAsset, LanguageConfigAsset.LanguageConfig>();
            }
            return _languageConfigAsset;
        }
    }
    private LanguageDataConfigAsset _languageDataConfigAsset;
    public LanguageDataConfigAsset languageDataConfigAsset
    {
        get
        {
            if (_languageDataConfigAsset == null)
            {
                _languageDataConfigAsset = GetConfigAsset<LanguageDataConfigAsset, LanguageDataConfigAsset.LanguageDataConfig>();
            }
            return _languageDataConfigAsset;
        }
    }

}