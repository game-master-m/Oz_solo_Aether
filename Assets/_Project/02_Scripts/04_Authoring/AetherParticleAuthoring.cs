using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Aether.Authoring
{
    public class AetherParticleAuthoring : MonoBehaviour
    {
        public enum AetherElement { Neutral, Fire, Ice, Sand, Electric }

        [Header("Initial Settings")]
        public AetherElement elementType = AetherElement.Neutral;
        public Color particleColor = Color.cyan;
        public float damping = 0.95f;

        class Baker : Baker<AetherParticleAuthoring>
        {
            public override void Bake(AetherParticleAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Components.AetherParticle
                {
                    ElementType = (int)authoring.elementType,
                    State = 0, // Idle
                    Color = new float4(authoring.particleColor.r, authoring.particleColor.g, authoring.particleColor.b, authoring.particleColor.a),
                    Instability = 0f,
                    VelocityDamping = authoring.damping
                });
            }
        }
    }
}
