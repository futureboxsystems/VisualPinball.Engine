﻿// Visual Pinball Engine
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

using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace VisualPinball.Unity
{
    [AddComponentMenu("Pinball/On-Screen Input System Button")]
    public class OnScreenInputSystemButton : OnScreenButton
    {
		[SerializeField]
		public string InputActionMapName = "";

		[SerializeField]
		public string InputActionName = "";

		private string _controlPathInternal;

		protected override void OnEnable()
		{
			var inputManager = new InputManager(InputManager.ASSETS_RESOURCES_PATH);
			var inputAction = inputManager.FindAction(InputActionMapName, InputActionName);

			if (inputAction != null && inputAction.bindings.Count > 0) {
				_controlPathInternal = inputAction.bindings[0].path;
			}

			base.OnEnable();
		}

		protected override string controlPathInternal
		{
			get => _controlPathInternal;
			set { }
		}
	}
}
