using UnityEngine;

namespace DefaultNamespace.Pattern
{
    public interface IVisitor
    {
        void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}