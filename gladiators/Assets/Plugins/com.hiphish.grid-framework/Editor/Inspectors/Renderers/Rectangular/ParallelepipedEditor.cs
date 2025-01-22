using UnityEditor;
using UnityEngine;
using GridFramework.Renderers.Rectangular;

namespace GridFramework.Editor.Renderers.Rectangular {
	/// <summary>
	///   Inspector for rectangular parallelepiped renderers.
	/// </summary>
	[CustomEditor (typeof(Parallelepiped))]
	public class ParallelepipedEditor : RendererEditor<Parallelepiped> {
		private Vector3 from, to;

		protected override void SpecificFields() {
			from = EditorGUILayout.Vector3Field("From", _renderer.From);
			to   = EditorGUILayout.Vector3Field("To"  , _renderer.To  );
		}

		protected override void AssignSpecific() {
			_renderer.From = from;
			_renderer.To   = to;
		}
	}
}
