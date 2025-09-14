using UnityEngine;

namespace PizzaCompany.SaveSystem
{
    public interface IMoveableApplier
    {
        void ApplyMovement(Vector3 movement);
    }
}
