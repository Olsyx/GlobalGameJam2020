using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Utils {
    public static class GizmosUtils {
		public static void DrawCollider(Transform owner, Collider collider, Color color) {
			if (collider == null) {
				return;
			}

			Gizmos.color = color;

			if (collider is BoxCollider) {
				DrawWireCube(owner.position + ((BoxCollider)collider).center, ((BoxCollider)collider).size, owner.rotation);
			} else if (collider is SphereCollider) {
				Gizmos.DrawWireSphere(owner.position + ((SphereCollider)collider).center, ((SphereCollider)collider).radius);
			} else if (collider is MeshCollider) {
				Gizmos.DrawWireMesh(((MeshCollider)collider).sharedMesh);
			}
		}

		public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation) {
			Matrix4x4 cubeTransform = Matrix4x4.TRS(center, rotation, size);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
			Gizmos.matrix *= cubeTransform;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = oldGizmosMatrix;
		}
	}

}