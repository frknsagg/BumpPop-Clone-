using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    {
       public UnityAction<Vector3> OnDrag=delegate { };
    }
}
