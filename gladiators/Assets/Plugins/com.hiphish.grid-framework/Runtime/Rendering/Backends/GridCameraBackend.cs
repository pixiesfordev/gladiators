using UnityEngine;
using GridFramework.Renderers;

namespace GridFramework.Rendering {
	[AddComponentMenu("Grid Framework/Rendering/Grid Camera Backend")]
	[RequireComponent(typeof(Camera))]
	public class GridCameraBackend : MonoBehaviour {
		[SerializeField]
		private bool _renderWhenNotMain = false;
		private Camera _camera;

		/// <summary>
		///   Whether to render even when this is not the main camera.
		/// </summary>
		public bool RenderWhenNotMain {
			get => _renderWhenNotMain;
			set => _renderWhenNotMain = value;
		}

		void Start() {
			_camera = GetComponent<Camera>();
		}

		void OnPostRender() {
			var isMainCamera = _camera == Camera.main;

			if (!isMainCamera && !RenderWhenNotMain) {
				return;
			}

			var pollResults = System.PollAllRenderers();
			foreach (var result in pollResults) {
				var renderer = result.Key;
				var (segments, _) = result.Value;
				if (renderer.enabled) {
					Render(renderer, segments);
				}
			}
		}

		private void Render(GridRenderer renderer, RendererLineSegments segmentSets) {
			var is_wide = renderer.LineWidth != 0;
			renderer.Material.SetPass(0);

			// DANGER: For performance reason we do not compute these values
			// unless wide lines are used, but we have to initialise them in
			// advance anyway. Make sure the correct value is used.
			float mult = 0.0f, pixelSize = 0.0f;
			if (is_wide) {
				mult = 0.5f * renderer.LineWidth; //the multiplier, half the desired width
				if (_camera.orthographic) {
					pixelSize = 2f * _camera.orthographicSize / _camera.pixelHeight;
				} else {
					var p1 = _camera.ScreenToWorldPoint(50f * Vector3.forward);
					var p2 = _camera.ScreenToWorldPoint(new Vector3(20f, 0, 50f));
					pixelSize = (p1 - p2).magnitude / 20f;
				}

			}

			GL.Begin(is_wide ? GL.QUADS : GL.LINES);
			foreach (var segmentSet in segmentSets) {
				var color = segmentSet.Color;
				if (color.a < Mathf.Epsilon) {
					continue;  // Performance: skip rendering invisible segments
				}
				GL.Color(color);

				foreach (var segment in segmentSet) {
					if (segment == null) Debug.Log("ping");
					if (is_wide) {
						RenderWideSegment(segment, mult, pixelSize);
					} else {
						RenderThinSegment(segment);
					}
				}
			}
			GL.End();
		}

		private void RenderThinSegment(LineSegment segment) {
			GL.Vertex(segment.From);
			GL.Vertex(segment.To);
		}

		private void RenderWideSegment(LineSegment segment, float mult, float pixelSize) {
			var from = segment.From;
			var to   = segment.To;
			var dir = Vector3.Cross(from - to, transform.forward).normalized * pixelSize * mult;

			GL.Vertex(from - dir);
			GL.Vertex(from + dir);
			GL.Vertex(to   + dir);
			GL.Vertex(to   - dir);
		}
	}
}
