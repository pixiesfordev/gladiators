using System;
using System.Collections.Generic;
using GridFramework.Renderers;

namespace GridFramework.Rendering {
	/// <summary>
	///   The Grid Framework rendering system.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This static class serves as the connective tissue between renderers and rendering
	///     backends. Renderers report points and changes to geometry to the rendering system. The
	///     rendering system then forwards that information to all the backends.
	///   </para>
	///   <para>
	///     Having this in-between layer lets any renderer communicate to any backend. We can add or
	///     remove renderers without having to notify the backends, and we can add or remove
	///     backends without having to notify the renderers.
	///   </para>
	/// </remarks>
	/// <seealso cref="GridFramework.Renderers.GridRenderer"/>
	static public class System {
		private static ISet<GridRenderer> renderers = new HashSet<GridRenderer>();

		/// <summary>
		///   Event which is raised when the line segments of a grid renderer have been computed
		///   instead of being reused.
		/// </summary>
		/// <seealso cref="RendererRefreshEventArgs"/>
		/// <seealso cref="PollAllRenderers"/>
		public static event RendererRefreshHandler RendererRefreshed;

		/// <summary>
		///   Event which is raised when a new renderer registers itself with the rendering system.
		/// </summary>
		/// <seealso cref="RendererUnregistered"/>
		public static event RendererRegisterHandler RendererRegistered;

		/// <summary>
		///   Event which is raised when a renderer unregisters itself from the rendering system.
		/// </summary>
		/// <seealso cref="RendererRegistered"/>
		public static event RendererRegisterHandler RendererUnregistered;

		internal static void RegisterRenderer(GridRenderer renderer) {
			renderers.Add(renderer);
			RendererRegistered?.Invoke(renderer);
		}

		internal static void UnregisterRenderer(GridRenderer renderer) {
			renderers.Remove(renderer);
			RendererUnregistered?.Invoke(renderer);
		}

		/// <summary>
		///   Unordered set of all renderers currently registered in the rendering system.
		/// </summary>
		public static ISet<GridRenderer> Renderers {
			get => renderers;
		}

		/// <summary>
		///   Prompts all active renderers to compute their line segments and returns all results.
		/// </summary>
		/// <returns>
		///   Dictionary where the key is the renderer and the value is a pair. The first item of
		///   the pair are the line segments, the second item is an indicator whether the points
		///   have been reused (<c>true</c>) or computed from scratch (<c>false</c>).
		/// <returns>
		/// <remarks>
		///   As a side effect a notification is sent out for each renderer which has been
		///   refreshed.
		/// </remarks>
		/// <seealso cref="PollRenderers(ICollection{GridRenderer})">
		/// <seealso cref="PollRenderer(GridRenderer)">
		/// <seealso cref="RendererRefreshed">
		public static IDictionary<GridRenderer, (RendererLineSegments, bool)> PollAllRenderers() {
			return PollRenderers(renderers);
		}

		/// <summary>
		///   Prompts the given active renderers to compute their line segments and returns all
		///   results.
		/// </summary>
		/// <param name="renderers">
		///   The renderers to poll.
		/// </param>
		/// <returns>
		///   Dictionary where the key is the renderer and the value is a pair. The first item of
		///   the pair are the line segments, the second item is an indicator whether the points
		///   have been reused (<c>true</c>) or computed from scratch (<c>false</c>).
		/// </returns>
		/// <remarks>
		///   As a side effect a notification is sent out for each renderer which has been
		///   refreshed.
		/// </remarks>
		/// <seealso cref="PollAllRenderers"/>
		/// <seealso cref="PollRenderer(GridRenderer)">
		/// <seealso cref="RendererRefreshed">
		public static IDictionary<GridRenderer, (RendererLineSegments, bool)>
		PollRenderers(ICollection<GridRenderer> renderers) {
			var result = new Dictionary<GridRenderer, (RendererLineSegments, bool)>();
			foreach (var renderer in renderers) {
				var (segments, refreshed) = renderer.GetLineSegments();
				if (refreshed) {
					RendererRefreshed?.Invoke(renderer, new RendererRefreshEventArgs(segments));
				}
				result.Add(renderer, (segments, refreshed));
			}
			return result;
		}

		/// <summary>
		///   Prompts a single active renderer to compute its line segments and returns the result.
		/// </summary>
		/// <param name="renderer">
		///   The renderer to poll.
		/// </param>
		/// <remarks>
		///   As a side effect a notification is sent out for the renderer if it has been refreshed.
		/// </remarks>
		/// <seealso cref="PollAllRenderers">
		/// <seealso cref="PollRenderers(ICollection{GridRenderer})">
		/// <seealso cref="RendererRefreshed">
		public static (RendererLineSegments, bool) PollRenderer(GridRenderer renderer) {
			var (segments, refreshed) = renderer.GetLineSegments();
			if (refreshed) {
				RendererRefreshed?.Invoke(renderer, new RendererRefreshEventArgs(segments));
			}
			return (segments, refreshed);
		}

		/// <summary>
		///   Delegate type for handlers of renderer refresh events.
		/// </summary>
		/// <param name="renderer">
		///   Renderer which has been refreshed.
		/// </param>
		/// <param name="args">
		///   Data describing the refresh event.
		/// </param>
		/// <seealso cref="RendererRefreshed"/>
		public delegate void RendererRefreshHandler(GridRenderer renderer, RendererRefreshEventArgs args);

		/// <summary>
		///   Delegate type for handlers of renderer registration events.
		/// </summary>
		/// <param name="renderer">
		///   Renderer whose registration status has changed.
		/// </param>
		/// <seealso cref="RendererRegistered"/>
		/// <seealso cref="RendererUnregistered"/>
		public delegate void RendererRegisterHandler(GridRenderer renderer);

		/// <summary>
		///   Data class of an argument to the grid renderer refresh event.
		/// </summary>
		/// <seealso cref="RendererRefreshed"/>
		public class RendererRefreshEventArgs : EventArgs {
			/// <summary>
			///   The grid line segments to render.
			/// </summary>
			public readonly RendererLineSegments Segments;

			/// <summary>
			///   The only constructor, set all values here.
			/// </summary>
			public RendererRefreshEventArgs(RendererLineSegments segments) {
				this.Segments = segments;
			}
		}
	}
}
