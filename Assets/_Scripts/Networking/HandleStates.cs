using UnityEngine;
using Unity.Netcode;

namespace ContradictiveGames.Multiplayer
{
    public class HandleStates
    {
        public class InputState{
            public int Tick;
            public Vector2 MoveInput;
            public Vector2 AimInput;
        }

        public class TransformStateReadWrite : INetworkSerializable {
            public int Tick;
            public Vector3 FinalPos;
            public Quaternion FinalRot;
            public bool IsMoving;

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                if(serializer.IsReader){
                    var reader = serializer.GetFastBufferReader();
                    reader.ReadValueSafe(out Tick);
                    reader.ReadValueSafe(out FinalPos);
                    reader.ReadValueSafe(out FinalRot);
                    reader.ReadValueSafe(out IsMoving);
                }
                else{
                    var writer = serializer.GetFastBufferWriter();
                    writer.WriteValueSafe(Tick);
                    writer.WriteValueSafe(FinalPos);
                    writer.WriteValueSafe(FinalRot);
                    writer.WriteValueSafe(IsMoving);
                }
            }
        }
    }
}
