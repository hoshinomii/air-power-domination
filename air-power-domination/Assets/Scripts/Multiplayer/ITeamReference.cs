using UnityEngine;

namespace RDP.Multiplayer {
	public interface ITeamReference {
		Team GetTeam();
		void SetTeam(Team team);
	}
	
	public abstract class TeamReference : MonoBehaviour {
	
		public Team team;
		public int teamID;
		public virtual Team GetTeam() {
			return team;
		}
		
		public virtual void SetTeam(Team team, int teamID) {
			this.team = team;
			this.teamID = teamID;
		}
		
		
		
	}
		
}