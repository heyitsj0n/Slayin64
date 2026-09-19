using UnityEngine;

/// Unity 4 -> 2022 physics compatibility (Slayin rebuild, 2026-09-19).
/// In Unity 4 a Rigidbody2D switched to kinematic stopped dead. Since Unity 5.5 a kinematic body KEEPS its
/// velocity and moves with it, so a coin that "landed" (isKinematic = true) sank through the floor, and a
/// paused entity would drift. Freeze() reproduces the old behaviour everywhere the game sets isKinematic.
public static class Rb2DCompat
{
	public static void Freeze(Rigidbody2D rb)
	{
		if (rb == null) return;
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
	}
}
