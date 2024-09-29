using Unity.Netcode.Components;

namespace ContradictiveGames.Multiplayer
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
