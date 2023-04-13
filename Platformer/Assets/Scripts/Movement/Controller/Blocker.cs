namespace Movement.Controller
{
    public class Blocker
    {
        public bool BlockActive { get; private set; }

        public void Block(bool activeBlock)
        {
            BlockActive = activeBlock;
        }
    }
}
