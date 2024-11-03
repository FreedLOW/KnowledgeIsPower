using System;
using CodeBase.UI;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;

namespace CodeBase.StaticData.Window
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowId;
        public WindowBase Prefab;
    }
}