namespace GameUI
{
    public interface IMediator
    {
        public void Notify(GameUIComponent component, Enum option);
    }
}
