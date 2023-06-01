using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Utils
{
    public static class BridgeConstructorTool
    {
        [MenuItem("GameObject/2D Object/Physics/Bridge")]
        private static void CreateNewBridge()
        {
            GameObject CurrentBridge = new GameObject("Bridge");
            BridgeConstructor bridgeContainer = CurrentBridge.AddComponent<BridgeConstructor>();
            bridgeContainer.SetData(
                Resources.Load("InGame/Bridge/Stake", typeof(GameObject)) as GameObject,
                Resources.Load("InGame/Bridge/Plank", typeof(GameObject)) as GameObject,
                Resources.Load("InGame/Bridge/EndStake", typeof(GameObject)) as GameObject);
            bridgeContainer.Create();
        }

    }
}