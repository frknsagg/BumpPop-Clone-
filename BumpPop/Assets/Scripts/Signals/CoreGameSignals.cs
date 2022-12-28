using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction<GameObject> OnChangePlayer = delegate { };
        public UnityAction OnLevelFailed = delegate { };
        public UnityAction<int> OnCloneBall=delegate {  };
    }
}