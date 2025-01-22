using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GridFramework.Renderers;

namespace GridFramework.Rendering {
	/// <summary>
	///   Grid rendering backend which uses a mesh with line topology to render all grids in the
	///   scene.
	/// </summary>
	/// <remarks>
	///   <p>
	///     This rendering backend should be attached to a <c>GameObject</c> placed at the origin of the
	///     world. It automatically renders every grid in the current scene. In order to change the
	///     appearance of the grids you need to assign <c>Material</c> to the <c>MeshRenderer</c> of
	///     the object.
	///   </p>
	/// </remarks>
	/// <seealso cref="GridFramework.Rendering.GridMeshBackendLocal.cs" />
	[AddComponentMenu("Grid Framework/Rendering/Global Mesh Backend")]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class GlobalMeshBackend : MonoBehaviour {
		private const MeshTopology topology = MeshTopology.Lines;

		private readonly Func<Vector3, int, Vector3> first = (v, n) => v;
		private readonly Func<int, int, int> sum = (i, n) => i + n;

		private readonly IDictionary<GridRenderer, Mesh> meshes = new Dictionary<GridRenderer, Mesh>();
		private Mesh mesh;

		void Awake() {
			mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;

			foreach (var renderer in System.Renderers) {
				OnRendererRegister(renderer);
				var (segments, _) = System.PollRenderer(renderer);
				UpdateMesh(meshes[renderer], segments);
			}
			CombineMeshes(this.meshes.Values, this.mesh);
		}

		void Start() {
			System.RendererRegistered   += OnRendererRegister;
			System.RendererUnregistered += OnRendererUnregister;
		}

		void OnDestroy() {
			System.RendererRegistered   -= OnRendererRegister;
			System.RendererUnregistered -= OnRendererUnregister;
		}

		void FixedUpdate() {
			System.PollAllRenderers();
		}

		void OnEnable() {
			System.RendererRefreshed += OnRendererRefresh;
		}

		void OnDisable() {
			System.RendererRefreshed -= OnRendererRefresh;
		}

		private void OnRendererRegister(GridRenderer renderer) {
			meshes.Add(renderer, new Mesh());
			System.PollAllRenderers();
		}

		private void OnRendererUnregister(GridRenderer renderer) {
			meshes.Remove(renderer);
			CombineMeshes(meshes.Values, mesh);
		}

		private void OnRendererRefresh(GridRenderer renderer, System.RendererRefreshEventArgs args) {
			var mesh = meshes[renderer];
			var segments = args.Segments;
			UpdateMesh(mesh, segments);
			CombineMeshes(this.meshes.Values, this.mesh);
		}

		/// <summary>
		///   Update the values of a single mesh.
		///   <param name="segments">
		///     The new segment sets.
		///   </param>
		///   <param name="mesh">
		///     The mesh which needs to be updated. Will be mutated.
		///   </param>
		/// </summary>
		private void UpdateMesh(Mesh mesh, RendererLineSegments segments) {
			var vertices = new Vector3[2 * segments.Length];
			var indices  = new int[2 * segments.Length];

			var i = 0;
			foreach (var segmentSet in segments) {
				foreach (var segment in segmentSet) {
					for (var j = 0; j < 2; ++j) {
						vertices[i+j] = segment[j];
						indices[i+j] = i+j;
					}
					i += 2;
				}
			}

			mesh.Clear(true);
			mesh.SetVertices(vertices);
			mesh.SetIndices(indices, topology, 0);
			mesh.RecalculateBounds();
		}

		/// <summary>
		///   Combine all the individual meshes for each renderer into one common mesh.
		/// </summary>
		/// <param name="meshes">
		///   Collection of meshes to combine
		/// </param>
		/// <param name="result">
		///   Mesh object to assign the new values to. Will be mutated.
		/// </param>
		private void CombineMeshes(ICollection<Mesh> meshes, Mesh result) {
			var vertices = Flatten(from mesh in meshes select mesh.vertices, first);
			var indices = Flatten(from mesh in meshes select mesh.GetIndices(0), sum);

			result.Clear(true);
			result.SetVertices(vertices);
			result.SetIndices(indices, topology, 0);
			result.RecalculateBounds();
		}

		/// <summary>
		///   Flatten a collection of arrays of data points into one array of data points according
		///   to a provided rule.
		/// </summary>
		/// <param name="how">
		///   A function which projects the current item and the total length of data points from
		///   the previous groups onto the final value.
		/// </param>
		/// <seealso cref="System.Linq.SelectMany"/>
		private T[] Flatten<T> (IEnumerable<T[]> source, Func<T, int, T> how) {
			var length = source.Select(dataPoints => dataPoints.Length).Sum();
			var result = new T[length];

			var total = 0;
			foreach (var dataPoints in source) {
				var n = dataPoints.Length;
				for (var i = 0; i < n; ++i) {
					result[total + i] = how(dataPoints[i], total);
				}
				total += n;
			}

			return result;
		}
	}
}
