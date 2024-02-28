using UnityEngine;

/// <summary>
/// 配置容器类
/// </summary>
public abstract class ConfigAssetBase : ScriptableObject
{
    //配置基类
    public class ConfigAsset
    {
        public int ID;
    }
    public abstract string GetConfigName();
    public abstract void ReadFromBytes(byte[] bytes);
    public abstract void ReadList();
}
