using Unity.Entities;
using Unity.Mathematics;

namespace Aether.Components
{
    /// <summary>
    /// Core data for a single Aether Particle.
    /// Tracks its current state, element type, and physical properties.
    /// </summary>
    public struct AetherParticle : IComponentData
    {
        // 0 = Neutral, 1 = Fire, 2 = Ice, 3 = Sand, 4 = Electric
        public int ElementType;

        // Current state in the lifecycle
        // 0 = Idle (Wild), 1 = Gathering (Moving to Mouse), 2 = Sculpting (In Hand), 3 = Frozen/Dead
        public int State;

        // Visual variation (for shader graphs)
        public float4 Color;
        
        // Physical properties
        public float Instability; // 0.0 to 1.0 (Explodes at 1.0)
        public float VelocityDamping;
    }
}
