using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif  // UNITY_EDITOR
using RenderingSystem      = GridFramework.Rendering.System;
using LineSegment          = GridFramework.Rendering.LineSegment;
using LineSegmentGroup     = GridFramework.Rendering.LineSegmentGroup;
using RendererLineSegments = GridFramework.Rendering.RendererLineSegments;

namespace GridFramework.Renderers {
	/// <summary>
	///   Abstract base class for all grid renderers.
	/// </summary>
	/// <remarks>
	///   <para>
	///     In order to be visible in the scene a grid has to be rendered by a renderer.  A grid
	///     renderer is similar to Unity's own <c>MeshRenderer</c> class in that it does only the
	///     displaying job.
	///   </para>
	///   <para>
	///     The shape of the rendered grid depends on the renderer used, to change the shape you
	///     have to assign a different renderer to the <c>GameObject</c>. This class is the base
	///     class for all renderers, similar to how <c>RectGrid</c> is the base class of all grids.
	///   </para>
	/// </remarks>
	/// <seealso cref="GridFramework.Rendering.System"/>
	[ExecuteInEditMode]
	public abstract class GridRenderer : MonoBehaviour {
#region  Private variables
		[SerializeField] private float    _lineWidth = 0;
		[SerializeField] private Material _material;
		[SerializeField] private Color    _colorX = new Color(1f, 0f, 0f, .5f);
		[SerializeField] private Color    _colorY = new Color(0f, 1f, 0f, .5f);
		[SerializeField] private Color    _colorZ = new Color(0f, 0f, 1f, .5f);

		private Transform _transform;

		/// <summary>
		///   This is used to check when the Transform has changed.
		/// </summary>
		private Matrix4x4 _oldTransformMatrix;

		/// <summary>
		///   Whether something about the renderer or grid has changed since the last time points
		///   have been published.
		/// </summary>
		/// <remarks>
		///   The initial state is set to dirty by default, will force generation of points on the
		///   first run.
		/// </remarks>
		private bool isDirty = true;
#endregion

#region  Protected variables
		/// <summary>
		///   We store the draw points here for reuse.
		/// </summary>
		/// <remarks>
		///   The outer dimension is always 3 and stands for the three axes.  The middle dimension
		///   is the amount of lines per axis and it's always different. The inner dimension is
		///   always 2 and contains the two end points of each line. You need to mutate this member
		///   in <c>ComputeLines</c>.
		/// </remarks>
		protected LineSegment[][] _lineSets = new LineSegment[3][];
#endregion  // Protected variables

#region  Accessors
		/// <summary>
		///   Colour of <c>X</c>-axis lines.
		/// </summary>
		public Color ColorX {
			get => _colorX;
			set => _colorX = value;
		}

		/// <summary>
		///   Colour of <c>Y</c>-axis lines.
		/// </summary>
		public Color ColorY {
			get => _colorY;
			set => _colorY = value;
		}

		/// <summary>
		///   Colour of <c>Z</c>-axis lines.
		/// </summary>
		public Color ColorZ {
			get => _colorZ;
			set => _colorZ = value;
		}

		/// <summary>
		///   The width of the lines used when rendering the grid.
		/// </summary>
		/// <value>
		///   The width of the render line.
		/// </value>
		/// <remarks>
		///   The width of the rendered lines, if it is set to 0 all lines will be one pixel wide,
		///   otherwise they will have the specified width in world units.
		/// </remarks>
		public float LineWidth {
			get => _lineWidth;
			set => _lineWidth = value;
		}

		/// <summary>
		///   The material for rendering, if none is given it uses a default material.
		/// </summary>
		/// <remarks>
		///   <para>
		///     You can use you own material if you want control over the shader used, otherwise a
		///     default material with the following shader will be used:
		///   </para>
		///   <code>
		///     Shader "GridFramework/DefaultShader" {
		///         SubShader {
		///             Pass {
		///                 Blend SrcAlpha OneMinusSrcAlpha
		///                 ZWrite Off Cull Off Fog {
		///                     Mode Off
		///                 }
		///                 BindChannels {
		///                     Bind "vertex", vertex Bind "color", color
		///                 }
		///             }
		///         }
		///     }
		///   </code>
		///   <para>
		///     The shader itself can be found among the shaders as
		///     <c>GridFramework/DefaultShader</c>.
		///   </para>
		/// </remarks>
		public Material Material {
			get {
				if (!_material) {
					_material = new Material(Shader.Find("GridFramework/DefaultShader"));
				}
				return _material;
			}
			set => _material = value;
		}

		private Transform Transform_ {
			get {
				if (!_transform) {
					_transform = GetComponent<Transform>();
				}
				return _transform;
			}
		}
#endregion

#region  Public interface
		/// <summary>
		///   Get the line segments of a renderer.
		/// </summary>
		/// <returns>
		///   A pair; first element is the line segments, second element indicates whether the
		///   segments have been newly generated (refreshed).
		/// </returns>
		/// <remarks>
		///   If the grid has not changed the renderer can reuse the previously computed line
		///   segments. Normally this would be just an implementation detail, but this information
		///   can be useful for backends as well, which is why it is included with the result.
		/// </remarks>
		public (RendererLineSegments segments, bool refreshed) GetLineSegments() {
			var dirty = isDirty || TransformHasChanged() || GridHasChanged();
			var (xCount, yCount, zCount) = CountLines();

			_lineSets = AllocatePoints(xCount, yCount, zCount);
			if (dirty) {
				ComputeLines(_lineSets);
			}

			var lineSegments = LineSetsToLineSegmentGroup(_lineSets);
			var result = (lineSegments, dirty);

			isDirty = false;
			return result;
		}
#endregion  // Public interface

#region  Caching methods
		private RendererLineSegments LineSetsToLineSegmentGroup(LineSegment[][] lineSets) {
			var x = MakeLineSegmentGroup(ColorX, lineSets[0]);
			var y = MakeLineSegmentGroup(ColorY, lineSets[1]);
			var z = MakeLineSegmentGroup(ColorZ, lineSets[2]);

			return new RendererLineSegments(x, y, z);
		}

		private LineSegmentGroup MakeLineSegmentGroup(Color color, LineSegment[] lineSet) {
			var count = lineSet.Length;
			var segments = new LineSegment[count];
			for (var i = 0; i < count; ++i) {
				var from = lineSet[i][0];
				var to   = lineSet[i][1];
				segments[i] = new LineSegment(from, to);
			}

			return new LineSegmentGroup(color, segments);
		}

		/// <summary>
		///   Allocates a memory array for new draw points when needed.
		/// </summary>
		/// <remarks>
		///   This method first checks is the size of the individual line sets has changed or if
		///   they even exist. If so, then it simply returns false.  Otherwise the size array is
		///   updated and then the line arrays are created and all vectors set to <c>(0, 0, 0)</c>.
		/// </remarks>
		private LineSegment[][] AllocatePoints(int xCount, int yCount, int zCount) {
			var result = _lineSets;
			int[] counts = {xCount, yCount, zCount};
			for (var i = 0; i < 3; ++i) {
				// If the array already has the right size skip
				if (result[i] != null && result[i].Length == counts[i]) {
					continue;
				}

				result[i] = new LineSegment[counts[i]];

				for (var j = 0; j < counts[i]; ++j) {
					_lineSets[i][j] = new LineSegment(Vector3.zero, Vector3.zero);
				}
			}
			return result;
		}

		/// <summary>
		///   Whether the postion or rotation of the renderer have changed.
		/// </summary>
		private bool TransformHasChanged() {
			var matrix = Transform_.localToWorldMatrix;

			if (_oldTransformMatrix == matrix) {
				return false;
			}

			_oldTransformMatrix = matrix;
			return true;
		}
#endregion  // Caching methods

#region  Abstract methods
		/// <summary>
		///   Computes the coordinates of the end points of all lines.
		/// </summary>
		/// <param name="lineSets">
		///   Pre-allocated array of line segments; the outer dimension is the axis, the inner
		///   dimension are the individual lines for the respective axis.
		/// </param>
		/// <remarks>
		///   <para>
		///     This is an abstract method, the implementations are up to the subclasses. Call the
		///     method after the amount of lines is known and the <c>_lineSets</c> array is
		///     allocated.
		///   </para>
		///   <para>
		///     When writing an implementation the method must compute for every line set every
		///     line. Each line consists of two end points, the order of which does not matter.
		///   </para>
		/// </remarks>
		protected abstract void ComputeLines(LineSegment[][] lineSets);

		/// <summary>
		///   Computes the amount of lines.
		/// </summary>
		/// <returns>
		///   A triple of the number of lines along the X-, Y- and Z axis, in that order.
		/// </returns>
		/// <remarks>
		///   This is an abstract method, the implementations are up to the subclasses. This method
		///   should be called before the array is allocated. note that both <c>from</c> and
		///   <c>to</c> are references, this allows us to convert them into a common format that can
		///   be used in the subsequent calculations. For example, absolute world dimensions could
		///   be converted to relative grid dimensions and all other calculations would only need to
		///   be implemented for relative grid dimensions.
		/// </remarks>
		protected abstract (int x, int y, int z) CountLines();

		/// <summary>
		///   Indicator for whether the relevant properties of a grid have change.
		/// </summary>
		/// <returns>
		///   Whether the grid has changed since the last time this method was called.
		/// </returns>
		/// <remarks>
		///   <para>
		///     Implementations need to indicate whether the rendering-relevant properties of the
		///     grid have changed since the last time this method has been called. The result is
		///     used in order to determine whether the points of a grid need to be newly computed.
		///   </para>
		///   <para>
		///     This method should be implemented in a base class of renderers for a particular grid
		///     class. For example, there is an abstract base class for all rectangular grid
		///     renderers which implements this method. That way, even if a new renderer for
		///     rectangular grids is added, as long as it inherits from <c>RectangularRenderer</c>
		///     it will profit from that implementations.
		///   </para>
		/// </remarks>
		protected abstract bool GridHasChanged();
#endregion  // Abstract methods

#region  Visual methods
		/// <summary>
		///   Draws the grid using gizmos.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method draws the grid in the editor using gizmos. There is usually no reason to
		///     call this method manually, you should instead set the drawing flags of the grid
		///     itself. However, if you must, call this method from inside <c>OnDrawGizmos</c>.
		///   </para>
		/// </remarks>
		private void DrawGrid() {
			if (!enabled) {
				return;
			}

			var dirty = isDirty || TransformHasChanged() || GridHasChanged();
			var (xCount, yCount, zCount) = CountLines();

			_lineSets = AllocatePoints(xCount, yCount, zCount);
			if (dirty) {
				ComputeLines(_lineSets);
			}

			var lineSegments = LineSetsToLineSegmentGroup(_lineSets);

			foreach (var lineSegmentSet in lineSegments) {
				var color = lineSegmentSet.Color;
				if (Mathf.Abs(color.a) < Mathf.Epsilon) {
					continue;
				}
				Gizmos.color = color;
				foreach (var segment in lineSegmentSet) {
					if (segment == null) {
						continue;
					}
					Gizmos.DrawLine(segment.From, segment.To);
				}
			}
		}
#endregion  // Visual methods

#region  Callback Methods
		void OnEnable() {
#if UNITY_EDITOR
			Undo.undoRedoPerformed += this.MarkAsDirty;
#endif  // UNITY_EDITOR
			RenderingSystem.RegisterRenderer(this);
		}

		void OnDisable() {
#if UNITY_EDITOR
			Undo.undoRedoPerformed -= this.MarkAsDirty;
#endif  // UNITY_EDITOR
			RenderingSystem.UnregisterRenderer(this);
		}

		void OnDrawGizmos() {
			DrawGrid();
		}
#endregion

#region  Hook methods
		/// <summary>
		///   Mark that the points of the renderer have changed since the last time the renderer has
		///   been polled.
		/// </summary>
		/// <remarks>
		///   You need to call this method when you know that something about the grid or the
		///   renderer has affected the points to render. This can be the case for example whenever
		///   one of the public properties of the renderer is changed.
		/// </remarks>
		protected void MarkAsDirty() {
			isDirty = true;
		}
	}
#endregion
}
