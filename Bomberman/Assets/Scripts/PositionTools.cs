using UnityEngine;
using System.Collections;

public static class PositionTools {

	public static Vector3 AbsolutePosition(Vector3 v) {
		Vector2 tmp = RelativePosition(v);
		return new Vector3(tmp.x, v.y, tmp.y);
	}

	public static Vector2 RelativePosition(Vector3 v) {
		return RelativePosition(new Vector2(v.x,v.z));
	}

	public static Vector2 RelativePosition(Vector2 v) {
		float x = Mathf.Ceil(v.x - 0.5f);
		float y = Mathf.Ceil(v.y - 0.5f);
		return new Vector2(x,y);
	}

	public static Vector3 AbsoluteDirection(Vector2 dir) {
		return new Vector3(dir.x, .5f, dir.y);
	}

	public static Vector2 Position(Vector3 p) {
		return new Vector2(p.x, p.z);
	}

	public static Vector2 Abs(Vector2 v) {
		return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
	}

	public static Vector3 Abs(Vector3 v) {
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}

}