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

using System;
using System.Threading;
using UnityEngine;

namespace VisualPinball.Unity
{
	public class SoundComponent : MonoBehaviour
	{
		[SerializeField]
		private SoundAsset soundAsset;

		private CancellationTokenSource allowFadeCts;
		private CancellationTokenSource instantCts;

		public async void Play()
		{
			allowFadeCts ??= new();
			instantCts ??= new();
			try {
				await SoundUtils.Play(soundAsset, gameObject, allowFadeCts.Token, instantCts.Token);
			} catch (OperationCanceledException) { }
		}

		public void Stop()
		{
			allowFadeCts?.Cancel();
			allowFadeCts?.Dispose();
			allowFadeCts = null;
		}

		protected virtual void OnDisable()
		{
			instantCts?.Cancel();
			instantCts?.Dispose();
			instantCts = null;
			allowFadeCts?.Dispose();
			allowFadeCts = null;
		}
	}
}
