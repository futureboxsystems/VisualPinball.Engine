// Visual Pinball Engine
// Copyright (C) 2023 freezy and VPE Team
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global

using UnityEditor;
using UnityEngine;

namespace VisualPinball.Unity.Editor
{
	[CustomEditor(typeof(TableComponent))]
	[CanEditMultipleObjects]
	public class TableInspector : ItemInspector
	{
		protected override MonoBehaviour UndoTarget => target as MonoBehaviour;

		private SerializedProperty _globalDifficultyProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			_globalDifficultyProperty = serializedObject.FindProperty(nameof(TableComponent.GlobalDifficulty));
		}

		public override void OnInspectorGUI()
		{
			var tableComponent = (TableComponent) target;

			BeginEditing();

			PropertyField(_globalDifficultyProperty);

			EndEditing();

			if (!EditorApplication.isPlaying) {
				//DrawDefaultInspector();
				const string ext = "vpe";
				if (GUILayout.Button($"Save as .{ext}")) {
					var tableContainer = tableComponent.TableContainer;
					var path = EditorUtility.SaveFilePanel(
						$"Save table as .{ext}",
						"",
						tableComponent.name + $".{ext}",
						ext);

					if (!string.IsNullOrEmpty(path)) {
						var writer = new PackageWriter(tableComponent.gameObject);
						writer.WritePackageSync(path);
					}
				}
			}
		}
	}
}
