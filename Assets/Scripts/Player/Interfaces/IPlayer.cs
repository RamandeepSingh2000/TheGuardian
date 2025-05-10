using Player;
using UnityEngine;

namespace Player
{
    public interface IPlayer
    {
        GameObject GetGameObject();
        Transform GetTransform();
        T GetCustomComponent<T>() where T : Component;
        void EnableInputs();
        void DisableInputs();
        Transform GetDeathEnergyTargetTransform();
        void AddDeathEnergy(float amount);
    }
}
