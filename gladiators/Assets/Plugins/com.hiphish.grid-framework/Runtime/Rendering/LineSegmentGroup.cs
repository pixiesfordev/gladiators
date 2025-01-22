using System.Collections.Generic;
using UnityEngine;

namespace GridFramework.Rendering {
	/// <summary>
	///   Data class of of grid lines to render with a given colour. All segments belong to the same
	///   group, i.e. lines along the same axis.
	/// </summary>
	/// <seealso cref="LineSegment"/>
	/// <seealso cref="RendererLineSegments"/>
	public class LineSegmentGroup {
		/// <summary>
		///   Create a new group with given colour and segments.
		/// </summary>
		/// <param name="color">
		///   Colour of the segments to render.
		/// </param>
		/// <param name="segments">
		///   Array of segments, will be stored by reference.
		/// </param>
		public LineSegmentGroup(Color color, LineSegment[] segments) {
			Color = color; Segments = segments;
		}

		/// <summary> Colour of the group.  </summary>
		public Color Color {get; private set;}

		/// <summary> All the segments of the group in arbitrary order. </summary>
		public LineSegment[] Segments {get; private set;}

		/// <summary> Number of line segments in the group. </summary>
		public int Length => Segments.Length;

		/// <summary>
		///   Indexer into the array of segments, provided as a convenience over having to index the
		///   segments array itself.
		/// </summary>
		/// <param name="i"> Index into the segments array. </param>
		/// <seealso cref="Segments"/>
		public LineSegment this[int i] => Segments[i];
		
		/// <summary>
		///   The default enumerator iterates over the line segments, provided as a convenience over
		///   having to index the segments array itself.
		/// </summary>
		/// <seealso cref="Segments"/>
		public IEnumerator<LineSegment> GetEnumerator() {
			return ((IEnumerable<LineSegment>) Segments).GetEnumerator();
		}

		public override string ToString() {
			return $"LineSegmentGroup({Color}, {Segments})";
		}
	}
}
