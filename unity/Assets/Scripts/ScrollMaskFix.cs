using System.Collections.Generic;
using UnityEngine;

// Slayin64 2026-09-19: the tavern's tk2dUIScrollableArea masks used to be depth-buffer masks, which on
// Unity 2022 painted solid black bars over the title bar and description area (they hid everything behind
// them, not just the scrolling items). The mask shader is now stencil-only; this component gives every
// renderer under the scroll Content a clone of its material on "tk2d/BlendVertexColorMasked", which skips
// pixels inside a mask rectangle. tk2dSprite re-assigns the shared collection material whenever a sprite
// changes, so the swap is re-applied every LateUpdate (a handful of renderers, cheap).
public class ScrollMaskFix : MonoBehaviour
{
	private static Shader masked;
	private static readonly Dictionary<Material, Material> clones = new Dictionary<Material, Material>();

	public static void Attach(Transform content)
	{
		if (content != null && content.GetComponent<ScrollMaskFix>() == null)
		{
			content.gameObject.AddComponent<ScrollMaskFix>();
		}
	}

	private void Awake()
	{
		if (masked == null)
		{
			masked = Resources.Load<Shader>("BlendVertexColorMasked");
			if (masked == null)
			{
				masked = Shader.Find("tk2d/BlendVertexColorMasked");
			}
		}
	}

	private void LateUpdate()
	{
		Apply();
	}

	private void OnEnable()
	{
		Apply();
	}

	private void Apply()
	{
		if (masked == null)
		{
			return;
		}
		Renderer[] rs = GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < rs.Length; i++)
		{
			Material m = rs[i].sharedMaterial;
			if (m == null || m.shader == masked)
			{
				continue;
			}
			Material c;
			if (!clones.TryGetValue(m, out c) || c == null)
			{
				c = new Material(m);
				c.shader = masked;
				c.name = m.name + " (masked)";
				clones[m] = c;
			}
			rs[i].sharedMaterial = c;
		}
	}
}
