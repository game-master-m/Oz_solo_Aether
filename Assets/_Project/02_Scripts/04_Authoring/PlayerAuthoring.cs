using Unity.Entities;
using UnityEngine;
using Aether.Components;

namespace Aether.Authoring
{
    public class PlayerAuthoring : MonoBehaviour
    {
        [Header("Settings")]
        public float moveSpeed = 5f;
        [Header("Gathering")]
        public float gatherRange = 10f;
        public float pullSpeed = 15f;

        class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerTag());
                AddComponent(entity, new PlayerMoveSpeed { Value = authoring.moveSpeed });
                AddComponent(entity, new PlayerGatheringStats { Range = authoring.gatherRange, PullSpeed = authoring.pullSpeed });
                AddComponent(entity, new PlayerInput());
            }
        }
    }
}
