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

using System.IO;
using NUnit.Framework;
using UnityEngine;
using VisualPinball.Engine.Test.Test;
using VisualPinball.Engine.Test.VPT.Gate;
using VisualPinball.Engine.VPT.Table;
using VisualPinball.Unity.Editor;

namespace VisualPinball.Unity.Test
{
	public class GateTests
	{
		[Test]
		public void ShouldWriteImportedGateData()
		{
			const string tmpFileName = "ShouldWriteGateData.vpx";
			var go = VpxImportEngine.ImportIntoScene(VpxPath.Gate, options: ConvertOptions.SkipNone);
			var ta = go.GetComponent<TableComponent>();
			ta.TableContainer.Export(tmpFileName);

			var writtenTable = FileTableContainer.Load(tmpFileName);
			GateDataTests.ValidateGateData(writtenTable.Gate("Data").Data);

			File.Delete(tmpFileName);
			Object.DestroyImmediate(go);
		}

	}
}
