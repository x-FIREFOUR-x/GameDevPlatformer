
namespace InputReader
{
    public interface IEntityInputSource
    {
        float HorizontalDirection { get; }
        bool Attack { get; }
        bool Jump { get; }
        bool Roll { get; }
        bool Block { get; }

        void ResetOneTimeActions();
    }
}
