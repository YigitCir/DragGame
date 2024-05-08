namespace DefaultNamespace.Pattern
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}