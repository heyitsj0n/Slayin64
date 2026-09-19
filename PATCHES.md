## PrefsManager.cs
91-	public static int getFamePoints()
92-	{
93:#if SLAYIN_CHEAT
94-		return 999999;   // Slayin64 Unlimited: fame points never run out
95-#else
96-		return PlayerPrefs.GetInt("FAME_POINTS", 0);
97-#endif
98-	}
99-
100-	public static void saveScore(int value)
101-	{
--
222-	public static bool getSoldItem(Item item)
223-	{
224:		// 2026-09-19: NOT forced true in Unlimited any more - "sold" means already bought, so every shop
225-		// slot showed SOLD OUT (owner). Unlimited fame points (getFamePoints) let him buy anything instead.
226-		return PlayerPrefs.GetInt("item" + item.shop + "_" + item.id, 0) == 1;
227-	}
228-
229-	public static void saveGraveItem(Item item)
230-	{
231-		PlayerPrefs.SetInt("graveitem" + item.graveType(), item.id);
232-		PlayerPrefs.Save();

## PlayerModel.cs
160-	public bool creditFM(int amount)
161-	{
162-		if (famePoints - amount >= 0)
163-		{
164:#if !SLAYIN_CHEAT
165-			famePoints -= amount;
166-#endif
167-			FPBinding(famePoints);
168-			PrefsManager.saveFamePoints(famePoints);
169-			return true;
170-		}

## AbstractBehavior.cs (z tie-break)
	private static int zTieBreak;

	private void Start()
	{
		anim = GetComponent<tk2dSpriteAnimator>();
		alive = true;
		// Unity 4 resolved equal-depth draw order consistently; Unity 2022 re-sorts equal z every frame, so two
		// overlapping slimes at z=0 flickered. A unique sub-millimetre z per entity keeps the intended layering
		// (the game separates layers by >= 0.1) and makes the tie deterministic. (Slayin rebuild 2026-09-19)
		Vector3 p = base.transform.position;
		p.z += 0.0001f * (float)((zTieBreak++ % 400) + 1);
		base.transform.position = p;
		onStart();
	}

## ShopView.cs (mask fix hook)
81-		content = shop.FindChild("ScrollableArea").FindChild("Content");
82:		ScrollMaskFix.Attach(content);   // stencil-clip the scroll items instead of black depth-mask bars (2026-09-19)
83-		cursor.parent = content;
