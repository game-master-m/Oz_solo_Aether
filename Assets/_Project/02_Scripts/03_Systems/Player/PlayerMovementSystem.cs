using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Aether.Components;

namespace Aether.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct PlayerMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (velocity, input, speed) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<PlayerInput>, RefRO<PlayerMoveSpeed>>().WithAll<PlayerTag>())
            {
                float2 moveInput = input.ValueRO.MoveInput;
                float moveSpeed = speed.ValueRO.Value;
                
                float3 currentVel = velocity.ValueRO.Linear;
                
                // Apply velocity on X/Z, preserve Y (gravity)
                // Note: Camera relative movement will be added in Design Refinement
                float3 targetVel = new float3(moveInput.x * moveSpeed, currentVel.y, moveInput.y * moveSpeed);
                
                velocity.ValueRW.Linear = targetVel;
            }
        }
    }
}
