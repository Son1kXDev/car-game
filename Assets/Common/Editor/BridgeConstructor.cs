using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Utils
{
    public static class BridgeConstructor
    {
        [MenuItem("GameObject/2D Object/Physics/Bridge")]
        private static void CreateNewBridge()
        {
            GameObject CurrentBridge = new GameObject("Bridge");
            BridgeContainer bridgeContainer = CurrentBridge.AddComponent<BridgeContainer>();
            bridgeContainer.SetData(
                Resources.Load("Bridge/Stake", typeof(GameObject)) as GameObject,
                Resources.Load("Bridge/Plank", typeof(GameObject)) as GameObject,
                Resources.Load("Bridge/EndStake", typeof(GameObject)) as GameObject);
            bridgeContainer.Create();
        }
    }
}