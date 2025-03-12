using UnityEngine;

//using MLAPI;
//using MLAPI.NetworkVariable;
//using MLAPI.Messaging;
// using Unity.Netcode;

namespace RDP.Unit_Controls.Movement {
	public class UnitFollow : MonoBehaviour {
		/*private enum FollowBehaviorState{Moving = 0, Pausing = 1}
		
		public Transform followTarget;

		//public float followApproachDistance = 5.0f;
		private NetworkVariable<float> followApproachDistance = new NetworkVariable<float>(5.0f);

		//public float followTriggerDistance = 10.0f;
		private NetworkVariable<float> followTriggerDistance = new NetworkVariable<float>(10.0f);

		//public float speed = 4.0f;
		private NetworkVariable<float> speed = new NetworkVariable<float>(4.0f);

		private FollowBehaviorState followState = FollowBehaviorState.Pausing;

		//public Vector3 destination;
		private NetworkVariable<Vector3> destination = new NetworkVariable<Vector3>();

		//Example of NetworkVariable
		//private NetworkVariable<float> myFloat = new NetworkVariable<float>(5.0f);

		private NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

		private void Update()
		{
		    GetDestination();
		    UpdateState();
		}

		[ServerRpc(RequireOwnership = false)]
		void UpdateState()
		{
		    switch (followState)
		    {
		        case FollowBehaviorState.Moving:
		            Moving();
		            break;
		        case FollowBehaviorState.Pausing:
		            Idle();
		            break;
		        
		    } 
		}

		[ServerRpc(RequireOwnership = false)]
		private void Moving()
		{
		    if (NetworkManager.Singleton.IsServer)
		    {
		        var position = PositionOnScene();
		        transform.position = position;
		        Position.Value = position;
		    }
		    else
		    {

		        if ((followTarget.position - transform.position).magnitude <= followApproachDistance.Value)
		        {
		            followState = FollowBehaviorState.Pausing;
		        }
		        else
		        {
		            UnitMovement unitMovement = GetComponent<UnitMovement>();
		            unitMovement.SetLocation(destination.Value + new Vector3(followApproachDistance.Value, 0, followApproachDistance.Value));
		        }
		    }
		}

		[ServerRpc(RequireOwnership = false)]
		private void Idle()
		{
		    if (NetworkManager.Singleton.IsServer)
		    {
		        var position = PositionOnScene();
		        transform.position = position;
		        Position.Value = position;
		    }
		    else
		    {
		        float distance = (transform.position - destination.Value).magnitude;
		        if (distance > followTriggerDistance.Value)
		        {
		            followState = FollowBehaviorState.Moving;
		        }
		    }
		}

		[ServerRpc(RequireOwnership = false)]
		private void GetDestination()
		{
		    if (NetworkManager.Singleton.IsServer)
		    {
		        var position = PositionOnScene();
		        transform.position = position;
		        Position.Value = position;
		    }
		    else
		    {
		        if (followTarget)
		        {
		            UnitMovement unitMovement = followTarget.GetComponent<UnitMovement>();
		            if (unitMovement)
		            {
		                destination.Value = unitMovement.GetDestination();
		            }
		        }
		        else destination.Value = transform.position;
		    }
		}

		Vector3 PositionOnScene()
		{
		    return new Vector3();
		}*/
	}
}