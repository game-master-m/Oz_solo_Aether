using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Aether.Components;

namespace Aether.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct AetherGatheringSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float3 playerPos = float3.zero;
            bool isGathering = false;
            float range = 0f;
            float pullSpeed = 0f;

            // 1. Get Player Data (Single Player assumption for prototype)
            foreach (var (localTransform, input, stats) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerInput>, RefRO<PlayerGatheringStats>>().WithAll<PlayerTag>())
            {
                playerPos = localTransform.ValueRO.Position;
                isGathering = input.ValueRO.IsGathering;
                range = stats.ValueRO.Range;
                pullSpeed = stats.ValueRO.PullSpeed;
            }

            // Early exit if no player
            if (range <= 0) return;

            // 2. Iterate Particles
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (transform, velocity, particle, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<PhysicsVelocity>, RefRW<AetherParticle>>().WithEntityAccess())
            {
                float distSq = math.distancesq(transform.ValueRO.Position, playerPos);
                
                if (isGathering && distSq < range * range)
                {
                    // STATE: GATHERING (1)
                    particle.ValueRW.State = 1;

                    // Pull towards player
                    float3 dir = math.normalize(playerPos - transform.ValueRO.Position);
                    velocity.ValueRW.Linear += dir * pullSpeed * SystemAPI.Time.DeltaTime;

                    // Collection Check
                    if (distSq < 0.5f * 0.5f)
                    {
                        ecb.DestroyEntity(entity);
                    }
                }
                else
                {
                    // STATE: IDLE (0)
                    if (particle.ValueRO.State != 0)
                    {
                        particle.ValueRW.State = 0;
                    }
                    
                    // Simple damping when not being pulled
                    velocity.ValueRW.Linear = math.lerp(velocity.ValueRO.Linear, float3.zero, SystemAPI.Time.DeltaTime * particle.ValueRO.VelocityDamping);
                }
            }
        }
    }
}
