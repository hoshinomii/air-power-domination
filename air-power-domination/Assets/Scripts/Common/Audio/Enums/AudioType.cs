namespace RDP.Common.Audio.Enums {
	public enum AudioType {
		None,

		//OST
		BGMGame,
		BGMMenu,
		BGMCredits,
		BGMLobby,
		BGMWin,
		BGMLose,

		// Game SFX
		SfxAirstrikeBefore,
		SfxAirstrikeAfter,
		SfxReconBefore,
		SfxReconAfter,
		Sfx2Repair,
		Sfx2RepairComplete,
		Sfx2Interact,
		Sfx2InteractComplete,

		//SFX For UI
		SfxUIButtonClick,
		SfxUILobbyRoleClick,

		SfxBuildComplete,
		AmbientBGMDesert,
		BGMActionPhase,
	}
}