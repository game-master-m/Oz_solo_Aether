using Unity.Entities;
using Unity.Mathematics;

namespace Aether.Components
{
    public struct PlayerTag : IComponentData {}

    public struct PlayerMoveSpeed : IComponentData
    {
        public float Value;
    }

    public struct PlayerInput : IComponentData
    {
        public float2 MoveInput;
        public bool IsGathering;
    }

    public struct PlayerGatheringStats : IComponentData
    {
        public float Range;
        public float PullSpeed;
    }
}
